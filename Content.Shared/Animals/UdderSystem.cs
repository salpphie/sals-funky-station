// SPDX-FileCopyrightText: 2021 FoLoKe <36813380+FoLoKe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2021 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 2023 Emisse <99158783+Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Sirionaut <148076704+Sirionaut@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Cojoke <83733158+Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 MilenVolf <63782763+MilenVolf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Piras314 <p1r4s@proton.me>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2024 nikthechampiongr <32041239+nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.DoAfter;
using Content.Shared.IdentityManagement;
using Content.Shared.Mobs.Systems;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Popups;
using Content.Shared.Udder;
using Content.Shared.Verbs;
using Robust.Shared.Timing;

namespace Content.Shared.Animals;
/// <summary>
///     Gives the ability to produce milkable reagents;
///     produces endlessly if the owner does not have a HungerComponent.
/// </summary>
public sealed class UdderSystem : EntitySystem
{
    [Dependency] private readonly HungerSystem _hunger = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainerSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<UdderComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<UdderComponent, GetVerbsEvent<AlternativeVerb>>(AddMilkVerb);
        SubscribeLocalEvent<UdderComponent, MilkingDoAfterEvent>(OnDoAfter);
    }

    private void OnMapInit(EntityUid uid, UdderComponent component, MapInitEvent args)
    {
        component.NextGrowth = _timing.CurTime + component.GrowthDelay;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        var query = EntityQueryEnumerator<UdderComponent>();
        while (query.MoveNext(out var uid, out var udder))
        {
            if (_timing.CurTime < udder.NextGrowth)
                continue;

            udder.NextGrowth += udder.GrowthDelay;

            if (_mobState.IsDead(uid))
                continue;

            if (!_solutionContainerSystem.ResolveSolution(uid, udder.SolutionName, ref udder.Solution, out var solution))
                continue;

            if (solution.AvailableVolume == 0)
                continue;

            // Actually there is food digestion so no problem with instant reagent generation "OnFeed"
            if (EntityManager.TryGetComponent(uid, out HungerComponent? hunger))
            {
                // Is there enough nutrition to produce reagent?
                if (_hunger.GetHungerThreshold(hunger) < HungerThreshold.Okay)
                    continue;

                _hunger.ModifyHunger(uid, -udder.HungerUsage, hunger);
            }

            //TODO: toxins from bloodstream !?
            _solutionContainerSystem.TryAddReagent(udder.Solution.Value, udder.ReagentId, udder.QuantityPerUpdate, out _);
        }
    }

    private void AttemptMilk(Entity<UdderComponent?> udder, EntityUid userUid, EntityUid containerUid)
    {
        if (!Resolve(udder, ref udder.Comp))
            return;

        var doargs = new DoAfterArgs(EntityManager, userUid, 5, new MilkingDoAfterEvent(), udder, udder, used: containerUid)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            MovementThreshold = 1.0f,
        };

        _doAfterSystem.TryStartDoAfter(doargs);
    }

    private void OnDoAfter(Entity<UdderComponent> entity, ref MilkingDoAfterEvent args)
    {
        if (args.Cancelled || args.Handled || args.Args.Used == null)
            return;

        if (!_solutionContainerSystem.ResolveSolution(entity.Owner, entity.Comp.SolutionName, ref entity.Comp.Solution, out var solution))
            return;

        if (!_solutionContainerSystem.TryGetRefillableSolution(args.Args.Used.Value, out var targetSoln, out var targetSolution))
            return;

        args.Handled = true;
        var quantity = solution.Volume;
        if (quantity == 0)
        {
            _popupSystem.PopupClient(Loc.GetString("udder-system-dry"), entity.Owner, args.Args.User);
            return;
        }

        if (quantity > targetSolution.AvailableVolume)
            quantity = targetSolution.AvailableVolume;

        var split = _solutionContainerSystem.SplitSolution(entity.Comp.Solution.Value, quantity);
        _solutionContainerSystem.TryAddSolution(targetSoln.Value, split);

        _popupSystem.PopupClient(Loc.GetString("udder-system-success", ("amount", quantity), ("target", Identity.Entity(args.Args.Used.Value, EntityManager))), entity.Owner,
            args.Args.User, PopupType.Medium);
    }

    private void AddMilkVerb(Entity<UdderComponent> entity, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (args.Using == null ||
             !args.CanInteract ||
             !EntityManager.HasComponent<RefillableSolutionComponent>(args.Using.Value))
            return;

        var uid = entity.Owner;
        var user = args.User;
        var used = args.Using.Value;
        AlternativeVerb verb = new()
        {
            Act = () =>
            {
                AttemptMilk(uid, user, used);
            },
            Text = Loc.GetString("udder-system-verb-milk"),
            Priority = 2
        };
        args.Verbs.Add(verb);
    }
}
