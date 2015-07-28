using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PrinceGame
{
    class MessageBoxScreen : GameScreen
    {
        #region "Fields"

        private string message;

        //private Texture2D gradientTexture;
        private InputAction menuSelect;

        private InputAction menuCancel;
        #endregion

        #region "Events"

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion

        #region "Initialization"


        /// <summary>
        /// Constructor automatically includes the standard "A=ok, B=cancel"
        /// usage text prompt.
        /// </summary>
        public MessageBoxScreen(string message)
            : this(message, true)
        {
        }


        /// <summary>
        /// Constructor lets the caller specify whether to include the standard
        /// "A=ok, B=cancel" usage text prompt.
        /// </summary>
        public MessageBoxScreen(string message, bool includeUsageText)
        {
            const string usageText = " A button, Space, Enter = ok " + "B button, Esc = cancel";

            if (includeUsageText)
            {
                this.message = message + usageText;
            }
            else
            {
                this.message = message;
            }

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            menuSelect = new InputAction(new Buttons[] {
			Buttons.A,
			Buttons.Start
		}, new Keys[] {
			Keys.Space,
			Keys.Enter
		}, true);
            menuCancel = new InputAction(new Buttons[] {
			Buttons.B,
			Buttons.Back
		}, new Keys[] {
			Keys.Escape,
			Keys.Back
		}, true);
        }


        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                //gradientTexture = content.Load<Texture2D>("gradient");
                ContentManager content = ScreenManager.Game.Content;
            }
        }


        #endregion

        #region "Handle Input"


        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex = default(PlayerIndex);

            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            if (menuSelect.Evaluate(input, ControllingPlayer, ref playerIndex))
            {
                // Raise the accepted event, then exit the message box.
                if (Accepted != null)
                {
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));
                }

                ExitScreen();
            }
            else if (menuCancel.Evaluate(input, ControllingPlayer, ref playerIndex))
            {
                // Raise the cancelled event, then exit the message box.
                if (Cancelled != null)
                {
                    Cancelled(this, new PlayerIndexEventArgs(playerIndex));
                }

                ExitScreen();
            }
        }


        #endregion

        #region "Draw"


        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle(Convert.ToInt32(Math.Truncate(textPosition.X)) - hPad, Convert.ToInt32(Math.Truncate(textPosition.Y)) - vPad, Convert.ToInt32(Math.Truncate(textSize.X)) + hPad * 2, Convert.ToInt32(Math.Truncate(textSize.Y)) + vPad * 2);

            // Fade the popup alpha during transitions.
            Color color__1 = Color.White * TransitionAlpha;
            //Color color = Color.Red;

            spriteBatch.Begin();

            // Draw the background rectangle.
            //spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

            // Draw the message box text.
            spriteBatch.DrawString(font, message, textPosition, color__1);

            spriteBatch.End();
        }


        #endregion
    }

}
