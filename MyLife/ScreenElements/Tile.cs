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
    public class Tile
    {
        #region Fields and Properties

        public enum Alignment { Center, Fill };
        public enum Interaction { None, Image};
        Vector2 position;

        bool visible = true;

        public bool IsVisible
        {
            get { return visible; }
            set { visible = value; }
        }
        CalendarEvent linkedEvent;

        public CalendarEvent LinkedEvent
        {
            get { return linkedEvent; }
            set { linkedEvent = value; }
        }

        Alignment alignment = Alignment.Center;

        internal Alignment TileAlignment
        {
            get { return alignment; }
            set { alignment = value; }
        }

        bool interact = false;

        Interaction interaction = Interaction.None;

        /// <summary>
        /// How the tile behaves when pressed
        /// </summary>
        internal Interaction TileIteraction
        {
            get { return interaction; }
            set { interaction = value; }
        }

        Rectangle image;

        /// <summary>
        /// rectangle of the default image
        /// </summary>
        public Rectangle Image
        {
            get { return image; }
            set 
            { 
                image = value;
                if (altImage.IsEmpty)
                    altImage = image;
            }
        }

        Rectangle source;
        Rectangle altImage;

        /// <summary>
        /// rectangle of the alternate image
        /// </summary>
        public Rectangle AltImage
        {
            get { return altImage; }
            set { altImage = value; }
        }

        float pressScale=1f;

        /// <summary>
        /// scale to shrink to.
        /// </summary>
        public float Scale
        {
            get { return pressScale; }
            set { pressScale = value; }
        }

        float fontScale = 1f;

        /// <summary>
        /// scale to shrink font.
        /// </summary>
        public float FontScale
        {
            get { return fontScale; }
            set { fontScale = value; }
        }

        protected Rectangle rectangle;

        public Vector2 Position
        {
            get { return position; }
            set 
            { 
                position = value;
                rectangle = new Rectangle((int)Position.X, (int)position.Y, (int)size.X, (int)size.Y);
            }
        }

        string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        Vector2 size;

        public Vector2 Size
        {
            get { return size; }
            set
            {
                size = value;
                rectangle = new Rectangle((int)Position.X, (int)position.Y, (int)size.X, (int)size.Y);
            }
        }

        Texture2D background;

        public Texture2D Background
        {
            get { return background; }
            set { 
                background = value;
                if (image.IsEmpty)
                    Image = background.Bounds;
                }
        }

        SpriteFont font;

        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        GameScreen screen;

        public GameScreen Screen
        {
            get { return screen; }
            set { screen = value; }
        }

        Color textColor;

        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        Color buttonColor = Color.White;

        public Color ButtonColor
        {
            get { return buttonColor; }
            set { buttonColor = value; }
        }

        #endregion

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Selected;

        public Tile()
        {
            // TODO: Construct any child components here            
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization code here
        }

        /// <summary>
        /// Update the tile, called each game update
        /// </summary>
        public void Update(GameTime gameTime)
        {
            //TODO: Add Update Code

            if (interact && visible)
            {
                MouseState state = Mouse.GetState();
                if (state.LeftButton == ButtonState.Released)
                    interact = false;
            }

        }

        /// <summary>
        /// Checks to see if the mouse is over the tile and the tile is in focus
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void CheckPress(MouseState mouseState)
        {
            if (visible)
            {
                if (Selected != null)
                {
                    if (IsInFocus(mouseState.X, mouseState.Y))
                    {
                        Selected(this, new PlayerIndexEventArgs(PlayerIndex.One));
                        if (interaction != Interaction.None)
                            interact = true;
                    }
                }

            }
        }



        public void Draw()
        {
            Draw(1f);
        }

        /// <summary>
        /// Draw with alpha, because transparency is really cool ;)
        /// </summary>
        public void Draw(float alpha)
        {
            if (visible)
            {
                SpriteBatch spriteBatch = screen.ScreenManager.SpriteBatch;

                float scale = 1f;
                Rectangle drawRect;
                if (background != null)
                {
                    if (image == Rectangle.Empty)
                        image = background.Bounds;
                    if (interact)
                    {
                        source = altImage;
                        scale = pressScale;
                    }
                    else
                        source = image;

                    if (alignment == Alignment.Center)
                        drawRect = new Rectangle((int)(position.X + size.X / 2f - background.Width / 2),
                            (int)(position.Y + size.Y / 2 - background.Height / 2f), (int)source.Width, (int)source.Height);
                    else
                        drawRect = rectangle;

                    drawRect = new Rectangle((int)(drawRect.X + (drawRect.Width - drawRect.Width * scale) / 2),
                                              (int)(drawRect.Y + (drawRect.Height - drawRect.Height * scale) / 2),
                                              (int)(drawRect.Width * scale),
                                              (int)(drawRect.Height * scale));
                    spriteBatch.Draw(background, drawRect, source, buttonColor * alpha, 0, Vector2.Zero, SpriteEffects.None, 0.1f);
                }

                if (text != null)
                {
                    List<string> strings = SpliceString(text);
                    Vector2 textSize;
                    int i=0;
                    foreach (string s in strings)
                    {
                        textSize = font.MeasureString(s) * fontScale;
                        spriteBatch.DrawString(font, s, new Vector2(position.X + size.X / 2 - textSize.X / 2,
                            position.Y + size.Y / 2 - (textSize.Y * strings.Count) / 2 + textSize.Y * i), textColor * alpha, 0, Vector2.Zero, fontScale * scale, SpriteEffects.None, 0f);
                        i++;
                    }
                }
            }
            
        }

        List<string> SpliceString(string str)
        {
            Vector2 textSize = font.MeasureString(str) * fontScale;
            List<string> final = new List<string>();
            final.Add(str);
            if (textSize.X > size.X && str.IndexOf(" ") != -1)
            {
                final.Clear();
                string line = "";
                string test = "";
                List<string> spliced = new List<string>();
                spliced = Splice(str);
                int i=0;
                while (i < spliced.Count)
                {
                    test = spliced[i];
                    textSize = font.MeasureString(line+ " " +test) * fontScale;
                    if (textSize.X > size.X)
                    {
                        final.Add(line);
                        line = test;
                    }
                    else
                        line += " " + test;
                    i++;
                }
                final.Add(line);
            }
            return final;
        }

        List<string> Splice(string str)
        {
            List<string> spliced = new List<string>();
            string front;
            while (str.IndexOf(" ") != -1)
            {
                front = str.Substring(0, str.IndexOf(" ")+1);
                str = str.Substring(str.IndexOf(" ")+1, str.Length - (str.IndexOf(" ")+1));
                spliced.Add(front);
            }
            spliced.Add(str);
            return spliced;
        }

        public bool IsInFocus(int x, int y)
        {
            if (rectangle.Contains(new Point(x, y)))
                return true;
            else
                return false;
        }
    }
}
