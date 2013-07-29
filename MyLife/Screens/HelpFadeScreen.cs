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
    public class HelpFadeScreen : GameScreen
    {
        #region Fields And Properties

        ContentManager content;
        Texture2D circleTexture;

        Tile selection;

        public Tile Selection
        {
            get { return selection; }
            set { selection = value; }
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

        string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        Vector2 titleLoc;
        float titleScale;
        Rectangle circleLoc;


        public event EventHandler<PlayerIndexEventArgs> Exit;

        #endregion

        #region Initialize

        /// <summary>
        /// Default Constructor
        /// </summary>
        public HelpFadeScreen()
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
            circleTexture = content.Load<Texture2D>("circle");
            font = content.Load<SpriteFont>("gamefont");

            titleScale = .3f;
            Vector2 titleSize = font.MeasureString(title)*titleScale;
            if ((selection.Position.Y + selection.Size.Y/2) <= GlobalVariables.ScreenHeight/2)
                titleLoc = new Vector2(GlobalVariables.ScreenWidth/2 - titleSize.X/2,GlobalVariables.ScreenHeight - titleSize.Y);
            else
                titleLoc = new Vector2(GlobalVariables.ScreenWidth / 2 - titleSize.X / 2, 8);

            int x = (int)(selection.Position.X - selection.Size.X/2);
            int y = (int)(selection.Position.Y - selection.Size.Y/2);
            int width = (int)(selection.Size.X*2);
            int height = (int)(selection.Size.Y*2);
            circleLoc = new Rectangle(x, y, width, height);
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
                OnExit(pIndex);
            if (input.IsDashboard(ControllingPlayer, out pIndex))
                OnExit(pIndex);

            if (InputDeviceState.LeftButton)
            {
                OnExit(pIndex);
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the Help selection
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 3/4);

            SpriteBatch sb = ScreenManager.SpriteBatch;

            sb.Begin();

            Color white = Color.White * TransitionAlpha;
            Color red = new Color(255,50,50,255) * TransitionAlpha;

            DrawStringScale(sb, title, titleLoc, red, titleScale);
   
            selection.Draw(TransitionAlpha);

            sb.Draw(circleTexture, circleLoc, white);

            sb.End();

        }

        void DrawStringScale(SpriteBatch sb, String text, Vector2 position, Color color, float scale)
        {
            sb.DrawString(font, text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        #endregion

        #region Exit

        void OnExit(PlayerIndex playerIndex)
        {
            if (Exit != null)
                Exit(this, new PlayerIndexEventArgs(playerIndex));
            ExitScreen();
        }

        #endregion

    }


}
