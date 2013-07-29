#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MyLife
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Initialization

        int NumberOfButtons = 4;
        float MarginRatio = 0.5f;
        int buttonSize;
        int margin;

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("")
        {
            Title = GlobalVariables.Game.Content.Load<Texture2D>("title");

            buttonSize = (int)(GlobalVariables.ScreenWidth / (NumberOfButtons + (NumberOfButtons + 1) * MarginRatio));
            margin = (int)(buttonSize * MarginRatio);
            int buttonY = GlobalVariables.ScreenHeight - buttonSize - margin;

            TitlePostion = new Vector2(GlobalVariables.ScreenWidth / 2 - Title.Width / 2, 0.1f * GlobalVariables.ScreenHeight);

            SpriteFont font = GlobalVariables.Game.Content.Load<SpriteFont>("BigButtonFont");
            Color buttonColor = new Color(0.55f, 0.34f, 0, 0.2f);

            Tile playTile = new Tile()
            {
                //Text = "Play",
                //TextColor = Color.Red,
                Position = new Vector2(margin, buttonY),
                Size = new Vector2(buttonSize),
                Screen = this,
                Font = font,
                ButtonColor = buttonColor,
                Background = GlobalVariables.Game.Content.Load<Texture2D>("play")
            };
            playTile.Selected += PlayGameMenuEntrySelected;
            Tiles.Add(playTile);

            Tile recordsTile = new Tile()
            {
                //Text = "Records",
                //TextColor = Color.Red,
                Position = playTile.Position + new Vector2(margin + buttonSize, 0),
                Size = new Vector2(buttonSize),
                Screen = this,
                Font = font,
                ButtonColor = buttonColor,
                Background = GlobalVariables.Game.Content.Load<Texture2D>("records")
            };
            Tiles.Add(recordsTile);

            Tile helpTile = new Tile()
            {
                //Text = "Help",
                //TextColor = Color.Red,
                Position = recordsTile.Position + new Vector2(margin + buttonSize, 0),
                Size = new Vector2(buttonSize),
                Screen = this,
                Font = font,
                ButtonColor = buttonColor,
                Background = GlobalVariables.Game.Content.Load<Texture2D>("help")
            };
            helpTile.Selected += OptionsMenuEntrySelected;
            Tiles.Add(helpTile);

            Tile exitTile = new Tile()
            {
                Position = helpTile.Position + new Vector2(margin + buttonSize, 0),
                Size = new Vector2(buttonSize),
                Screen = this,
                Font = font,
                ButtonColor = buttonColor,
                Background = GlobalVariables.Game.Content.Load<Texture2D>("exit")
            };
            exitTile.Selected += OnCancel;
            Tiles.Add(exitTile);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //LoadingScreen.Load(ScreenManager, false, e.PlayerIndex,
            //                   new GameplayScreen());
            GameScreen gameplay = new GameplayScreen();
            ScreenManager.AddScreen(gameplay, e.PlayerIndex);
        }


        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HelpScreen(), e.PlayerIndex);
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
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
