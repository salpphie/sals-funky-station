// SPDX-FileCopyrightText: 2023 Repo <47093363+Titian3@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2024 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Localizations;
using Robust.Client.AutoGenerated;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.Info.PlaytimeStats;

[GenerateTypedNameReferences]
public sealed partial class PlaytimeStatsEntry : ContainerButton
{
    public TimeSpan Playtime { get; private set; }  // new TimeSpan property

    public PlaytimeStatsEntry(string role, TimeSpan playtime, StyleBox styleBox)
    {
        RobustXamlLoader.Load(this);

        RoleLabel.Text = role;
        Playtime = playtime;  // store the TimeSpan value directly
        PlaytimeLabel.Text = ContentLocalizationManager.FormatPlaytime(playtime);  // convert to string for display
        BackgroundColorPanel.PanelOverride = styleBox;
    }

    public void UpdateShading(StyleBoxFlat styleBox)
    {
        BackgroundColorPanel.PanelOverride = styleBox;
    }
    public string? PlaytimeText => PlaytimeLabel.Text;

    public string? RoleText => RoleLabel.Text;
}
