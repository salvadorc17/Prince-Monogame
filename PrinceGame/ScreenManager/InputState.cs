using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace PrinceGame
{


    /// <summary>
    /// Helper for reading input from keyboard, gamepad, and touch input. This class 
    /// tracks both the current and previous state of the input devices, and implements 
    /// query methods for high level input actions such as "move up through the menu"
    /// or "pause the game".
    /// </summary>
    public class InputState
    {

        public const int MaxInputs = 4;
        public readonly KeyboardState[] CurrentKeyboardStates;

        public readonly GamePadState[] CurrentGamePadStates;
        public readonly KeyboardState[] LastKeyboardStates;

        public readonly GamePadState[] LastGamePadStates;

        public readonly bool[] GamePadWasConnected;

        public TouchCollection TouchState;

        public readonly List<GestureSample> Gestures = new List<GestureSample>();

        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        public InputState()
        {
            CurrentKeyboardStates = new KeyboardState[MaxInputs];
            CurrentGamePadStates = new GamePadState[MaxInputs];

            LastKeyboardStates = new KeyboardState[MaxInputs];
            LastGamePadStates = new GamePadState[MaxInputs];

            GamePadWasConnected = new bool[MaxInputs];
        }

        /// <summary>
        /// Reads the latest state user input.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i <= MaxInputs - 1; i++)
            {
                LastKeyboardStates[i] = CurrentKeyboardStates[i];
                LastGamePadStates[i] = CurrentGamePadStates[i];

                CurrentKeyboardStates[i] = Keyboard.GetState();
                //CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

                // Keep track of whether a gamepad has ever been
                // connected, so we can detect if it is unplugged.
                if (CurrentGamePadStates[i].IsConnected)
                {
                    GamePadWasConnected[i] = true;
                }
            }

            // Get the raw touch state from the TouchPanel
            TouchState = TouchPanel.GetState();

            // Read in any detected gestures into our list for the screens to later process
            Gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                Gestures.Add(TouchPanel.ReadGesture());
            }
        }


        /// <summary>
        /// Helper for checking if a key was pressed during this update. The
        /// controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a keypress
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsKeyPressed(Keys key, System.Nullable<PlayerIndex> controllingPlayer, ref PlayerIndex playerIndex__1)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex__1 = controllingPlayer.Value;

                int i = Convert.ToInt32(playerIndex__1);

                return CurrentKeyboardStates[i].IsKeyDown(key);
            }
            else
            {
                // Accept input from any player.
                return (IsKeyPressed(key, PlayerIndex.One, ref playerIndex__1) || IsKeyPressed(key, PlayerIndex.Two, ref playerIndex__1) || IsKeyPressed(key, PlayerIndex.Three, ref playerIndex__1) || IsKeyPressed(key, PlayerIndex.Four, ref playerIndex__1));
            }
        }


        /// <summary>
        /// Helper for checking if a button was pressed during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsButtonPressed(Buttons button, System.Nullable<PlayerIndex> controllingPlayer, ref PlayerIndex playerIndex__1)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex__1 = controllingPlayer.Value;

                int i = Convert.ToInt32(playerIndex__1);

                return CurrentGamePadStates[i].IsButtonDown(button);
            }
            else
            {
                // Accept input from any player.
                return (IsButtonPressed(button, PlayerIndex.One, ref playerIndex__1) || IsButtonPressed(button, PlayerIndex.Two, ref playerIndex__1) || IsButtonPressed(button, PlayerIndex.Three, ref playerIndex__1) || IsButtonPressed(button, PlayerIndex.Four, ref playerIndex__1));
            }
        }


        /// <summary>
        /// Helper for checking if a key was newly pressed during this update. The
        /// controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a keypress
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsNewKeyPress(Keys key, System.Nullable<PlayerIndex> controllingPlayer, ref PlayerIndex playerIndex__1)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex__1 = controllingPlayer.Value;

                int i = Convert.ToInt32(playerIndex__1);

                return (CurrentKeyboardStates[i].IsKeyDown(key) && LastKeyboardStates[i].IsKeyUp(key));
            }
            else
            {
                // Accept input from any player.
                return (IsNewKeyPress(key, PlayerIndex.One, ref playerIndex__1) || IsNewKeyPress(key, PlayerIndex.Two, ref playerIndex__1) || IsNewKeyPress(key, PlayerIndex.Three, ref playerIndex__1) || IsNewKeyPress(key, PlayerIndex.Four, ref playerIndex__1));
            }
        }


        /// <summary>
        /// Helper for checking if a button was newly pressed during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsNewButtonPress(Buttons button, System.Nullable<PlayerIndex> controllingPlayer, ref PlayerIndex playerIndex__1)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex__1 = controllingPlayer.Value;

                int i = Convert.ToInt32(playerIndex__1);

                return (CurrentGamePadStates[i].IsButtonDown(button) && LastGamePadStates[i].IsButtonUp(button));
            }
            else
            {
                // Accept input from any player.
                return (IsNewButtonPress(button, PlayerIndex.One, ref playerIndex__1) || IsNewButtonPress(button, PlayerIndex.Two, ref playerIndex__1) || IsNewButtonPress(button, PlayerIndex.Three, ref playerIndex__1) || IsNewButtonPress(button, PlayerIndex.Four, ref playerIndex__1));
            }
        }
    }
}
