// SPDX-FileCopyrightText: 2022 ike709 <ike709@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Forensics
{
    /// <summary>
    /// This controls fibers left by gloves on items,
    /// which the forensics system uses.
    /// </summary>
    [RegisterComponent]
    public sealed partial class FiberComponent : Component
    {
        [DataField]
        public LocId FiberMaterial = "fibers-synthetic";

        [DataField]
        public string? FiberColor;
    }
}
