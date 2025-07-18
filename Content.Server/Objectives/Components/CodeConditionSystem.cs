// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Objectives.Systems;

namespace Content.Server.Objectives.Components;

/// <summary>
/// An objective that is set to complete by code in another system.
/// Use <see cref="CodeConditionSystem"/> to check and set this.
/// </summary>
[RegisterComponent, Access(typeof(CodeConditionSystem))]
public sealed partial class CodeConditionComponent : Component
{
    /// <summary>
    /// Whether the objective is complete or not.
    /// </summary>
    [DataField]
    public bool Completed;
}
