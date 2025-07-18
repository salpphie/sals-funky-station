// SPDX-FileCopyrightText: 2022 Moony <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Vordenburg <114301317+Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Shuttles.Systems;
using Content.Server.Station.Systems;
using Robust.Shared.Serialization.TypeSerializers.Implementations;
using Robust.Shared.Utility;

namespace Content.Server.Station.Components;

/// <summary>
/// Stores core information about a station, namely its config and associated grids.
/// All station entities will have this component.
/// </summary>
[RegisterComponent, Access(typeof(StationSystem))]
public sealed partial class StationDataComponent : Component
{
    /// <summary>
    /// The game map prototype, if any, associated with this station.
    /// </summary>
    [DataField("stationConfig")]
    public StationConfig? StationConfig = null;

    /// <summary>
    /// List of all grids this station is part of.
    /// </summary>
    [DataField("grids")]
    public HashSet<EntityUid> Grids = new();
}
