using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace PrinceGame
{
    class PlayerIndexEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            this.m_playerIndex = playerIndex;
        }


        /// <summary>
        /// Gets the index of the player who triggered this event.
        /// </summary>
        public PlayerIndex PlayerIndex
        {
            get { return m_playerIndex; }
        }

        private PlayerIndex m_playerIndex;
    }

}
