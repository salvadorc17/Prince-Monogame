using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PrinceGame
{
    class MainMenuScreen : MenuScreen
    {
        #region "Initialization"


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base(string.Empty)
        {
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry(string.Empty);
            //MenuEntry optionsMenuEntry = new MenuEntry("Options");
            //MenuEntry exitMenuEntry = new MenuEntry(string.Empty);

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            //optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            //exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            //MenuEntries.Add(optionsMenuEntry);
            //MenuEntries.Add(exitMenuEntry);
            MenuEntries.Add(playGameMenuEntry);
        }


        #endregion

        #region "Handle Input"


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,new GameplayScreen());
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new PrinceOfPersiaGame());
        }


        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
