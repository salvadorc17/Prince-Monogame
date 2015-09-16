using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrinceGame
{
    public interface IScreenFactory
    {
        /// <summary>
        /// Creates a GameScreen from the given type.
        /// </summary>
        /// <param name="screenType">The type of screen to create.</param>
        /// <returns>The newly created screen.</returns>
        GameScreen CreateScreen(Type screenType);
    }
}
