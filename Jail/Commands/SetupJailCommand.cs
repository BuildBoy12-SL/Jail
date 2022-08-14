// -----------------------------------------------------------------------
// <copyright file="SetupJailCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Jail.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using Exiled.Permissions.Extensions;
    using UnityEngine;

    /// <inheritdoc />
    public class SetupJailCommand : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "setupjail";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "setjail" };

        /// <inheritdoc />
        public string Description { get; set; } = "Sets the current jail position.";

        /// <summary>
        /// Gets or sets the permission required to run this command.
        /// </summary>
        [Description("The permission required to run this command.")]
        public string RequiredPermission { get; set; } = "jail.setup";

        /// <summary>
        /// Gets or sets the response to send when the player lacks insufficient permission to run this command.
        /// </summary>
        [Description("The response to send when the player lacks sufficient permission to run this command.")]
        public string InsufficientPermissionResponse { get; set; } = "You don't have permission to use this command.";

        /// <summary>
        /// Gets or sets the response to send when the player is not near a surface that is safe to teleport to.
        /// </summary>
        [Description("The response to send when the player is not near a surface that is safe to teleport to.")]
        public string FindSafePositionResponse { get; set; } = "You must be standing in a safe position to use this command.";

        /// <summary>
        /// Gets or sets the response to send when the position is not set successfully.
        /// </summary>
        [Description("The response to send when the position is not set successfully.")]
        public string FailResponse { get; set; } = "Failed to set jail position.";

        /// <summary>
        /// Gets or sets the response to send when the position is set successfully.
        /// </summary>
        [Description("The response to send when the position is set successfully.")]
        public string SuccessResponse { get; set; } = "Jail position set successfully.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(RequiredPermission))
            {
                response = InsufficientPermissionResponse;
                return false;
            }

            Player player = Player.Get(sender);
            if (!PlayerMovementSync.FindSafePosition(player.Position, out Vector3 jailPosition))
            {
                response = FindSafePositionResponse;
                return false;
            }

            if (!TrySetPosition(jailPosition))
            {
                response = FailResponse;
                return false;
            }

            response = SuccessResponse;
            return true;
        }

        private static bool TrySetPosition(Vector3 position)
        {
            SortedDictionary<string, IConfig> sortedConfigs = Exiled.Loader.ConfigManager.LoadSorted(Exiled.Loader.ConfigManager.Read());
            if (!sortedConfigs.TryGetValue(Plugin.Instance.Prefix, out IConfig config) || config is not Config jailConfig)
            {
                Log.Error("Could not find the jail config when setting the position.");
                return false;
            }

            jailConfig.JailCommand.JailPosition = position;
            Exiled.Loader.ConfigManager.Save(sortedConfigs);
            Plugin.Instance.OnUnregisteringCommands();
            Plugin.Instance.OnRegisteringCommands();
            return true;
        }
    }
}