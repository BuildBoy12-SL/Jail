// -----------------------------------------------------------------------
// <copyright file="ReturnCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Jail.Commands
{
    using System;
    using System.ComponentModel;
    using CommandSystem;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using Jail.Models;
    using MonoMod.Utils;

    /// <inheritdoc />
    public class ReturnCommand : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "tpreturn";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "tpregresar", "unjail" };

        /// <inheritdoc />
        public string Description { get; set; } = "Returns a jailed player to their last position.";

        /// <summary>
        /// Gets or sets the permission required to run this command.
        /// </summary>
        [Description("The permission required to run this command.")]
        public string RequiredPermission { get; set; } = "jail.return";

        /// <summary>
        /// Gets or sets the response to send when the player lacks insufficient permission to run this command.
        /// </summary>
        [Description("The response to send when the player lacks sufficient permission to run this command.")]
        public string InsufficientPermissionResponse { get; set; } = "You don't have permission to use this command.";

        /// <summary>
        /// Gets or sets the response to send when a player is not specified.
        /// </summary>
        [Description("The response to send when a player is not specified.")]
        public string SpecifyPlayerResponse { get; set; } = "You must specify a player to jail.";

        /// <summary>
        /// Gets or sets the response to send when the specified player is not found.
        /// </summary>
        [Description("The response to send when the specified player is not found.")]
        public string PlayerNotFoundResponse { get; set; } = "Player not found.";

        /// <summary>
        /// Gets or sets the response to send when the specified player is already jailed.
        /// </summary>
        [Description("The response to send when the specified player is already jailed.")]
        public string PlayerNotJailedResponse { get; set; } = "{0} is not jailed.";

        /// <summary>
        /// Gets or sets the response to send when the specified player has been successfully jailed.
        /// </summary>
        [Description("The response to send when the specified player has been successfully jailed.")]
        public string PlayerUnjailedResponse { get; set; } = "{0} has been removed from the jail.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(RequiredPermission))
            {
                response = InsufficientPermissionResponse;
                return false;
            }

            if (arguments.Count == 0)
            {
                response = SpecifyPlayerResponse;
                return false;
            }

            Player player = Player.Get(arguments.At(0));
            if (player == null)
            {
                response = PlayerNotFoundResponse;
                return false;
            }

            JailedPlayer jailedPlayer = JailedPlayers.Get(player);
            if (jailedPlayer is null || !JailedPlayers.Remove(jailedPlayer))
            {
                response = string.Format(PlayerNotJailedResponse, player.Nickname);
                return false;
            }

            player.Position = jailedPlayer.Position;
            player.AddItem(jailedPlayer.Items);
            player.Ammo.AddRange(jailedPlayer.Ammo);

            if (jailedPlayer.Zone != ZoneType.Surface && Warhead.IsDetonated)
                player.Kill(DamageType.Warhead);

            response = string.Format(PlayerUnjailedResponse, player.Nickname);
            return true;
        }
    }
}