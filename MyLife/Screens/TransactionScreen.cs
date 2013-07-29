
#region Using Statements
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace MyLife
{
    /// <summary>
    /// Screen to display all transactions
    /// </summary>
    class TransactionScreen : GameScreen
    {

        #region Fields

        ContentManager content;
        SpriteFont font;

        Color buttonColor = Color.White;

        List<Tile> tiles = new List<Tile>();

        Tile backButton, nextButton;

        Texture2D background, nextArrowTexture, backArrowTexture;

        const float marginRatio = 0.05f;

        int page = 0;

        Vector2 margin = new Vector2(GlobalVariables.ScreenWidth * marginRatio, GlobalVariables.ScreenHeight * marginRatio);

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public TransactionScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            background = content.Load<Texture2D>("Transaction_Screen");
            nextArrowTexture = content.Load<Texture2D>("Continue_64");
            font = content.Load<SpriteFont>("DashboardFont");
            backArrowTexture = content.Load<Texture2D>("back_64");

            #region Create Tiles

            backButton = new Tile()
            {
                Position = new Vector2((GlobalVariables.ScreenWidth / 2) - 96, GlobalVariables.ScreenHeight - (margin.Y + 48)),
                Size = new Vector2(backArrowTexture.Width, backArrowTexture.Height),
                Screen = this,
                Background = backArrowTexture,
            };
            backButton.Selected += Back;
            tiles.Add(backButton);

            nextButton = new Tile()
            {
                Position = new Vector2((GlobalVariables.ScreenWidth / 2) + 32, GlobalVariables.ScreenHeight - (margin.Y + 48)),
                Size = new Vector2(nextArrowTexture.Width, nextArrowTexture.Height),
                Screen = this,
                Background = nextArrowTexture,
            };
            nextButton.Selected += Next;
            tiles.Add(nextButton);

            #endregion

        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (IsActive)
            {
                // TODO: this game isn't very fun! You could probably improve
                // it by inserting something more interesting in this space :-)

                foreach (Tile tile in tiles)
                    tile.Update(gameTime);

                //calendar.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            int numTrans = GlobalVariables.Finance.Transactions.Count;
            string bank, cash, name, credit, date, amount;
            Vector2 position = new Vector2(GlobalVariables.ScreenWidth*.0625f, GlobalVariables.ScreenHeight*0.25f-34);
            float fontScale = .18f;
            int totalPages = (int)((GlobalVariables.Finance.Transactions.Count) / 13);

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, GlobalVariables.ScreenWidth,
                GlobalVariables.ScreenHeight), Color.White);

            for (int i = page * 13; i < numTrans && i < page * 13 + 13; i++)
            {
                name = GlobalVariables.Finance.Transactions[i].Name;
                date = GlobalVariables.Finance.Transactions[i].Date.ToShortDateString();
                amount = "$" + GlobalVariables.Finance.Transactions[i].Amount.ToString();
                bank = "$" + GlobalVariables.Finance.Transactions[i].Bank.ToString();
                credit = "$" + GlobalVariables.Finance.Transactions[i].Credit.ToString();
                cash = "$" + GlobalVariables.Finance.Transactions[i].Cash.ToString();
                position += new Vector2(0, 34);
                DrawStringScale(spriteBatch, name, position, Color.Black, fontScale);
                DrawStringScale(spriteBatch, date, position + new Vector2(GlobalVariables.ScreenWidth * 0.25f,0), Color.Black, fontScale);
                DrawStringScale(spriteBatch, amount, position + new Vector2(GlobalVariables.ScreenWidth * 0.375f, 0), Color.Black, fontScale);
                DrawStringScale(spriteBatch, bank, position + new Vector2(GlobalVariables.ScreenWidth * 0.5f, 0), Color.Black, fontScale);
                DrawStringScale(spriteBatch, credit, position + new Vector2(GlobalVariables.ScreenWidth * 0.625f, 0), Color.Black, fontScale);
                DrawStringScale(spriteBatch, cash, position + new Vector2(GlobalVariables.ScreenWidth * 0.75f, 0), Color.Black, fontScale);
            }

            backButton.Draw();
            if (page != totalPages)
                nextButton.Draw();

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.\
            if (TransitionPosition > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, 0);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        void DrawStringScale(SpriteBatch sb, String text, Vector2 position, Color color, float scale)
        {
            sb.DrawString(font, text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (InputDeviceState.LeftButton)
            {
                MouseState mouseState = Mouse.GetState();
                foreach (Tile tile in tiles)
                    tile.CheckPress(mouseState);
            }



            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!

            PlayerIndex pIndex;

            if (input.IsMenuCancel(ControllingPlayer, out pIndex))
                ExitScreen();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Triggers when the user presses the Back button
        /// </summary>
        void Back(object sender, PlayerIndexEventArgs e)
        {
            if (page == 0)
                ExitScreen();
            else
                page--;
        }

        /// <summary>
        /// Triggers when the user presses the Next button
        /// </summary>
        void Next(object sender, PlayerIndexEventArgs e)
        {
            int totalPages = (int)((GlobalVariables.Finance.Transactions.Count) / 13);
            if (page < totalPages)
                page++;

        }

        #endregion
    }
}

