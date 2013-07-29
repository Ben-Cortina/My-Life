#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

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
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class HelpScreen : GameScreen
    {

        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        //Calendar calendar = new Calendar(GlobalVariables.Game);
        //Dashboard dashboard = new Dashboard(GlobalVariables.Game);

        //Color buttonColor = new Color(0.55f, 0.55f, 0, 1f);
        Color buttonColor = Color.White;

        List<Tile> tiles = new List<Tile>();

        Tile continueButton, backButton, undoButton, calendarT, bankT, book, test, note1, note2, note3;

        Texture2D background, dashboardTexture, calendarTexture, undoTexture, backTexture,
            continueTexture, smallCalendarTexture, summaryTexture, bookTexture, testTexture,
            todayTexture, noteTexture, textBoxTexture;

        const float marginRatio = 0.05f;

        Vector2 margin = new Vector2(GlobalVariables.ScreenWidth * marginRatio, GlobalVariables.ScreenHeight * marginRatio);

        float pauseAlpha;

        List<CalendarEvent> todaysEvents = new List<CalendarEvent>();
        SpriteFont dashboardFont;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public HelpScreen()
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

            SpriteFont font = content.Load<SpriteFont>("EventFont");
            SpriteFont midFont = content.Load<SpriteFont>("MediumButtonFont");
            SpriteFont calendarFont = content.Load<SpriteFont>("CalendarFont");
            dashboardFont = content.Load<SpriteFont>("gamefont");
            gameFont = content.Load<SpriteFont>("gamefont");
            background = content.Load<Texture2D>("finance");
            dashboardTexture = content.Load<Texture2D>("gradient");
            textBoxTexture = content.Load<Texture2D>("textbox");
            calendarTexture = content.Load<Texture2D>("calendar");
            smallCalendarTexture = content.Load<Texture2D>("vcalendar_new");
            summaryTexture = content.Load<Texture2D>("Money Calculator");
            bookTexture = content.Load<Texture2D>("book_icon");
            testTexture = content.Load<Texture2D>("Exams_icon_100");
            undoTexture = content.Load<Texture2D>("undo-icon-64");
            backTexture = content.Load<Texture2D>("Back_64");
            continueTexture = content.Load<Texture2D>("Continue_64");
            noteTexture = content.Load<Texture2D>("note-small");
            todayTexture = content.Load<Texture2D>("Calendar_large");

            #region Generate Tiles

            Tile today = new Tile()
            {
                Position = new Vector2(margin.X * 2, margin.Y),
                Background = todayTexture,
                Screen = this,
                Size = new Vector2(128, 128),
                TileAlignment = Tile.Alignment.Fill,
            };
            today.Selected += ShowDash;
            tiles.Add(today);

            note1 = new Tile()
            {
                Text = "  Credit Card \n  Payment Due",
                Position = new Vector2(margin.X * 2, margin.Y * 8),
                Background = noteTexture,
                Size = new Vector2(noteTexture.Width, noteTexture.Height),
                Font = font,
                Screen = this,
                TextColor = Color.Red,
            };
            note1.Selected += LoadPaymentScreen;
            tiles.Add(note1);

            note2 = new Tile()
            {
                Text = "Car Repair",
                Position = note1.Position + new Vector2(note1.Size.X + margin.X * 2, 0),
                Background = noteTexture,
                Size = new Vector2(noteTexture.Width, noteTexture.Height),
                Font = font,
                Screen = this,
                TextColor = Color.Red,
            };
            note2.Selected += LoadPaymentScreen;
            tiles.Add(note2);

            note3 = new Tile()
            {
                Text = "Random Expense",
                Position = note2.Position + new Vector2(note2.Size.X + margin.X * 2, 0),
                Background = noteTexture,
                Size = new Vector2(noteTexture.Width, noteTexture.Height),
                Font = font,
                Screen = this,
                TextColor = Color.Red,
            };
            note3.Selected += LoadPaymentScreen;
            tiles.Add(note3);


            bankT = new Tile()
            {
                Size = new Vector2(margin.Y * 2.5f, margin.Y * 2f),
                Position = new Vector2(GlobalVariables.ScreenWidth - margin.Y * 4, margin.Y * 2),
                Screen = this,
                TileIteraction = Tile.Interaction.Image,
                Scale = .8f,
                Background = summaryTexture,
                TileAlignment = Tile.Alignment.Center,
            };
            bankT.Selected += LoadBank;
            tiles.Add(bankT);

            calendarT = new Tile()
            {
                Size = new Vector2(margin.Y * 2.5f, margin.Y * 2.5f),
                Position = bankT.Position + new Vector2(0, bankT.Size.Y + margin.Y * 2),
                Background = smallCalendarTexture,
                Screen = this,
                TileIteraction = Tile.Interaction.Image,
                Scale = .8f,
                TileAlignment = Tile.Alignment.Center,
            };
            calendarT.Selected += ShowDash;
            tiles.Add(calendarT);

            book = new Tile()
            {
                Size = new Vector2(margin.Y * 2.7f, margin.Y * 2.5f),
                Position = calendarT.Position + new Vector2(0, calendarT.Size.Y + margin.Y * 2),
                Background = bookTexture,
                Screen = this,
                TileIteraction = Tile.Interaction.Image,
                Scale = .8f,
                TileAlignment = Tile.Alignment.Fill,
            };
            book.Selected += LoadTransactionScreen;
            tiles.Add(book);

            test = new Tile()
            {
                Size = new Vector2(margin.Y * 2.5f, margin.Y * 2.5f),
                Position = book.Position + new Vector2(0, book.Size.Y + margin.Y * 2),
                Background = testTexture,
                Screen = this,
                TileAlignment = Tile.Alignment.Fill,
                TileIteraction = Tile.Interaction.Image,
                Scale = .8f,
            };
            test.Selected += LoadQuiz;
            tiles.Add(test);

            undoButton = new Tile()
            {
                Size = new Vector2(undoTexture.Width, undoTexture.Height),
                Position = new Vector2(GlobalVariables.ScreenWidth / 2 - undoTexture.Width / 2 - margin.Y * 2, margin.Y * 17),
                Screen = this,
                Font = midFont,
                ButtonColor = buttonColor,
                Background = undoTexture,
                TileIteraction = Tile.Interaction.Image,
                Scale = .8f,
                TileAlignment = Tile.Alignment.Fill,

            };
            undoButton.Selected += Undo;
            tiles.Add(undoButton);

            continueButton = new Tile()
            {
                Position = undoButton.Position + new Vector2(undoButton.Size.X + margin.Y * 2, 0),
                Screen = this,
                Background = continueTexture,
                Font = midFont,
                ButtonColor = buttonColor,
                TileIteraction = Tile.Interaction.Image,
                Scale = .8f,
                Size = new Vector2(continueTexture.Width, continueTexture.Height),
            };
            continueButton.Selected += NewDay;
            tiles.Add(continueButton);

            backButton = new Tile()
            {
                Size = new Vector2(backTexture.Width, backTexture.Height),
                Position = undoButton.Position - new Vector2(backTexture.Width + margin.Y * 2, 0),
                Screen = this,
                TileIteraction = Tile.Interaction.Image,
                Scale = .8f,
                Background = backTexture,
            };
            backButton.Selected += OnCancel;
            tiles.Add(backButton);

            #endregion







            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            //Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                // TODO: this game isn't very fun! You could probably improve
                // it by inserting something more interesting in this space :-)

                foreach (Tile tile in tiles)
                    tile.Update(gameTime);

            }
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
                OnCancel(pIndex);


            if (input.IsDashboard(ControllingPlayer, out pIndex))
                ShowDash(pIndex);
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, GlobalVariables.ScreenWidth,
                GlobalVariables.ScreenHeight), new Color(0.7f, 0.7f, 0.7f, 1));

            spriteBatch.DrawString(gameFont, "What Is Happening Today?", new Vector2(margin.X * 5, margin.Y * 2), Color.White,0,Vector2.Zero,.5f,SpriteEffects.None,0f);

            foreach (Tile tile in tiles)
                tile.Draw();

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to go back to the menu?\nNOTE: All progress will be lost.";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ExitScreen();
        }

        /// <summary>
        /// Shows the Dashboard
        /// </summary>
        void ShowDash(object sender, PlayerIndexEventArgs e)
        {
            HelpFadeScreen hfs = new HelpFadeScreen()
            {
                Title = "This icon will bring up the dashboard, alternatively you can access the dashboard by F1.\n" +
                        "The Dashboard will show important information such as your bank account balance, cash\n" +
                        "and credit balance.",
                Selection = calendarT,
            };
            ScreenManager.AddScreen(hfs, e.PlayerIndex);
        }

        /// <summary>
        /// Shows the Dashboard
        /// </summary>
        void ShowDash(PlayerIndex playerIndex)
        {
            HelpFadeScreen hfs = new HelpFadeScreen()
            {
                Title = "This icon will bring up the dashboard, alternatively you can access the dashboard by F1.\n" +
                        "The Dashboard will show important information such as your bank account balance, cash\n" +
                        "and credit balance.",
                Selection = calendarT,
            };
            ScreenManager.AddScreen(hfs, playerIndex);
        }

        void LoadBank(object sender, PlayerIndexEventArgs e)
        {
            HelpFadeScreen hfs = new HelpFadeScreen()
            {
                Title = "This is the bank. This icon on the right will bring up the bank.\n" +
                        "The arrow in the top left will close the bank. The bank is where\n" +
                        "you can withdraw or deposit money.",
                Selection = bankT,
            };
            BankScreen bank = new BankScreen();
            hfs.Exit += delegate(object s, PlayerIndexEventArgs x)
            {
                bank.ExitScreen();
            };
            ScreenManager.AddScreen(bank, e.PlayerIndex);
            ScreenManager.AddScreen(hfs, e.PlayerIndex);
        }

        void LoadTransactionScreen(object sender, PlayerIndexEventArgs e)
        {
            HelpFadeScreen hfs = new HelpFadeScreen()
            {
                Title = "This is your record book.This icon on the right will bring up this book.\n" +
                        "The arrow on the bottom will close the book.This is where you can\n" +
                        "view your transaction history.",
                Selection = book,
            };
            TransactionScreen transactionScreen = new TransactionScreen();
            hfs.Exit += delegate(object s, PlayerIndexEventArgs x)
            {
                transactionScreen.ExitScreen();
            };
            ScreenManager.AddScreen(transactionScreen, e.PlayerIndex);
            ScreenManager.AddScreen(hfs, e.PlayerIndex);
        }

        void LoadPaymentScreen(object sender, PlayerIndexEventArgs e)
        {
            HelpFadeScreen hfs = new HelpFadeScreen()
            {
                Title = "These sticky notes represents the events that occured today. Every day you will need\n" +
                        "to pay each event which requires payment before you can continue you can view\n" +
                        "upcoming and past events on the dashboard.",
                Selection = (Tile)sender,
            };
            ScreenManager.AddScreen(hfs, e.PlayerIndex);
        }

        void NewDay(object sender, PlayerIndexEventArgs e)
        {
            HelpFadeScreen hfs = new HelpFadeScreen()
            {
                Title = "This arrow will advance the game into the next day. You may only advance\n"+
                        "once you have completed all the days events and answered correctly on the\n" + 
                        "quiz.",
                Selection = (Tile)sender,
            };
            ScreenManager.AddScreen(hfs, e.PlayerIndex);
        }

        void LoadQuiz(object sender, PlayerIndexEventArgs e)
        {
            HelpFadeScreen hfs = new HelpFadeScreen()
            {
                Title = "This is the quiz.This icon on the right will bring up the quiz. You must\n" +
                        "Select the correct answer before you continue to the next day. The arrow\n" +
                        "in the top left will exit our of the quiz.",
                Selection = test,
            };
            QuizScreen quiz = new QuizScreen();
            hfs.Exit += delegate(object s, PlayerIndexEventArgs x)
            {
                quiz.ExitScreen();
            };
            ScreenManager.AddScreen(quiz, e.PlayerIndex);
            ScreenManager.AddScreen(hfs, e.PlayerIndex);
        }

        void Undo(object sender, PlayerIndexEventArgs e)
        {
            HelpFadeScreen hfs = new HelpFadeScreen()
            {
                Title = "This icon will undo the last transaction. This should be used if you pay more or less\n" +
                        "than you meant to.\n",
                Selection = undoButton,
            };
            ScreenManager.AddScreen(hfs, e.PlayerIndex);
        }

        #endregion
    }
}
