using System;
#if WINDOWS
using System.Windows.Forms;
#endif
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
    public class Calendar
    {
        #region Fields and Properties

        Random random = new Random();
        List<Tile> days = new List<Tile>();
        List<Tile> weekTiles = new List<Tile>();

        Vector2 ratio = new Vector2(0f, 0f);
        Vector2 tileSize;

        string[] weeks = { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };

        Texture2D background;

        public Texture2D Background
        {
            get { return background; }
            set { background = value; }
        }
        Color normalColor = Color.Black;
        Color highlightColor = Color.Red;

        SpriteFont font;

        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        Vector2 position, size;

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        GameScreen screen;

        public GameScreen Screen
        {
            get { return screen; }
            set { screen = value; }
        }

        List<CalendarEvent> events;

        /// <summary>
        /// List of Calendar Events
        /// </summary>
        public List<CalendarEvent> Events
        {
            get { return events; }
            set { events = value; }
        }

        DateTime selectedDate;

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; }
        }

        List<CalendarEvent> selectedEvents;

        public List<CalendarEvent> SelectedEvents
        {
            get { return selectedEvents; }
            set { selectedEvents = value; }
        }

        #endregion

        #region Initialization

        public Calendar()
        {
            // TODO: Construct any child components here
            events = new List<CalendarEvent>();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization code here

            tileSize.X = size.X / (7 + 6 * ratio.X);
            tileSize.Y = 0.7f * size.Y / (6 + 5 * ratio.Y);

            for (int i = 0; i < weeks.Length; i++)
            {
                Tile week = new Tile()
                {
                    Text = weeks[i].ToString(),
                    Size = tileSize,
                    Position = position + new Vector2(i * (1 + ratio.X) * tileSize.X,
                        size.Y * 0.3f),
                    TextColor = Color.Black,
                    Screen = screen,
                    Font = font
                };

                weekTiles.Add(week);
                UpdateCalendar();
            }

        }

        public void ResetDate()
        {
            selectedDate = GlobalVariables.Date;
            selectedEvents = DateEvents(selectedDate);
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            foreach (Tile tile in days)
                tile.Update(gameTime);
        }

        #endregion

        #region Handle Input

        public void HandleInput()
        {
            if (InputDeviceState.LeftButton)
            {
                MouseState mouseState = Mouse.GetState();
                foreach (Tile tile in days)
                    tile.CheckPress(mouseState);
            }
        }

        #endregion

        #region Draw

        public void Draw(float alpha)
        {
            SpriteBatch sb = screen.ScreenManager.SpriteBatch;
            Color white = Color.White * alpha;

            sb.Draw(background, new Rectangle((int) position.X, (int) Position.Y,
                (int) Size.X, (int) Size.Y), null, white);

            string currentMonth = GlobalVariables.Date.ToString("Y");
            Vector2 monthSize = font.MeasureString(currentMonth);

            sb.DrawString(font, currentMonth, position + new Vector2(size.X/2 - monthSize.X/2, size.Y * 0.13f) , white);

            foreach (Tile week in weekTiles)
                week.Draw(alpha);

            foreach (Tile today in days)
                today.Draw(alpha);
        }

        #endregion

        void UpdateCalendar()
        {
            days.Clear();
            int week = 1;
            Color color;
            for (DateTime temp = new DateTime(GlobalVariables.Date.Year, GlobalVariables.Date.Month, 1);
                temp.Month == GlobalVariables.Date.Month; temp = temp.AddDays(1))
            {
                if (temp == GlobalVariables.Date)
                    color = Color.DarkGreen;
                else
                {
                    color = normalColor;
                    foreach (CalendarEvent e in events)
                    {
                        if (e.DueDate == temp)
                        {
                            color = highlightColor; 
                            break;
                        }
                    }
                }

                Tile tile = new Tile()
                {
                    Text = temp.Day.ToString(),
                    Position = position + new Vector2((int)temp.DayOfWeek * (1 + ratio.X) * tileSize.X,
                        (1 + ratio.Y) * week * tileSize.Y + size.Y * 0.3f),
                    Screen = screen,
                    Font = font,
                    TextColor = color,
                    Size = tileSize,
                };
                tile.Selected += DateSelected;
                    
                days.Add(tile);

                if (temp.DayOfWeek == DayOfWeek.Saturday)
                    week++;
            }
        }

        public void NewDay()
        {
            GlobalVariables.Date = GlobalVariables.Date.AddDays(1);
            GenerateSupriseEvents();
            UpdateCalendar();
        }

        /// <summary>
        /// Generates events for that day. will always have atleast one event but no more than 3
        /// </summary>
        public void GenerateSupriseEvents()
        {
            Random n = new Random();
            List<CalendarEvent> evs = new List<CalendarEvent>();
            evs = DateEvents(GlobalVariables.Date);
            int ran = n.Next(0, 4 - evs.Count);

            //make sure there is at least 1 event
            if (evs.Count == 0 && ran == 0)
                SupriseEvent(n);

            //generate a random number of events
            for (int i=0;i<ran;i++)
                SupriseEvent(n);

        }

        void SupriseEvent(Random n)
        {
            CalendarEvent temp;
            int ran = n.Next(0, 100);
            if (ran < 5)
            {
                temp = new CalendarEvent()
                {
                    Name = "Car Repair",
                    Amount = n.Next(1,40)*50,
                    DueDate = GlobalVariables.Date,
                };
            }
            else if (ran < 15)
            {
                temp = new CalendarEvent()
                {
                    Name = "Receive Medical Help",
                    Amount = n.Next(1, 40) * 50,
                    DueDate = GlobalVariables.Date,
                };
            }
            else if (ran < 35)
            {
                temp = new CalendarEvent()
                {
                    Name = "Get Gas",
                    Amount = n.Next(40, 80) ,
                    DueDate = GlobalVariables.Date,
                };
            }
            else if (ran < 65)
            {
                temp = new CalendarEvent()
                {
                    Name = "Bought Luxury",
                    Amount = n.Next(40, 300),
                    DueDate = GlobalVariables.Date,
                };
            }
            else
            {
                temp = new CalendarEvent()
                {
                    Name = "Random Expense",
                    Amount = n.Next(1, 50),
                    DueDate = GlobalVariables.Date,
                };
            }
            events.Add(temp);
        }

        /// <summary>
        /// Checks to see which events need to be triggered
        /// </summary>
        void CheckDues()
        {
            foreach (CalendarEvent cE in events)
                cE.DueCheck();
        }


        public void DateSelected(object o, PlayerIndexEventArgs arg)
        {
            int day =  Convert.ToInt32(((Tile)o).Text);
            selectedDate = new DateTime(GlobalVariables.Date.Year, GlobalVariables.Date.Month, day);
            selectedEvents =  DateEvents(selectedDate);
        }
        /// <summary>
        /// returns all events due on the passed day.
        /// </summary>
        public List<CalendarEvent> DateEvents( DateTime date)
        { 
            List<CalendarEvent> evs = new List<CalendarEvent>();
            foreach (CalendarEvent e in events)
                if (e.DueDate == date)
                    evs.Add(e);
            return evs;
        }



    }
}
