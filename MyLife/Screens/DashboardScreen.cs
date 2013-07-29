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
    public class DashboardScreen : GameScreen
    {
        #region Fields And Properties

        Calendar calendar;

        public Calendar Calendar
        {
            get { return calendar; }
            set { calendar = value; }
        }

        Texture2D gradientTexture;

        public Texture2D Background
        {
            get { return gradientTexture; }
            set { gradientTexture = value; }
        }

        Texture2D textBox;

        public Texture2D TextBox
        {
            get { return textBox; }
            set { textBox = value; }
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

        Rectangle credBox;
        Rectangle moneyBox;
        Rectangle calendarBox;
        Rectangle eventBox;

        #endregion

        #region Initialize

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DashboardScreen()
        {
            
            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        public void Initialize()
        {
            calendar.Initialize();
            calendarBox = new Rectangle((int)calendar.Position.X, (int)calendar.Position.Y, (int)calendar.Size.X, (int)calendar.Size.Y);
            credBox = new Rectangle(100, 100, 450, 185);
            moneyBox = new Rectangle(100, 400, 450, 150);
            eventBox = new Rectangle(700, 350, 450, 320);
            ResetDate();
        }

        public void ResetDate()
        {
            calendar.ResetDate();
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input.IsPauseGame(ControllingPlayer))
                ExitScreen();
            PlayerIndex pIndex;

            if (input.IsMenuCancel(ControllingPlayer, out pIndex) || input.IsDashboard(ControllingPlayer, out pIndex))
                ExitScreen();

            if (InputDeviceState.LeftButton)
            {
                MouseState mouseState = Mouse.GetState();
                Point point = new Point( mouseState.X, mouseState.Y);
                if (!calendarBox.Contains(point) && !credBox.Contains(point) && !moneyBox.Contains(point))
                    ExitScreen();
            }
                calendar.HandleInput();
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the Dashboard
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = screen.ScreenManager.SpriteBatch;

            string title = "Summary";
            Vector2 titleSize = font.MeasureString(title);

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            //sb.DrawString(font, title, position + new Vector2((size.X - titleSize.X) / 2, 50), fontColor);


            //Create elemets for drawing
            String bankBalance = "$" + GlobalVariables.Finance.BankBalance.ToString();
            String creditBalance = "$" + (GlobalVariables.Finance.CreditCardBalance + GlobalVariables.Finance.CreditCardMonthBalance).ToString();
            String monthBalance = "$" + (GlobalVariables.Finance.CreditCardMonthBalance).ToString();
            String limit = "$" + GlobalVariables.Finance.CreditCardLimit.ToString();
            String cash = "$" + GlobalVariables.Finance.Cash.ToString();


            // Fade the popup alpha during transitions.
            Color white = Color.White*TransitionAlpha;

            Rectangle credInfo = new Rectangle((int)credBox.X + 32,
                     (int)credBox.Y,
                     (int)credBox.Width - 32 * 2,
                     (int)credBox.Height - 8);
            Rectangle moneyInfo = new Rectangle((int)moneyBox.X + 32,
                     (int)moneyBox.Y,
                     (int)moneyBox.Width - 32 * 2,
                     (int)moneyBox.Height - 8);
            Rectangle eventInfo = new Rectangle((int)eventBox.X + 32,
                     (int)eventBox.Y,
                     (int)eventBox.Width - 32 * 2,
                     (int)eventBox.Height - 8);

            sb.Begin();

            // Draw the bank Box.
            DrawBox(sb, credBox, white);
            DrawInfo(sb, "Credit Card", "Limit:", limit, "Monthly Balance:",monthBalance, "Total Balance:", creditBalance, credInfo, white);

            // Draw the money Box
            DrawBox(sb, moneyBox, white);
            DrawInfo(sb, "Personal Money", "Cash:", cash, "Bank Balance:", bankBalance, moneyInfo, white);

            // Draw the eventbox.
            DrawBox(sb, eventBox, white);
            DrawEventBoxInfo(sb, calendar.SelectedEvents, eventInfo, white);

            //Draw Calendar
            calendar.Draw(TransitionAlpha);
            sb.End();

        }

        void DrawBox(SpriteBatch sb, Rectangle area,Color color)
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
                                  area.Y + (font.MeasureString(title).Y * titleScale) + (font.MeasureString(info1).Y * infoScale)*2 +24 );

            Vector2 textBoxPos = new Vector2(infoPos.X + (font.MeasureString(info3).X * infoScale) + 10,
                                             infoPos.Y);
            DrawStringScale(sb, info3, infoPos, color, infoScale);
            sb.Draw(textBox, textBoxPos, color);
            DrawStringScale(sb, boxString3, textBoxPos + new Vector2(2, 2), black, infoScale);
        }

        void DrawInfo(SpriteBatch sb, String title, String info1, String boxString1, String info2, String boxString2, Rectangle area, Color color)
        {
            float titleScale = .25f;
            float infoScale = .15f;
            Vector2 titlePos = new Vector2(area.X + area.Width / 2 - (font.MeasureString(title).X * titleScale) / 2,
                                           area.Y);

            Vector2 infoPos = new Vector2(area.X,
                                          area.Y + (font.MeasureString(title).Y * titleScale)+8);

            Vector2 textBoxPos = new Vector2(infoPos.X + (font.MeasureString(info1).X * infoScale) + 10,
                                             infoPos.Y );

            Color black = Color.Black * TransitionAlpha;

            DrawStringScale(sb, title, titlePos, color, titleScale);
            DrawStringScale(sb, info1, infoPos, color, infoScale);
            sb.Draw(textBox, textBoxPos, color);
            DrawStringScale(sb, boxString1, textBoxPos + new Vector2(2,2), black, infoScale);

            infoPos = new Vector2(infoPos.X,
                                  infoPos.Y + (font.MeasureString(info2).Y * infoScale)+8);

            textBoxPos = new Vector2(infoPos.X + (font.MeasureString(info2).X * infoScale) + 10,
                                             infoPos.Y);

            DrawStringScale(sb, info2, infoPos, color, infoScale);
            sb.Draw(textBox, textBoxPos, color);
            DrawStringScale(sb, boxString2, textBoxPos + new Vector2(2, 2), black, infoScale);

        }

        void DrawStringScale(SpriteBatch sb, String text, Vector2 position, Color color, float scale)
        {
            sb.DrawString(font, text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        void DrawEventBoxInfo(SpriteBatch sb, List<CalendarEvent> events, Rectangle area, Color color)
        {
            float titleScale = .25f;
            float infoScale = .15f;
            float subtitleScale = .18f;
            string title = "Events on " + calendar.SelectedDate.ToShortDateString();
            string amount;
            Vector2 titlePos = new Vector2(area.X + area.Width / 2 - (font.MeasureString(title).X * titleScale) / 2,
                                              area.Y);
            Vector2 infoPos = new Vector2(area.X, area.Y + (font.MeasureString(title).Y * titleScale+10));

            DrawStringScale(sb, title, titlePos, color, titleScale);

            DrawStringScale(sb, "Event Name", infoPos, color, subtitleScale);
            DrawStringScale(sb, "Amount", new Vector2(infoPos.X +250, infoPos.Y), color, subtitleScale);
            infoPos.Y += 40;
            foreach(CalendarEvent e in events)
            {
                if (e.Name == "Credit Card Payment")
                {
                    int limit = GlobalVariables.Finance.CreditCardLimit;
                    int balance = GlobalVariables.Finance.CreditCardBalance;
                    int monthBalance = GlobalVariables.Finance.CreditCardMonthBalance;
                    int fees = 0;
                    if (monthBalance > limit)
                        fees += 35;
                    amount = "$" + ((int)Math.Ceiling((balance + monthBalance) * .01f + fees)).ToString();
                }
                else if (e.Amount == 0)
                    amount = "Unknown";
                else
                    amount = "$" + e.Amount;
                DrawStringScale(sb, e.Name, infoPos, color, infoScale);
                DrawStringScale(sb, amount, new Vector2(infoPos.X + 250, infoPos.Y), color, infoScale);
                infoPos.Y += 30;
            }
        }

        #endregion

    }


}
