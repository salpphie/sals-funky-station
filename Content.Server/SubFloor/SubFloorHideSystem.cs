// SPDX-FileCopyrightText: 2022 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2024 MilenVolf <63782763+MilenVolf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Eye;
using Content.Shared.SubFloor;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Player;

namespace Content.Server.SubFloor;

public sealed class SubFloorHideSystem : SharedSubFloorHideSystem
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly SharedEyeSystem _eye = default!;

    private HashSet<ICommonSession> _showFloors = new();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<ShowSubfloorRequestEvent>(OnShowSubfloor);
        SubscribeLocalEvent<GetVisMaskEvent>(OnGetVisibility);

        _player.PlayerStatusChanged += OnPlayerStatus;
    }

    private void OnPlayerStatus(object? sender, SessionStatusEventArgs e)
    {
        if (e.NewStatus == SessionStatus.Connected)
            return;

        _showFloors.Remove(e.Session);

        if (e.Session.AttachedEntity != null)
            _eye.RefreshVisibilityMask(e.Session.AttachedEntity.Value);
    }

    private void OnGetVisibility(ref GetVisMaskEvent ev)
    {
        if (!TryComp(ev.Entity, out ActorComponent? actor))
            return;

        if (_showFloors.Contains(actor.PlayerSession))
        {
            ev.VisibilityMask |= (int)VisibilityFlags.Subfloor;
        }
    }

    private void OnShowSubfloor(ShowSubfloorRequestEvent ev, EntitySessionEventArgs args)
    {
        // TODO: Commands are a bit of an eh? for client-only but checking shared perms
        var ent = args.SenderSession.AttachedEntity;

        if (!TryComp(ent, out EyeComponent? eyeComp))
            return;

        if (ev.Value)
        {
            _showFloors.Add(args.SenderSession);
        }
        else
        {
            _showFloors.Remove(args.SenderSession);
        }

        _eye.RefreshVisibilityMask((ent.Value, eyeComp));

        RaiseNetworkEvent(new ShowSubfloorRequestEvent()
        {
            Value = ev.Value,
        }, args.SenderSession);
    }
}
