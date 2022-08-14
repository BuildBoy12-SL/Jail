// -----------------------------------------------------------------------
// <copyright file="JailCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Jail.Commands
{
    using System;
    using System.ComponentModel;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using UnityEngine;

    /// <inheritdoc />
    public class JailCommand : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "tpjail";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "tpcarcel", "jail" };

        /// <inheritdoc />
        public string Description { get; set; } = "Teleports the specified player to the jail.";

        /// <summary>
        /// Gets or sets the position of the jail.
        /// </summary>
        [Description("The position of the jail.")]
        public Vector3 JailPosition { get; set; } = Vector3.zero;

        /// <summary>
        /// Gets or sets the response to send when the position of the jail has not been set or is set incorrectly.
        /// </summary>
        [Description("The response to send when the position of the jail has not been set or is set incorrectly.")]
        public string JailPositionNotSetResponse { get; set; } = "The jail position has not been set or is set incorrectly.";

        /// <summary>
        /// Gets or sets the permission required to run this command.
        /// </summary>
        [Description("The permission required to run this command.")]
        public string RequiredPermission { get; set; } = "jail.jail";

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
        public string PlayerAlreadyJailedResponse { get; set; } = "{0} is already jailed.";

        /// <summary>
        /// Gets or sets the response to send when the specified player has been successfully jailed.
        /// </summary>
        [Description("The response to send when the specified player has been successfully jailed.")]
        public string PlayerJailedResponse { get; set; } = "{0} has been jailed.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(RequiredPermission))
            {
                response = InsufficientPermissionResponse;
                return false;
            }

            if (JailPosition == Vector3.zero || !PlayerMovementSync.FindSafePosition(JailPosition, out Vector3 teleportPosition))
            {
                response = JailPositionNotSetResponse;
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

            if (!JailedPlayers.Add(player))
            {
                response = string.Format(PlayerAlreadyJailedResponse, player.Nickname);
                return false;
            }

            player.Teleport(teleportPosition);
            player.ClearInventory();

            response = string.Format(PlayerJailedResponse, player.Nickname);
            return true;
        }
    }
}