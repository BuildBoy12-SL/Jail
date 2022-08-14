// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Jail
{
    using Exiled.API.Interfaces;
    using Jail.Commands;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Commands.JailCommand"/> command.
        /// </summary>
        public JailCommand JailCommand { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Commands.ReturnCommand"/> command.
        /// </summary>
        public ReturnCommand ReturnCommand { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Commands.SetupJailCommand"/> command.
        /// </summary>
        public SetupJailCommand SetupJailCommand { get; set; } = new();
    }
}