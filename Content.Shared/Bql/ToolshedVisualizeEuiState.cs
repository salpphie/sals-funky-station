// SPDX-FileCopyrightText: 2023 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.Bql;

[Serializable, NetSerializable]
public sealed class ToolshedVisualizeEuiState : EuiStateBase
{
    public readonly (string name, NetEntity entity)[] Entities;

    public ToolshedVisualizeEuiState((string name, NetEntity entity)[] entities)
    {
        Entities = entities;
    }
}
