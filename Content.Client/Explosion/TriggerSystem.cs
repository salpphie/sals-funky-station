// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Client.Explosion;

public sealed partial class TriggerSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        InitializeProximity();
    }
}
