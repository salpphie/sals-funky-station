// SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2021 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Brandon Hu <103440971+Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.EntityList;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityList
{
    [AdminCommand(AdminFlags.Spawn)]
    public sealed class SpawnEntityListCommand : IConsoleCommand
    {
        public string Command => "spawnentitylist";
        public string Description => "Spawns a list of entities around you";
        public string Help => $"Usage: {Command} <entityListPrototypeId>";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 1)
            {
                shell.WriteError($"Invalid arguments.\n{Help}");
                return;
            }

            if (shell.Player is not { } player)
            {
                shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
                return;
            }

            if (player.AttachedEntity is not {} attached)
            {
                shell.WriteError(Loc.GetString("shell-only-players-can-run-this-command"));
                return;
            }

            var prototypeManager = IoCManager.Resolve<IPrototypeManager>();

            if (!prototypeManager.TryIndex(args[0], out EntityListPrototype? prototype))
            {
                shell.WriteError($"No {nameof(EntityListPrototype)} found with id {args[0]}");
                return;
            }

            var entityManager = IoCManager.Resolve<IEntityManager>();
            var i = 0;

            foreach (var entity in prototype.Entities(prototypeManager))
            {
                entityManager.SpawnEntity(entity.ID, entityManager.GetComponent<TransformComponent>(attached).Coordinates);
                i++;
            }

            shell.WriteLine($"Spawned {i} entities.");
        }
    }
}
