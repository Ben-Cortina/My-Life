
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
    /// Screen to provide functional deposits and withdrawls
    /// </summary>
    class BankScreen : GameScreen
    {

        #region Fields

        ContentManager content;
        SpriteFont numberFont;

        Color buttonColor = Color.White;

        List<Tile> tiles = new List<Tile>();

        Texture2D background, buttonTexture, textBoxTexture, backArrowTexture;

        const float marginRatio = 0.05f;

        int transfer = 0;

        Vector2 margin = new Vector2(GlobalVariables.ScreenWidth * marginRatio, GlobalVariables.ScreenHeight * marginRatio);

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public BankScreen()
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
            background = content.Load<Texture2D>("Bank_back");
            textBoxTexture = content.Load<Texture2D>("textbox");
            buttonTexture = content.Load<Texture2D>("button");
            numberFont = content.Load<SpriteFont>("DashboardFont");
            backArrowTexture = content.Load<Texture2D>("back_64");

            #region Create Tiles

            Tile one = new Tile()
            {
                Text = "+ $1",
                TextColor = Color.Black,
                Position = new Vector2(GlobalVariables.ScreenWidth * .08f, GlobalVariables.ScreenHeight * .75f),
                Background = buttonTexture,
                Image = new Rectangle(0,0,159,59),
                AltImage = new Rectangle(180,0,159,59),
                TileIteraction = Tile.Interaction.Image,
                Screen = this,
                Font = numberFont,
                FontScale = .18f,
                Size = new Vector2(159, 59),
                TileAlignment = Tile.Alignment.Fill,
            };
            one.Selected += delegate(object o, PlayerIndexEventArgs arg)
            {
                if (transfer < 1000000)
                    transfer += 1;
            };
            tiles.Add(one);

            Tile ten = new Tile()
            {
                Text = "+ $10",
                TextColor = Color.Black,
                Position = new Vector2(one.Position.X + one.Size.X + 5, one.Position.Y),
                Background = buttonTexture,
                Image = new Rectangle(0, 0, 159, 59),
                AltImage = new Rectangle(180, 0, 159, 59),
                TileIteraction = Tile.Interaction.Image,
                Screen = this,
                Font = numberFont,
                FontScale = .18f,
                Size = new Vector2(159, 59),
                TileAlignment = Tile.Alignment.Fill,
            };
            ten.Selected += delegate(object o, PlayerIndexEventArgs arg)
            {
                if (transfer < 999991)
                transfer += 10;
            };
            tiles.Add(ten);
            
            Tile hundred = new Tile()
            {
                Text = "+ $100",
                TextColor = Color.Black,
                Position = new Vector2(ten.Position.X + ten.Size.X + 5, ten.Position.Y),
                Background = buttonTexture,
                Image = new Rectangle(0, 0, 159, 59),
                AltImage = new Rectangle(180, 0, 159, 59),
                TileIteraction = Tile.Interaction.Image,
                Screen = this,
                Font = numberFont,
                FontScale = .18f,
                Size = new Vector2(159, 59),
                TileAlignment = Tile.Alignment.Fill,
            };
            hundred.Selected += delegate(object o, PlayerIndexEventArgs arg)
            {
                if (transfer < 999901)
                    transfer += 100;
            };
            tiles.Add(hundred);


            Tile oneM = new Tile()
            {
                Text = "- $1",
                TextColor = Color.Black,
                Position = new Vector2(one.Position.X,one.Position.Y + one.Size.Y + 5),
                Background = buttonTexture,
                Image = new Rectangle(0, 0, 159, 59),
                AltImage = new Rectangle(180, 0, 159, 59),
                TileIteraction = Tile.Interaction.Image,
                Screen = this,
                Font = numberFont,
                FontScale = .18f,
                Size = new Vector2(159, 59),
                TileAlignment = Tile.Alignment.Fill,
            };
            oneM.Selected += delegate(object o, PlayerIndexEventArgs arg)
            {
                if (transfer > 0)
                    transfer -= 1;
            };
            tiles.Add(oneM);

            Tile tenM = new Tile()
            {
                Text = "- $10",
                TextColor = Color.Black,
                Position = new Vector2(oneM.Position.X + oneM.Size.X + 5, oneM.Position.Y),
                Background = buttonTexture,
                Image = new Rectangle(0, 0, 159, 59),
                AltImage = new Rectangle(180, 0, 159, 59),
                TileIteraction = Tile.Interaction.Image,
                Screen = this,
                Font = numberFont,
                FontScale = .18f,
                Size = new Vector2(159, 59),
                TileAlignment = Tile.Alignment.Fill,
            };
            tenM.Selected += delegate(object o, PlayerIndexEventArgs arg)
            {
                if (transfer > 9)
                    transfer -= 10;
            };
            tiles.Add(tenM);

            Tile hundredM = new Tile()
            {
                Text = "- $100",
                TextColor = Color.Black,
                Position = new Vector2(tenM.Position.X + tenM.Size.X + 5, tenM.Position.Y),
                Background = buttonTexture,
                Image = new Rectangle(0, 0, 159, 59),
                AltImage = new Rectangle(180, 0, 159, 59),
                TileIteraction = Tile.Interaction.Image,
                Screen = this,
                Font = numberFont,
                FontScale = .18f,
                Size = new Vector2(159, 59),
                TileAlignment = Tile.Alignment.Fill,
            };
            hundredM.Selected += delegate(object o, PlayerIndexEventArgs arg)
            {
                if (transfer > 99)
                    transfer -= 100;
            };
            tiles.Add(hundredM);

            Tile depositTile = new Tile()
            {
                Text = "Deposit",
                TextColor = Color.Black,
                Position = new Vector2(GlobalVariables.ScreenWidth*.625f, GlobalVariables.ScreenHeight * .68f),
                Background = buttonTexture,
                Image = new Rectangle(0, 0, 159, 59),
                AltImage = new Rectangle(180, 0, 159, 59),
                TileIteraction = Tile.Interaction.Image,
                Screen = this,
                Font = numberFont,
                FontScale = .20f,
                Size = new Vector2(216, 80),
                TileAlignment = Tile.Alignment.Fill,
            };
            depositTile.Selected += Deposit;
            tiles.Add(depositTile);

            Tile withdrawTile = new Tile()
            {
                Text = "Withdrawal",
                TextColor = Color.Black,
                Position = new Vector2(depositTile.Position.X, depositTile.Position.Y + depositTile.Size.Y +5),
                Background = buttonTexture,
                Image = new Rectangle(0, 0, 159, 59),
                AltImage = new Rectangle(180, 0, 159, 59),
                TileIteraction = Tile.Interaction.Image,
                Screen = this,
                Font = numberFont,
                FontScale = .20f,
                Size = new Vector2(216, 80),
                TileAlignment = Tile.Alignment.Fill,
            };
            withdrawTile.Selected += Withdraw;
            tiles.Add(withdrawTile);

            Tile back = new Tile()
            {
                Position = new Vector2(margin.X, margin.Y),
                Size = new Vector2(backArrowTexture.Width, backArrowTexture.Height),
                TileIteraction = Tile.Interaction.Image,
                Scale = .8f,
                Screen = this,
                Background = backArrowTexture,
            };
            back.Selected += delegate(object o, PlayerIndexEventArgs arg)
            {
                ExitScreen();
            };
            tiles.Add(back);

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
            float bigFont = .30f;
            float smallFont = .22f;
            float tbScale = 1.5f;
            string balance = "$" + GlobalVariables.Finance.BankBalance.ToString();
            string cash = "$" + GlobalVariables.Finance.Cash.ToString();
            string trans = "$" + transfer.ToString();
            Vector2 bTextSize = numberFont.MeasureString(balance) * bigFont;
            Vector2 cTextSize = numberFont.MeasureString(cash) * bigFont;
            Vector2 tTextSize = numberFont.MeasureString(trans) * smallFont;
            Vector2 bPosition = new Vector2(GlobalVariables.ScreenWidth * .28f - bTextSize.X/2,
                                            GlobalVariables.ScreenHeight * .44f - bTextSize.Y);
            Vector2 cPosition = new Vector2(GlobalVariables.ScreenWidth * .71f - cTextSize.X/2,
                                            GlobalVariables.ScreenHeight * .44f - cTextSize.Y);

            Vector2 tbPosition = new Vector2(GlobalVariables.ScreenWidth * .28f - textBoxTexture.Width *tbScale / 2,
                                            GlobalVariables.ScreenHeight * .66f);
            Vector2 tPosition = tbPosition + new Vector2(3, 3);
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, GlobalVariables.ScreenWidth,
                GlobalVariables.ScreenHeight), Color.White);

            DrawStringScale(spriteBatch, balance, bPosition, Color.Black, bigFont);
            DrawStringScale(spriteBatch, cash, cPosition, Color.Black, bigFont);

            spriteBatch.Draw(textBoxTexture, tbPosition,textBoxTexture.Bounds, Color.White,0,Vector2.Zero,tbScale,SpriteEffects.None,0.1f);
            DrawStringScale(spriteBatch, trans, tPosition, Color.Black, smallFont);

            foreach (Tile tile in tiles)
                tile.Draw();

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.\
            if (TransitionPosition > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f,0);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        void DrawStringScale(SpriteBatch sb, String text, Vector2 position, Color color, float scale)
        {
            sb.DrawString(numberFont, text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
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

        void Deposit(object sender, PlayerIndexEventArgs e)
        {
            if (GlobalVariables.Finance.Cash >=transfer && transfer > 0)
            {
                GlobalVariables.Finance.BankBalance += transfer;
                GlobalVariables.Finance.Cash -= transfer;

                Transaction temp = new Transaction("Bank Deposit",transfer);
                GlobalVariables.Finance.Transactions.Add(temp);

                transfer = 0;

            }
            
        }

        void Withdraw(object sender, PlayerIndexEventArgs e)
        {
            if (GlobalVariables.Finance.BankBalance >= transfer && transfer > 0)
            {
                GlobalVariables.Finance.Cash += transfer;
                GlobalVariables.Finance.BankBalance -= transfer;

                Transaction temp = new Transaction("Bank Withdrawal", transfer);
                GlobalVariables.Finance.Transactions.Add(temp);

                transfer = 0;
            }
            
        }

        #endregion
    }
}

