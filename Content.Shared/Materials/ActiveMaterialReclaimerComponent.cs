// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Materials;

/// <summary>
/// Tracker component for the process of reclaiming entities
/// <seealso cref="MaterialReclaimerComponent"/>
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedMaterialReclaimerSystem)), AutoGenerateComponentPause]
public sealed partial class ActiveMaterialReclaimerComponent : Component
{
    /// <summary>
    /// Container used to store the item currently being reclaimed
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public Container ReclaimingContainer = default!;

    /// <summary>
    /// When the reclaiming process ends.
    /// </summary>
    [DataField("endTime", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoPausedField]
    public TimeSpan EndTime;

    /// <summary>
    /// The length of the reclaiming process.
    /// Used for calculations.
    /// </summary>
    [DataField("duration"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan Duration;
}
