using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace PrinceGame
{


    /// <summary>
    /// Enum describes the screen transition state.
    /// </summary>
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden
    }


    /// <summary>
    /// A screen is a single layer that has update and draw logic, and which
    /// can be combined with other layers to build up a complex menu system.
    /// For instance the main menu, the options menu, the "are you sure you
    /// want to quit" message box, and the main game itself are all implemented
    /// as screens.
    /// </summary>
    public abstract class GameScreen
    {
        /// <summary>
        /// Normally when one screen is brought up over the top of another,
        /// the first screen will transition off to make room for the new
        /// one. This property indicates whether the screen is only a small
        /// popup, in which case screens underneath it do not need to bother
        /// transitioning off.
        /// </summary>
        public bool IsPopup
        {
            get { return m_isPopup; }
            protected set { m_isPopup = value; }
        }


        private bool m_isPopup = false;

        /// <summary>
        /// Indicates how long the screen takes to
        /// transition on when it is activated.
        /// </summary>
        public TimeSpan TransitionOnTime
        {
            get { return m_transitionOnTime; }
            protected set { m_transitionOnTime = value; }
        }


        private TimeSpan m_transitionOnTime = TimeSpan.Zero;

        /// <summary>
        /// Indicates how long the screen takes to
        /// transition off when it is deactivated.
        /// </summary>
        public TimeSpan TransitionOffTime
        {
            get { return m_transitionOffTime; }
            protected set { m_transitionOffTime = value; }
        }


        private TimeSpan m_transitionOffTime = TimeSpan.Zero;

        /// <summary>
        /// Gets the current position of the screen transition, ranging
        /// from zero (fully active, no transition) to one (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionPosition
        {
            get { return m_transitionPosition; }
            protected set { m_transitionPosition = value; }
        }


        private float m_transitionPosition = 1;

        /// <summary>
        /// Gets the current alpha of the screen transition, ranging
        /// from 1 (fully active, no transition) to 0 (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }


        /// <summary>
        /// Gets the current screen transition state.
        /// </summary>
        public ScreenState ScreenState
        {
            get { return m_screenState; }
            set { m_screenState = value; }
        }


        private ScreenState m_screenState = ScreenState.TransitionOn;

        /// <summary>
        /// There are two possible reasons why a screen might be transitioning
        /// off. It could be temporarily going away to make room for another
        /// screen that is on top of it, or it could be going away for good.
        /// This property indicates whether the screen is exiting for real:
        /// if set, the screen will automatically remove itself as soon as the
        /// transition finishes.
        /// </summary>
        public bool IsExiting
        {
            get { return m_isExiting; }
            protected internal set { m_isExiting = value; }
        }


        private bool m_isExiting = false;

        /// <summary>
        /// Checks whether this screen is active and can respond to user input.
        /// </summary>
        public bool IsActive
        {
            get { return !otherScreenHasFocus && (m_screenState == ScreenState.TransitionOn || m_screenState == ScreenState.Active); }
        }


        private bool otherScreenHasFocus;

        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return m_screenManager; }
            internal set { m_screenManager = value; }
        }


        private ScreenManager m_screenManager;

        /// <summary>
        /// Gets the index of the player who is currently controlling this screen,
        /// or null if it is accepting input from any player. This is used to lock
        /// the game to a specific player profile. The main menu responds to input
        /// from any connected gamepad, but whichever player makes a selection from
        /// this menu is given control over all subsequent screens, so other gamepads
        /// are inactive until the controlling player returns to the main menu.
        /// </summary>
        public System.Nullable<PlayerIndex> ControllingPlayer
        {
            get { return m_controllingPlayer; }
            internal set { m_controllingPlayer = value; }
        }


        private System.Nullable<PlayerIndex> m_controllingPlayer;

        /// <summary>
        /// Gets the gestures the screen is interested in. Screens should be as specific
        /// as possible with gestures to increase the accuracy of the gesture engine.
        /// For example, most menus only need Tap or perhaps Tap and VerticalDrag to operate.
        /// These gestures are handled by the ScreenManager when screens change and
        /// all gestures are placed in the InputState passed to the HandleInput method.
        /// </summary>
        public GestureType EnabledGestures
        {
            get { return m_enabledGestures; }
            protected set
            {
                m_enabledGestures = value;

                // the screen manager handles this during screen changes, but
                // if this screen is active and the gesture types are changing,
                // we have to update the TouchPanel ourself.
                if (ScreenState == ScreenState.Active)
                {
                    TouchPanel.EnabledGestures = value;
                }
            }
        }


        private GestureType m_enabledGestures = GestureType.None;
        /// <summary>
        /// Gets whether or not this screen is serializable. If this is true,
        /// the screen will be recorded into the screen manager's state and
        /// its Serialize and Deserialize methods will be called as appropriate.
        /// If this is false, the screen will be ignored during serialization.
        /// By default, all screens are assumed to be serializable.
        /// </summary>
        public bool IsSerializable
        {
            get { return m_isSerializable; }
            protected set { m_isSerializable = value; }
        }


        private bool m_isSerializable = true;

        /// <summary>
        /// Activates the screen. Called when the screen is added to the screen manager or if the game resumes
        /// from being paused or tombstoned.
        /// </summary>
        /// <param name="instancePreserved">
        /// True if the game was preserved during deactivation, false if the screen is just being added or if the game was tombstoned.
        /// On Xbox and Windows this will always be false.
        /// </param>
        public virtual void Activate(bool instancePreserved)
        {
        }


        /// <summary>
        /// Deactivates the screen. Called when the game is being deactivated due to pausing or tombstoning.
        /// </summary>
        public virtual void Deactivate()
        {
        }


        /// <summary>
        /// Unload content for the screen. Called when the screen is removed from the screen manager.
        /// </summary>
        public virtual void Unload()
        {
        }


        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (m_isExiting)
            {
                // If the screen is going away to die, it should transition off.
                m_screenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, m_transitionOffTime, 1))
                {
                    // When the transition finishes, remove the screen.
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (coveredByOtherScreen)
            {
                // If the screen is covered by another, it should transition off.
                if (UpdateTransition(gameTime, m_transitionOffTime, 1))
                {
                    // Still busy transitioning.
                    m_screenState = ScreenState.TransitionOff;
                }
                else
                {
                    // Transition finished!
                    m_screenState = ScreenState.Hidden;
                }
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                if (UpdateTransition(gameTime, m_transitionOnTime, -1))
                {
                    // Still busy transitioning.
                    m_screenState = ScreenState.TransitionOn;
                }
                else
                {
                    // Transition finished!
                    m_screenState = ScreenState.Active;
                }
            }
        }


        public void ResetTransition()
        {
            m_transitionPosition = 1;
        }

        /// <summary>
        /// Helper for updating the screen transition position.
        /// </summary>
        private bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            // How much should we move by?
            float transitionDelta = 0;

            if (time == TimeSpan.Zero)
            {
                transitionDelta = 1;
            }
            else
            {
                transitionDelta = Convert.ToSingle(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);
            }

            // Update the transition position.
            m_transitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (m_transitionPosition <= 0)) || ((direction > 0) && (m_transitionPosition >= 1)))
            {
                m_transitionPosition = MathHelper.Clamp(m_transitionPosition, 0, 1);
                return false;
            }

            // Otherwise we are still busy transitioning.
            return true;
        }


        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        public virtual void HandleInput(GameTime gameTime, InputState input)
        {
        }


        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>

        public virtual void Draw(GameTime gameTime)
        {

        }


        /// <summary>
        /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
        /// instantly kills the screen, this method respects the transition timings
        /// and will give the screen a chance to gradually transition off.
        /// </summary>
        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                // If the screen has a zero transition time, remove it immediately.
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                m_isExiting = true;
            }
        }
    }
}
