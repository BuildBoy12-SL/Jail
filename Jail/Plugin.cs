// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Jail
{
    using System;
    using System.Reflection;
    using CommandSystem;
    using Exiled.API.Features;
    using RemoteAdmin;

    /// <inheritdoc />
    public class Plugin : Plugin<Config>
    {
        /// <summary>
        /// Gets a static instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <inheritdoc/>
        public override string Author => "Build";

        /// <inheritdoc/>
        public override string Name => "Jail";

        /// <inheritdoc/>
        public override string Prefix => "Jail";

        /// <inheritdoc/>
        public override Version Version { get; } = new(1, 0, 0);

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new(5, 2, 2);

        /// <inheritdoc />
        public override void OnEnabled()
        {
            Instance = this;
            Exiled.Events.Handlers.Server.WaitingForPlayers += JailedPlayers.Clear;
            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= JailedPlayers.Clear;
            Instance = null;
            base.OnDisabled();
        }

        /// <inheritdoc />
        public override void OnRegisteringCommands()
        {
            base.OnRegisteringCommands();
            foreach (PropertyInfo property in Config.GetType().GetProperties())
            {
                if (property.GetValue(Config) is not ICommand command)
                    continue;

                CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(command);
                Commands[typeof(RemoteAdminCommandHandler)][command.GetType()] = command;
            }
        }
    }
}