using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrinceGame
{
    class OptionsMenuScreen : MenuScreen
    {
        #region "Fields"

        //MenuEntry ungulateMenuEntry;
        private MenuEntry languageMenuEntry;
        //MenuEntry frobnicateMenuEntry;
        //MenuEntry elfMenuEntry;

        //enum Ungulate
        //{
        //    BactrianCamel,
        //    Dromedary,
        //    Llama,
        //}

        //static Ungulate currentUngulate = Ungulate.Dromedary;

        static string[] languages = {
		"Italian",
		"English"
	};

        static int currentLanguage = 1;

        //static bool frobnicate = true;
        //static int elf = 23;

        #endregion

        #region "Initialization"


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            languageMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            languageMenuEntry.Selected += LanguageMenuEntrySelected;
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        private void SetMenuEntryText()
        {
            //languageMenuEntry.Text = "Language: " + languages(currentLanguage);
        }


        #endregion

        #region "Handle Input"


        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        private void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;

            SetMenuEntryText();
        }



        #endregion
    }
}
