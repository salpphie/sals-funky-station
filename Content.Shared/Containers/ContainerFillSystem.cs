// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 2025 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using System.Linq;
using System.Numerics;
using Content.Shared.EntityTable;
using Robust.Shared.Containers;
using Robust.Shared.Map;

namespace Content.Shared.Containers;

public sealed class ContainerFillSystem : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _containerSystem = default!;
    [Dependency] private readonly EntityTableSystem _entityTable = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ContainerFillComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<EntityTableContainerFillComponent, MapInitEvent>(OnTableMapInit);
    }

    private void OnMapInit(EntityUid uid, ContainerFillComponent component, MapInitEvent args)
    {
        if (!TryComp(uid, out ContainerManagerComponent? containerComp))
            return;

        var xform = Transform(uid);
        var coords = new EntityCoordinates(uid, Vector2.Zero);

        foreach (var (contaienrId, prototypes) in component.Containers)
        {
            if (!_containerSystem.TryGetContainer(uid, contaienrId, out var container, containerComp))
            {
                Log.Error($"Entity {ToPrettyString(uid)} with a {nameof(ContainerFillComponent)} is missing a container ({contaienrId}).");
                continue;
            }

            foreach (var proto in prototypes)
            {
                var ent = Spawn(proto, coords);
                if (!_containerSystem.Insert(ent, container, containerXform: xform))
                {
                    var alreadyContained = container.ContainedEntities.Count > 0 ? string.Join("\n", container.ContainedEntities.Select(e => $"\t - {EntityManager.ToPrettyString(e)}")) : "< empty >";
                    Log.Error($"Entity {ToPrettyString(uid)} with a {nameof(ContainerFillComponent)} failed to insert an entity: {ToPrettyString(ent)}.\nCurrent contents:\n{alreadyContained}");
                    _transform.AttachToGridOrMap(ent);
                    break;
                }
            }
        }
    }

    private void OnTableMapInit(Entity<EntityTableContainerFillComponent> ent, ref MapInitEvent args)
    {
        if (!TryComp(ent, out ContainerManagerComponent? containerComp))
            return;

        if (TerminatingOrDeleted(ent) || !Exists(ent))
            return;

        var xform = Transform(ent);
        var coords = new EntityCoordinates(ent, Vector2.Zero);

        foreach (var (containerId, table) in ent.Comp.Containers)
        {
            if (!_containerSystem.TryGetContainer(ent, containerId, out var container, containerComp))
            {
                Log.Error($"Entity {ToPrettyString(ent)} with a {nameof(EntityTableContainerFillComponent)} is missing a container ({containerId}).");
                continue;
            }

            var spawns = _entityTable.GetSpawns(table);
            foreach (var proto in spawns)
            {
                var spawn = Spawn(proto, coords);
                if (!_containerSystem.Insert(spawn, container, containerXform: xform))
                {
                    var alreadyContained = container.ContainedEntities.Count > 0 ? string.Join("\n", container.ContainedEntities.Select(e => $"\t - {EntityManager.ToPrettyString(e)}")) : "< empty >";
                    Log.Error($"Entity {ToPrettyString(ent)} with a {nameof(EntityTableContainerFillComponent)} failed to insert an entity: {ToPrettyString(spawn)}.\nCurrent contents:\n{alreadyContained}");
                    _transform.AttachToGridOrMap(spawn);
                    break;
                }
            }
        }
    }
}
