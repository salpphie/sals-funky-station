// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 2023 Subversionary <109166122+Subversionary@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Vordenburg <114301317+Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Aviu00 <93730715+Aviu00@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 John Space <bigdumb421@gmail.com>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Ensnaring.Components;
/// <summary>
/// Use this on something you want to use to ensnare an entity with
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class EnsnaringComponent : Component
{
    /// <summary>
    /// How long it should take to free someone else.
    /// </summary>
    [DataField]
    public float FreeTime = 3.5f;

    /// <summary>
    /// How long it should take for an entity to free themselves.
    /// </summary>
    [DataField]
    public float BreakoutTime = 30.0f;

    /// <summary>
    /// How much should this slow down the entities walk?
    /// </summary>
    [DataField]
    public float WalkSpeed = 0.9f;

    /// <summary>
    /// How much should this slow down the entities sprint?
    /// </summary>
    [DataField]
    public float SprintSpeed = 0.9f;

    /// <summary>
    /// How much stamina does the ensnare sap
    /// </summary>
    [DataField]
    public float StaminaDamage = 55f;

    /// <summary>
    /// How many times can the ensnare be applied to the same target?
    /// </summary>
    [DataField]
    public float MaxEnsnares = 1;

    /// <summary>
    /// Should this ensnare someone when thrown?
    /// </summary>
    [DataField]
    public bool CanThrowTrigger;

    /// <summary>
    /// What is ensnared?
    /// </summary>
    [DataField]
    public EntityUid? Ensnared;

    /// <summary>
    /// Should breaking out be possible when moving?
    /// </summary>
    [DataField]
    public bool CanMoveBreakout;

    [DataField]
    public SoundSpecifier? EnsnareSound = new SoundPathSpecifier("/Audio/Effects/snap.ogg");

    /// <summary>
    /// Goobstation
    /// Should the ensaring entity be deleted upon removal?
    /// </summary>
    [DataField]
    public bool DestroyOnRemove;
}

/// <summary>
/// Used whenever you want to do something when someone becomes ensnared by the <see cref="EnsnaringComponent"/>
/// </summary>
public sealed class EnsnareEvent : EntityEventArgs
{
    public readonly float WalkSpeed;
    public readonly float SprintSpeed;

    public EnsnareEvent(float walkSpeed, float sprintSpeed)
    {
        WalkSpeed = walkSpeed;
        SprintSpeed = sprintSpeed;
    }
}

/// <summary>
/// Used whenever you want to do something when someone is freed by the <see cref="EnsnaringComponent"/>
/// </summary>
public sealed class EnsnareRemoveEvent : CancellableEntityEventArgs
{
    public readonly float WalkSpeed;
    public readonly float SprintSpeed;

    public EnsnareRemoveEvent(float walkSpeed, float sprintSpeed)
    {
        WalkSpeed = walkSpeed;
        SprintSpeed = sprintSpeed;
    }
}
