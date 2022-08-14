// -----------------------------------------------------------------------
// <copyright file="JailedPlayer.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Jail.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using UnityEngine;

    /// <summary>
    /// Represents a jailed player.
    /// </summary>
    public class JailedPlayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JailedPlayer"/> class.
        /// </summary>
        /// <param name="player">The player that was jailed.</param>
        public JailedPlayer(Player player)
        {
            UserId = player.UserId;
            Position = player.Position;
            Zone = player.Zone;
            Items = player.Items.Select(item => item.Type).ToList();
            Ammo = player.Ammo;
        }

        /// <summary>
        /// Gets the jailed player's id.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Gets the old position of the player.
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        /// Gets the old zone of the player.
        /// </summary>
        public ZoneType Zone { get; }

        /// <summary>
        /// Gets the old items of the player.
        /// </summary>
        public List<ItemType> Items { get; }

        /// <summary>
        /// Gets the old ammo of the player.
        /// </summary>
        public Dictionary<ItemType, ushort> Ammo { get; }
    }
}