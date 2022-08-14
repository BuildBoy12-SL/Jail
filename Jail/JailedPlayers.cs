// -----------------------------------------------------------------------
// <copyright file="JailedPlayers.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Jail
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Features;
    using Jail.Models;

    /// <summary>
    /// Manages a <see cref="HashSet{T}"/> of <see cref="JailedPlayer"/>s.
    /// </summary>
    public static class JailedPlayers
    {
        private static readonly HashSet<JailedPlayer> JailedPlayersValue = new();

        /// <summary>
        /// Adds a jailed player.
        /// </summary>
        /// <param name="player">The player to add to the set.</param>
        /// <returns>Whether the player was added successfully.</returns>
        public static bool Add(Player player) => Get(player) is null && JailedPlayersValue.Add(new JailedPlayer(player));

        /// <summary>
        /// Gets a jailed player.
        /// </summary>
        /// <param name="player">The player to get.</param>
        /// <returns>The <see cref="JailedPlayer"/>, or <see langword="null"/> if a corresponding model is not found.</returns>
        public static JailedPlayer Get(Player player) => JailedPlayersValue.FirstOrDefault(jailedPlayer => jailedPlayer.UserId == player.UserId);

        /// <summary>
        /// Removes a jailed player.
        /// </summary>
        /// <param name="jailedPlayer">The player to remove from the set.</param>
        /// <returns>Whether the player was removed successfully.</returns>
        public static bool Remove(JailedPlayer jailedPlayer) => JailedPlayersValue.Remove(jailedPlayer);

        /// <summary>
        /// Clears all jailed players.
        /// </summary>
        public static void Clear() => JailedPlayersValue.Clear();
    }
}