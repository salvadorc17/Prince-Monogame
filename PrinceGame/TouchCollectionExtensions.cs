using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;

namespace PrinceGame
{
    public sealed class TouchCollectionExtensions
    {
        private TouchCollectionExtensions()
        {
        }
        /// <summary>
        /// Determines if there are any touches on the screen.
        /// </summary>
        /// <param name="touchState">The current TouchCollection.</param>
        /// <returns>True if there are any touches in the Pressed or Moved state, false otherwise</returns>

        public static bool AnyTouch(TouchCollection touchState)
        {
            foreach (TouchLocation location in touchState)
            {
                if (location.State == TouchLocationState.Pressed || location.State == TouchLocationState.Moved)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
