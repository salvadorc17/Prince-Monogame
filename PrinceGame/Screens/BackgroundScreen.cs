using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PrinceGame
{


    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class BackgroundScreen : GameScreen
    {
        #region "Fields"

        private float delay;
        private ContentManager content;
        private Texture2D textureToDisplay = null;
        private Texture2D textureToDisplay2line = null;
        private Texture2D backgroundTexture;

        private Texture2D backgroundToDisplayTexture = null;
        private Texture2D titleTexture;
        private Texture2D presentsTexture;
        private Texture2D authorsTexture;
        private Texture2D copyrightTexture;
        private Texture2D text0BackgroudTexture;
        private Texture2D text1BackgroudTexture;

        private Texture2D text2BackgroudTexture;
        private float TransitionAlphaTitle;
        private int numTexture = 0;
        private int numLoops = 2;

        public Song music;

        #endregion

        #region "Initialization"


        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundScreen()
        {
            delay = 10f;
            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(1);
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                {
                    content = new ContentManager(ScreenManager.Game.Services, "Content");
                }

                //SoundEffect a = content.Load<SoundEffect>("Sounds/dos/story1");
                //SoundEffect  a  = content.Load<SoundEffect>("SONGS/DOS/story1");

                //music = content.Load<Song>(System.Configuration.ConfigurationSettings.AppSettings["CONFIG_songs"].ToString().ToUpper() + "1");
                // play files


                //music = content.Load<Song>("Songs/main_theme");

                backgroundTexture = content.Load<Texture2D>("Backgrounds/main_background");
                presentsTexture = content.Load<Texture2D>("Backgrounds/presents");
                authorsTexture = content.Load<Texture2D>("Backgrounds/author");
                titleTexture = content.Load<Texture2D>("Backgrounds/main_title");
                copyrightTexture = content.Load<Texture2D>("Backgrounds/copyright");

                //now for the text and other background like story etc
                text0BackgroudTexture = content.Load<Texture2D>("Backgrounds/text_0_background");
                text1BackgroudTexture = content.Load<Texture2D>("Backgrounds/text_1_background");
                text2BackgroudTexture = content.Load<Texture2D>("Backgrounds/text_2_background");


                backgroundToDisplayTexture = backgroundTexture;
            }
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void Unload()
        {
            //stop music 
            MediaPlayer.Stop();

            content.Unload();
        }


        #endregion

        #region "Update and Draw"


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundToDisplayTexture, fullscreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            if (delay > 0)
            {
                delay = delay - 0.1f;
                if (textureToDisplay != null)
                {
                    TransitionAlphaTitle = TransitionAlphaTitle + 0.05f;
                    spriteBatch.Draw(textureToDisplay, new Rectangle(fullscreen.Width / 2 - textureToDisplay.Width / 2, fullscreen.Height / 2, textureToDisplay.Width, textureToDisplay.Height), new Color(TransitionAlphaTitle, TransitionAlphaTitle, TransitionAlphaTitle));
                    if (textureToDisplay2line != null)
                    {
                        spriteBatch.Draw(textureToDisplay2line, new Rectangle(fullscreen.Width / 2 - textureToDisplay2line.Width / 2, fullscreen.Height - textureToDisplay2line.Height, textureToDisplay2line.Width, textureToDisplay2line.Height), new Color(TransitionAlphaTitle, TransitionAlphaTitle, TransitionAlphaTitle));
                    }
                }
            }
            else
            {
                numTexture += 1;
                delay = 20f;
                TransitionAlphaTitle = 0;
                switch (numTexture)
                {
                    case 1:
                        textureToDisplay = presentsTexture;
                        textureToDisplay2line = null;
                        break; // TODO: might not be correct. Was : Exit Select

                    case 2:
                        textureToDisplay = authorsTexture;
                        textureToDisplay2line = null;
                        break; // TODO: might not be correct. Was : Exit Select

                    case 3:
                        textureToDisplay = titleTexture;
                        textureToDisplay2line = copyrightTexture;
                        delay = 50f;
                        break; // TODO: might not be correct. Was : Exit Select

                    case 4:
                        backgroundToDisplayTexture = text0BackgroudTexture;
                        textureToDisplay = null;
                        textureToDisplay2line = null;
                        ResetTransition();
                        delay = 40f;
                        break; // TODO: might not be correct. Was : Exit Select

                    case 5:
                        backgroundToDisplayTexture = text1BackgroudTexture;
                        textureToDisplay = null;
                        textureToDisplay2line = null;
                        ResetTransition();
                        delay = 40f;
                        break; // TODO: might not be correct. Was : Exit Select

                    case 6:
                        backgroundToDisplayTexture = text2BackgroudTexture;
                        textureToDisplay = null;
                        textureToDisplay2line = null;
                        ResetTransition();
                        delay = 40f;
                        break; // TODO: might not be correct. Was : Exit Select

                    default:

                        backgroundToDisplayTexture = backgroundTexture;
                        textureToDisplay = null;
                        textureToDisplay2line = null;
                        ResetTransition();
                        numTexture = 0;
                        numLoops += 1;
                        break; // TODO: might not be correct. Was : Exit Select

                }
            }



            spriteBatch.End();
        }


        #endregion
    }
}
