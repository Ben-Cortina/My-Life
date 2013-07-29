using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace MyLife
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class PaymentScreen : GameScreen
    {
        #region Fields And Properties

        ContentManager content;
        Texture2D buttonTexture, textBoxTexture;

        List<Tile> tiles = new List<Tile>();

        Texture2D gradientTexture;

        public Texture2D Background
        {
            get { return gradientTexture; }
            set { gradientTexture = value; }
        }

        SpriteFont font;

        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        GameScreen screen;

        Color fontColor = Color.Green;

        public GameScreen Screen
        {
            get { return screen; }
            set { screen = value; }
        }

        Rectangle paymentBox;

        int minimum;

        public int Minimum
        {
            get { return minimum; }
            set { minimum = value; }
        }

        int maximum;

        public int Maximum
        {
            get { return maximum; }
            set { maximum = value; }
        }

        string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        int amount=0;

        CalendarEvent payment;

        public CalendarEvent PaymentEvent
        {
            get { return payment; }
            set { payment = value; }
        }

        Rectangle credBox;
        Rectangle moneyBox;

        public event EventHandler<PlayerIndexEventArgs> Paid;

        #endregion

        #region Initialize

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PaymentScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            buttonTexture = content.Load<Texture2D>("button");
            textBoxTexture = content.Load<Texture2D>("textbox");
            font = content.Load<SpriteFont>("DashboardFont");
            gradientTexture = content.Load<Texture2D>("gradient");
            amount = minimum;
            Vector2 buttonSize = new Vector2(108, 40);
            if (minimum == maximum)
            {
                paymentBox = new Rectangle((int)(GlobalVariables.ScreenWidth * .25f),
                    (int)(GlobalVariables.ScreenHeight * .375f),
                    (int)(GlobalVariables.ScreenWidth * .5f),
                    (int)(GlobalVariables.ScreenHeight * .25f));
                #region Create Tiles
                Tile cashTile = new Tile()
                {
                    Text = "Cash",
                    TextColor = Color.Black,
                    Size = buttonSize,
                    Position = new Vector2(paymentBox.X + 3 * paymentBox.Width / 4 - (buttonSize.X + 2), paymentBox.Y + 104),
                    Background = buttonTexture,
                    Image = new Rectangle(0, 0, 159, 59),
                    AltImage = new Rectangle(180, 0, 159, 59),
                    TileIteraction = Tile.Interaction.Image,
                    Screen = this,
                    Font = font,
                    FontScale = .18f,
                    TileAlignment = Tile.Alignment.Fill,
                };
                cashTile.Selected += PayCash;
                tiles.Add(cashTile);

                Tile creditTile = new Tile()
                {
                    Text = "Credit",
                    TextColor = Color.Black,
                    Position = new Vector2(paymentBox.X + 3 * paymentBox.Width / 4 + 2, cashTile.Position.Y),
                    Background = buttonTexture,
                    Image = new Rectangle(0, 0, 159, 59),
                    AltImage = new Rectangle(180, 0, 159, 59),
                    TileIteraction = Tile.Interaction.Image,
                    Screen = this,
                    Font = font,
                    FontScale = .18f,
                    Size = buttonSize,
                    TileAlignment = Tile.Alignment.Fill,
                };
                creditTile.Selected += PayCredit;
                tiles.Add(creditTile);
            }
            else
            {
                paymentBox = new Rectangle((int)(GlobalVariables.ScreenWidth * .25f),
                    (int)(GlobalVariables.ScreenHeight * .3f),
                    (int)(GlobalVariables.ScreenWidth * .5f),
                    (int)(GlobalVariables.ScreenHeight * .4f));


                Tile one = new Tile()
                {
                    Text = "+ $1",
                    TextColor = Color.Black,
                    Position = new Vector2(paymentBox.X + paymentBox.Width/4 - (buttonSize.X+2), paymentBox.Y+144),
                    Background = buttonTexture,
                    Image = new Rectangle(0, 0, 159, 59),
                    AltImage = new Rectangle(180, 0, 159, 59),
                    TileIteraction = Tile.Interaction.Image,
                    Screen = this,
                    Font = font,
                    FontScale = .18f,
                    Size = buttonSize,
                    TileAlignment = Tile.Alignment.Fill,
                };
                one.Selected += delegate(object o, PlayerIndexEventArgs arg)
                {
                    if (amount < maximum)
                        amount += 1;
                };
                tiles.Add(one);

                Tile ten = new Tile()
                {
                    Text = "+ $10",
                    TextColor = Color.Black,
                    Position = new Vector2(one.Position.X, one.Position.Y + buttonSize.Y + 4),
                    Background = buttonTexture,
                    Image = new Rectangle(0, 0, 159, 59),
                    AltImage = new Rectangle(180, 0, 159, 59),
                    TileIteraction = Tile.Interaction.Image,
                    Screen = this,
                    Font = font,
                    FontScale = .18f,
                    Size = buttonSize,
                    TileAlignment = Tile.Alignment.Fill,
                };
                ten.Selected += delegate(object o, PlayerIndexEventArgs arg)
                {
                    if (amount < maximum-9)
                        amount += 10;
                };
                tiles.Add(ten);

                Tile hundred = new Tile()
                {
                    Text = "+ $100",
                    TextColor = Color.Black,
                    Position = new Vector2(ten.Position.X, ten.Position.Y + buttonSize.Y + 4),
                    Background = buttonTexture,
                    Image = new Rectangle(0, 0, 159, 59),
                    AltImage = new Rectangle(180, 0, 159, 59),
                    TileIteraction = Tile.Interaction.Image,
                    Screen = this,
                    Font = font,
                    FontScale = .18f,
                    Size = buttonSize,
                    TileAlignment = Tile.Alignment.Fill,
                };
                hundred.Selected += delegate(object o, PlayerIndexEventArgs arg)
                {
                    if (amount < maximum-99)
                        amount += 100;
                };
                tiles.Add(hundred);


                Tile oneM = new Tile()
                {
                    Text = "- $1",
                    TextColor = Color.Black,
                    Position = new Vector2(one.Position.X + buttonSize.X +4 , one.Position.Y),
                    Background = buttonTexture,
                    Image = new Rectangle(0, 0, 159, 59),
                    AltImage = new Rectangle(180, 0, 159, 59),
                    TileIteraction = Tile.Interaction.Image,
                    Screen = this,
                    Font = font,
                    FontScale = .18f,
                    Size = buttonSize,
                    TileAlignment = Tile.Alignment.Fill,
                };
                oneM.Selected += delegate(object o, PlayerIndexEventArgs arg)
                {
                    if (amount > minimum)
                        amount -= 1;
                };
                tiles.Add(oneM);

                Tile tenM = new Tile()
                {
                    Text = "- $10",
                    TextColor = Color.Black,
                    Position = new Vector2(oneM.Position.X, oneM.Position.Y + buttonSize.Y + 4),
                    Background = buttonTexture,
                    Image = new Rectangle(0, 0, 159, 59),
                    AltImage = new Rectangle(180, 0, 159, 59),
                    TileIteraction = Tile.Interaction.Image,
                    Screen = this,
                    Font = font,
                    FontScale = .18f,
                    Size = buttonSize,
                    TileAlignment = Tile.Alignment.Fill,
                };
                tenM.Selected += delegate(object o, PlayerIndexEventArgs arg)
                {
                    if (amount > minimum+9)
                        amount -= 10;
                };
                tiles.Add(tenM);

                Tile hundredM = new Tile()
                {
                    Text = "- $100",
                    TextColor = Color.Black,
                    Position = new Vector2(tenM.Position.X, tenM.Position.Y + buttonSize.Y + 4),
                    Background = buttonTexture,
                    Image = new Rectangle(0, 0, 159, 59),
                    AltImage = new Rectangle(180, 0, 159, 59),
                    TileIteraction = Tile.Interaction.Image,
                    Screen = this,
                    Font = font,
                    FontScale = .18f,
                    Size = buttonSize,
                    TileAlignment = Tile.Alignment.Fill,
                };
                hundredM.Selected += delegate(object o, PlayerIndexEventArgs arg)
                {
                    if (amount > minimum+99)
                        amount -= 100;
                };
                tiles.Add(hundredM);

                buttonSize = new Vector2(159, 59);
                Tile cashTile = new Tile()
                {
                    Text = "Cash",
                    TextColor = Color.Black,
                    Size = buttonSize,
                    Position = new Vector2(paymentBox.X + 3 * paymentBox.Width / 4 - (buttonSize.X/2), paymentBox.Y + 104),
                    Background = buttonTexture,
                    Image = new Rectangle(0, 0, 159, 59),
                    AltImage = new Rectangle(180, 0, 159, 59),
                    TileIteraction = Tile.Interaction.Image,
                    Screen = this,
                    Font = font,
                    FontScale = .20f,
                    TileAlignment = Tile.Alignment.Fill,
                };
                cashTile.Selected += PayCash;
                tiles.Add(cashTile);


            }
           #endregion





            credBox = new Rectangle((int)(GlobalVariables.ScreenWidth * .10f), paymentBox.Y +paymentBox.Height + 16, 450, 185);
            moneyBox = new Rectangle((int)(GlobalVariables.ScreenWidth * .6f), paymentBox.Y + paymentBox.Height + 16, 450, 150);
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

            if (InputDeviceState.LeftButton)
            {
                MouseState mouseState = Mouse.GetState();
                Point point = new Point(mouseState.X, mouseState.Y);
                if (!paymentBox.Contains(point))
                    ExitScreen();
                else if(!payment.Completed)
                    foreach (Tile tile in tiles)
                        tile.CheckPress(mouseState);
            }
        }

        #endregion

        #region Update and Draw

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
        /// Draws the Dashboard
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = ScreenManager.SpriteBatch;

            string amountString = "$"+ amount.ToString();
            float titleScale = .3f;
            float sectionScale = .24f;
            float numberScale = .18f;
            int balance = GlobalVariables.Finance.CreditCardBalance + GlobalVariables.Finance.CreditCardMonthBalance;
            string balanceString = "$"+ balance.ToString();
            Vector2 titleSize = font.MeasureString(title)*titleScale;

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            float size = font.MeasureString("Pay:").X * sectionScale;

            Vector2 amountPos = new Vector2(paymentBox.X + paymentBox.Width / 4 - size / 2, paymentBox.Y + 56);
            size = font.MeasureString("Using:").X * sectionScale;
            Vector2 usePos = new Vector2(paymentBox.X + 3*paymentBox.Width / 4 - size / 2, paymentBox.Y + 56);
            size = textBoxTexture.Width;
            Vector2 textBoxPos = new Vector2(paymentBox.X + paymentBox.Width / 4 - size / 2, paymentBox.Y + 104);
            Vector2 titlePos = new Vector2(paymentBox.X + paymentBox.Width / 2 - titleSize.X / 2,
                                           paymentBox.Y);
            size = font.MeasureString("PAID").X * .5f;
            Vector2 paidPos = new Vector2(paymentBox.X + paymentBox.Width / 2 - size / 2,
                                           paymentBox.Y + paymentBox.Height / 2 - (font.MeasureString("PAID").Y * .5f)/2);
            size = font.MeasureString("Credit Card Balance:").X * numberScale;
            Vector2 balanceTPos = new Vector2(paymentBox.X + 3*paymentBox.Width / 4 - size / 2, paymentBox.Y+164);

            size = font.MeasureString(balanceString).X * numberScale;
            Vector2 balancePos = new Vector2(paymentBox.X + 3*paymentBox.Width / 4 - size / 2, paymentBox.Y+204);


            //Create elemets for drawing
            String bankBalance = "$" + GlobalVariables.Finance.BankBalance.ToString();
            String creditBalance = "$" + GlobalVariables.Finance.CreditCardBalance.ToString();
            String monthBalance = "$" + GlobalVariables.Finance.CreditCardMonthBalance.ToString();
            String cash = "$" + GlobalVariables.Finance.Cash.ToString();
            String limit = "$" + GlobalVariables.Finance.CreditCardLimit.ToString();


            // Fade the popup alpha during transitions.
            Color white = Color.White * TransitionAlpha;
            Color green = Color.LightGreen *TransitionAlpha;
            Color black = Color.Black * TransitionAlpha;

            Rectangle credInfo = new Rectangle((int)credBox.X + 32,
                     (int)credBox.Y,
                     (int)credBox.Width - 32 * 2,
                     (int)credBox.Height - 8);

            Rectangle moneyInfo = new Rectangle((int)moneyBox.X + 32,
                     (int)moneyBox.Y,
                     (int)moneyBox.Width - 32 * 2,
                     (int)moneyBox.Height - 8);

            sb.Begin();

            // Draw the Box.
            DrawBox(sb, paymentBox, white);
            // Draw title
            DrawStringScale(sb, title, titlePos, white, titleScale);
            if (payment.Completed)
                DrawStringScale(sb, "PAID", paidPos, green, .5f);
            else
            {
                //Draw Pay Section
                DrawStringScale(sb, "Pay:", amountPos, white, sectionScale);
                sb.Draw(textBoxTexture, textBoxPos, white);
                DrawStringScale(sb, amountString, textBoxPos + new Vector2(2, -2), black, numberScale);

                //Draw Use section
                DrawStringScale(sb, "Using:", usePos, white, sectionScale);

                if (maximum != minimum)
                {
                    DrawStringScale(sb, "Credit Card Balance:", balanceTPos, white, numberScale);
                    DrawStringScale(sb, balanceString, balancePos, white, numberScale);
                }

                foreach (Tile tile in tiles)
                    tile.Draw(TransitionAlpha);
            }

            // Draw the bank Box.
            DrawBox(sb, credBox, white);
            DrawInfo(sb, "Credit Card", "Limit:", limit, "Monthly Balance:", monthBalance, "Total Balance:", creditBalance, credInfo, white);

            // Draw the money Box
            DrawBox(sb, moneyBox, white);
            DrawInfo(sb, "Personal Money", "Cash:", cash, "Bank Balance:", bankBalance, moneyInfo, white);
            sb.End();

        }

        void DrawBox(SpriteBatch sb, Rectangle area, Color color)
        {
            sb.Draw(gradientTexture, area, color);


        }

        void DrawInfo(SpriteBatch sb, String title, String info1, String boxString1, String info2, String boxString2, String info3, String boxString3, Rectangle area, Color color)
        {
            DrawInfo(sb, title, info1, boxString1, info2, boxString2, area, color);
            float titleScale = .25f;
            float infoScale = .15f;
            Color black = Color.Black * TransitionAlpha;
            Vector2 infoPos = new Vector2(area.X,
                                  area.Y + (font.MeasureString(title).Y * titleScale) + (font.MeasureString(info1).Y * infoScale) * 2 + 24);

            Vector2 textBoxPos = new Vector2(infoPos.X + (font.MeasureString(info3).X * infoScale) + 10,
                                             infoPos.Y);
            DrawStringScale(sb, info3, infoPos, color, infoScale);
            sb.Draw(textBoxTexture, textBoxPos, color);
            DrawStringScale(sb, boxString3, textBoxPos + new Vector2(2, 2), black, infoScale);
        }

        void DrawInfo(SpriteBatch sb, String title, String info1, String boxString1, String info2, String boxString2, Rectangle area, Color color)
        {
            float titleScale = .25f;
            float infoScale = .15f;
            Vector2 titlePos = new Vector2(area.X + area.Width / 2 - (font.MeasureString(title).X * titleScale) / 2,
                                           area.Y);

            Vector2 infoPos = new Vector2(area.X,
                                          area.Y + (font.MeasureString(title).Y * titleScale) + 8);

            Vector2 textBoxPos = new Vector2(infoPos.X + (font.MeasureString(info1).X * infoScale) + 10,
                                             infoPos.Y);

            Color black = Color.Black * TransitionAlpha;

            DrawStringScale(sb, title, titlePos, color, titleScale);
            DrawStringScale(sb, info1, infoPos, color, infoScale);
            sb.Draw(textBoxTexture, textBoxPos, color);
            DrawStringScale(sb, boxString1, textBoxPos + new Vector2(2, 2), black, infoScale);

            infoPos = new Vector2(infoPos.X,
                                  infoPos.Y + (font.MeasureString(info2).Y * infoScale) + 8);

            textBoxPos = new Vector2(infoPos.X + (font.MeasureString(info2).X * infoScale) + 10,
                                             infoPos.Y);

            DrawStringScale(sb, info2, infoPos, color, infoScale);
            sb.Draw(textBoxTexture, textBoxPos, color);
            DrawStringScale(sb, boxString2, textBoxPos + new Vector2(2, 2), black, infoScale);

        }

        void DrawStringScale(SpriteBatch sb, String text, Vector2 position, Color color, float scale)
        {
            sb.DrawString(font, text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        #endregion

        #region methods

        void PayCash(object sender, PlayerIndexEventArgs e)
        {
            if (GlobalVariables.Finance.Cash >= amount)
            {
                GlobalVariables.Finance.Cash -= amount;
                if (title == "Credit Card Payment")
                    GlobalVariables.Finance.CreditCardBalance -= amount;

                Transaction temp = new Transaction(title, amount);
                GlobalVariables.Finance.Transactions.Add(temp);

                OnPaid();

                payment.Completed = true;
            }

        }

        void PayCredit(object sender, PlayerIndexEventArgs e)
        {
            GlobalVariables.Finance.CreditCardMonthBalance += amount;

            Transaction temp = new Transaction(title, amount);
            GlobalVariables.Finance.Transactions.Add(temp);

            OnPaid();

            payment.Completed = true;

        }

        void OnPaid()
        {
            PlayerIndex d = new PlayerIndex();
            if (Paid != null)
                Paid(this, new PlayerIndexEventArgs(d));
        }

        #endregion

    }


}
