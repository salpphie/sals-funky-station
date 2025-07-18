// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Alert;
using Content.Shared.Whitelist;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Silicons.Borgs.Components;

/// <summary>
/// This is used for the core body of a borg. This manages a borg's
/// "brain", legs, modules, and battery. Essentially the master component
/// for borg logic.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedBorgSystem)), AutoGenerateComponentState]
public sealed partial class BorgChassisComponent : Component
{
    #region Brain
    /// <summary>
    /// A whitelist for which entities count as valid brains
    /// </summary>
    [DataField("brainWhitelist")]
    public EntityWhitelist? BrainWhitelist;

    /// <summary>
    /// The container ID for the brain
    /// </summary>
    [DataField("brainContainerId")]
    public string BrainContainerId = "borg_brain";

    [ViewVariables(VVAccess.ReadWrite)]
    public ContainerSlot BrainContainer = default!;

    public EntityUid? BrainEntity => BrainContainer.ContainedEntity;
    #endregion

    #region Modules
    /// <summary>
    /// A whitelist for what types of modules can be installed into this borg
    /// </summary>
    [DataField("moduleWhitelist")]
    public EntityWhitelist? ModuleWhitelist;

    /// <summary>
    /// How many modules can be installed in this borg
    /// </summary>
    [DataField("maxModules"), ViewVariables(VVAccess.ReadWrite)]
    public int MaxModules = 3;

    /// <summary>
    /// The ID for the module container
    /// </summary>
    [DataField("moduleContainerId")]
    public string ModuleContainerId = "borg_module";

    [ViewVariables(VVAccess.ReadWrite)]
    public Container ModuleContainer = default!;

    public int ModuleCount => ModuleContainer.ContainedEntities.Count;
    #endregion

    /// <summary>
    /// The currently selected module
    /// </summary>
    [DataField("selectedModule"), AutoNetworkedField]
    public EntityUid? SelectedModule;

    #region Visuals
    [DataField("hasMindState")]
    public string HasMindState = string.Empty;

    [DataField("noMindState")]
    public string NoMindState = string.Empty;
    #endregion

    [DataField]
    public ProtoId<AlertPrototype> BatteryAlert = "BorgBattery";

    [DataField]
    public ProtoId<AlertPrototype> NoBatteryAlert = "BorgBatteryNone";
}

[Serializable, NetSerializable]
public enum BorgVisuals : byte
{
    HasPlayer
}

[Serializable, NetSerializable]
public enum BorgVisualLayers : byte
{
    /// <summary>
    /// Main borg body layer.
    /// </summary>
    Body,

    /// <summary>
    /// Layer for the borg's mind state.
    /// </summary>
    Light,

    /// <summary>
    /// Layer for the borg flashlight status.
    /// </summary>
    LightStatus,
}
