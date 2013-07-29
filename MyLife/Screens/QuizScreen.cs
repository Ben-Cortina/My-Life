
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
    /// Screen to quiz the user
    /// </summary>
    class QuizScreen : GameScreen
    {

        #region Fields

        ContentManager content;
        SpriteFont font;

        Color buttonColor = Color.White;

        List<Tile> tiles = new List<Tile>();

        Texture2D background, buttonTexture, backArrowTexture;

        const float marginRatio = 0.05f;

        Vector2 margin = new Vector2(GlobalVariables.ScreenWidth * marginRatio, GlobalVariables.ScreenHeight * marginRatio);

        string answerA, answerB, answerC, answerD, question;
        int correctAnswer;

        public event EventHandler<PlayerIndexEventArgs> Complete;

        bool correct=false, wrong=false;

        public bool Correct
        {
            get { return correct; }
            set { correct = value; }
        }

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public QuizScreen()
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
            background = content.Load<Texture2D>("Quiz_Screen");
            buttonTexture = content.Load<Texture2D>("button");
            font = content.Load<SpriteFont>("DashboardFont");
            backArrowTexture = content.Load<Texture2D>("back_64");

            GenerateQuestion();
            float buttonTextSize = .18f;

            #region Create Tiles

            Vector2 buttonSize;

            buttonSize = new Vector2(font.MeasureString(answerA).X * buttonTextSize + 16, 40);
            Tile a = new Tile()
            {
                Text = answerA,
                TextColor = Color.Black,
                Size = buttonSize,
                Position = new Vector2(GlobalVariables.ScreenWidth / 2 - buttonSize.X / 2, GlobalVariables.ScreenHeight / 2 - buttonSize.Y / 2),
                Background = buttonTexture,
                Image = new Rectangle(0, 0, 159, 59),
                AltImage = new Rectangle(180, 0, 159, 59),
                TileIteraction = Tile.Interaction.Image,
                Screen = this,
                Font = font,
                FontScale = buttonTextSize,
                TileAlignment = Tile.Alignment.Fill,
            };
            a.Selected += AnsweredA;
            tiles.Add(a);

            buttonSize= new Vector2(font.MeasureString(answerB).X * buttonTextSize + 16, 40);
            Tile b = new Tile()
            {
                Text = answerB,
                TextColor = Color.Black,
                Size = buttonSize,
                Position = new Vector2(GlobalVariables.ScreenWidth / 2 - buttonSize.X / 2, a.Position.Y + buttonSize.Y +8),
                Background = buttonTexture,
                Image = new Rectangle(0, 0, 159, 59),
                AltImage = new Rectangle(180, 0, 159, 59),
                TileIteraction = Tile.Interaction.Image,
                Screen = this,
                Font = font,
                FontScale = buttonTextSize,
                TileAlignment = Tile.Alignment.Fill,
            };
            b.Selected += AnsweredB;
            tiles.Add(b);

            buttonSize= new Vector2(font.MeasureString(answerC).X * buttonTextSize + 16, 40);
            Tile c = new Tile()
            {
                Text = answerC,
                TextColor = Color.Black,
                Size = buttonSize,
                Position = new Vector2(GlobalVariables.ScreenWidth / 2 - buttonSize.X / 2, b.Position.Y + buttonSize.Y +8),
                Background = buttonTexture,
                Image = new Rectangle(0, 0, 159, 59),
                AltImage = new Rectangle(180, 0, 159, 59),
                TileIteraction = Tile.Interaction.Image,
                Screen = this,
                Font = font,
                FontScale = buttonTextSize,
                TileAlignment = Tile.Alignment.Fill,
            };
            c.Selected += AnsweredC;
            tiles.Add(c);

            buttonSize= new Vector2(font.MeasureString(answerD).X * buttonTextSize + 16, 40);
            Tile d = new Tile()
            {
                Text = answerD,
                TextColor = Color.Black,
                Size = buttonSize,
                Position = new Vector2(GlobalVariables.ScreenWidth / 2 - buttonSize.X / 2, c.Position.Y + buttonSize.Y +8),
                Background = buttonTexture,
                Image = new Rectangle(0, 0, 159, 59),
                AltImage = new Rectangle(180, 0, 159, 59),
                TileIteraction = Tile.Interaction.Image,
                Screen = this,
                Font = font,
                FontScale = buttonTextSize,
                TileAlignment = Tile.Alignment.Fill,
            };
            d.Selected += AnsweredD;
            tiles.Add(d);

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
            float bigFont = .25f;
            float hugeFont = .40f;
            string right = "CORRECT!";
            string notRight = "Wrong.";
            Vector2 questionSize = font.MeasureString(question) * bigFont;
            Vector2 rSize = font.MeasureString(right) * hugeFont;
            Vector2 nRSize = font.MeasureString(notRight) * hugeFont;
            Vector2 qPosition = new Vector2(GlobalVariables.ScreenWidth * .5f - questionSize.X / 2,
                                            GlobalVariables.ScreenHeight * .4f - questionSize.Y);
            Vector2 rPosition = new Vector2(GlobalVariables.ScreenWidth * .5f - rSize.X / 2,
                                            GlobalVariables.ScreenHeight - margin.Y - rSize.Y);
            Vector2 nRPosition = new Vector2(GlobalVariables.ScreenWidth * .5f - nRSize.X / 2,
                                            GlobalVariables.ScreenHeight - margin.Y - nRSize.Y);
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, GlobalVariables.ScreenWidth,
                GlobalVariables.ScreenHeight), Color.White);

            DrawStringScale(spriteBatch, question, qPosition, Color.Black, bigFont);

            if (wrong)
                DrawStringScale(spriteBatch, notRight, nRPosition, Color.Red, hugeFont);
            if (correct)
                DrawStringScale(spriteBatch, right, rPosition, Color.Green, hugeFont);
            foreach (Tile tile in tiles)
                tile.Draw();

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
                if (!correct)
                {
                    foreach (Tile tile in tiles)
                        tile.CheckPress(mouseState);
                }
                else
                    tiles[tiles.Count-1].CheckPress(mouseState);
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

        void AnsweredA(object sender, PlayerIndexEventArgs e)
        {
            wrong = true;
            if (correctAnswer == 1)
                OnComplete(e);
        }
        void AnsweredB(object sender, PlayerIndexEventArgs e)
        {
            wrong = true;
            if (correctAnswer == 2)
                OnComplete(e);

        }
        void AnsweredC(object sender, PlayerIndexEventArgs e)
        {
            wrong = true;
            if (correctAnswer == 3)
                OnComplete(e);

        }
        void AnsweredD(object sender, PlayerIndexEventArgs e)
        {
            wrong = true;
            if (correctAnswer == 4)
                OnComplete(e);
        }

        internal virtual void OnComplete(PlayerIndexEventArgs e)
        {
            correct = true;
            wrong = false;
            if (Complete != null)
                Complete(this, e);
        }

        void GenerateQuestion()
        {
            Random n = new Random();
            int q = n.Next(0, 10);
            string correct, wrong1, wrong2, wrong3;
            int month = n.Next(0, 1300);
            int limit = 1000;
            int balance = n.Next(0, 10000);
            int fee = 0;

            switch (q)
            {
                default:
                    correct = "";
                    wrong1 = "";
                    wrong2 = "";
                    wrong3 = "";
                    break;
                //Credit Minimum
                case 0:
                    question = "If this month's balance is $" + month.ToString() + ", your monthly limit\n" +
                               "is $1000, and your balance (excluding this month) is $" + balance.ToString() +
                               ".\nWhat is the minimum amount you have to pay? \n(overcharge fee is $35) (minimum payment is 1%)";
                    if (month > limit)
                        fee += 35;

                    correct = "$" + ((int)Math.Ceiling((balance + month) * 0.01f + fee)).ToString();
                    wrong1 = "$" + ((int)Math.Max(Math.Ceiling((balance + month) * 0.01f - 35), 0)).ToString();
                    wrong2 = "$" + ((int)Math.Ceiling((balance + month) * 0.02f + fee)).ToString();
                    wrong3 = "$" + ((int)Math.Ceiling((balance) * 0.01f + fee)).ToString();
                    break;

                //Credit Balance
                case 1:
                    question = "If this month's balance is $" + month.ToString() + ", your monthly limit\n" +
                               "is $1000, and your balance (excluding this month) is $" + balance.ToString() +
                               ".\nWhat will your balance be at the beginning of next month? \n(overcharge fee is $35) (intrest rate is 18%)";
                    correct = "$" + ((int)(balance * 0.18f + month)).ToString();
                    wrong1 = "$" + ((int)(balance * 0.10f + month)).ToString();
                    wrong2 = "$" + ((int)(balance * 0.18f)).ToString();
                    wrong3 = "$" + ((int)(balance * 0.10f)).ToString();
                    break;
                //Missed payments
                case 2:
                    question = "In order to improve your credit score you should...\n";
                    correct = "pay off more than just this months balance.";
                    wrong1 = "only pay the minimum payment.";
                    wrong2 = "use your credit card more often.";
                    wrong3 = "charge more than the limit each month.";
                    break;
                // Bills
                case 3:
                    question = "Bills should be...\n";
                    correct = "paid immediately.";
                    wrong1 = "ignored.";
                    wrong2 = "left in your mailbox.";
                    wrong3 = "thrown away.";
                    break;
                // Credit card balance
                case 4:
                    question = "A credit card balance is...\n";
                    correct = "the amount of money you have charged.";
                    wrong1 = "how much money you have left to spend.";
                    wrong2 = "unimportant.";
                    wrong3 = "for measuring weight.";
                    break;
                // Keeping money in a bank...
                case 5:
                    question = "Money in a bank account is not...\n";
                    correct = "physical money.";
                    wrong1 = "earning interest.";
                    wrong2 = "safe.";
                    wrong3 = "accessible.";
                    break;

                // Carrying Cash...
                case 6:
                    question = "You shouldn't...\n";
                    correct = "keep most of your money on your person.";
                    wrong1 = "keep most of you money in the bank.";
                    wrong2 = "pay your bills.";
                    wrong3 = "use anything but your credit card.";
                    break;
                // Planning...
                case 7:
                    question = "Planning ahead will...\n";
                    correct = "keep you out of financial trouble.";
                    wrong1 = "make you go bankrupt.";
                    wrong2 = "raise your debt.";
                    wrong3 = "hurt your credit score.";
                    break;
                // spending money
                case 8:
                    question = "You shouldn't...\n";
                    correct = "blindly spend your money.";
                    wrong1 = "monitor your credit score.";
                    wrong2 = "watch how much you are spending.";
                    wrong3 = "put money in the bank.";
                    break;
                // spending money
                case 9:
                    question = "The majority of you pay check should be...\n";
                    correct = "stored in savings.";
                    wrong1 = "spent on luxuries.";
                    wrong2 = "kept as cash.";
                    wrong3 = "thrown away.";
                    break;
            }
            List<string> answers = new List<string>() {correct,wrong1,wrong2,wrong3};
            answers = AssignOne(answers, correct);
            answers = AssignOne(answers, correct);
            answers = AssignOne(answers, correct);
            answers = AssignOne(answers, correct);
        }
        List<string> AssignOne(List<string> s, string cor)
        {
            Random n = new Random();
            int ran = n.Next(0,s.Count-1);
            if (s[ran] == cor)
                correctAnswer = s.Count;
            List<string> ret = new List<string>();
            switch (s.Count)
            {
                default:
                    break;
                case 1:
                    answerA = s[ran];
                    s.RemoveAt(ran);
                    break;
                case 2:
                    answerB = s[ran];
                    s.RemoveAt(ran);
                    break;
                case 3:
                    answerC = s[ran];
                    s.RemoveAt(ran);
                    break;
                case 4:
                    answerD = s[ran];
                    s.RemoveAt(ran);
                    break;
            }
            return s;

        }
        #endregion
    }
}

