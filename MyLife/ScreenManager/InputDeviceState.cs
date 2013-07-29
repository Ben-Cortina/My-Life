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
    static class InputDeviceState
    {

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        /// 

        static TimeSpan elapsedTime = TimeSpan.Zero;
        static TimeSpan scanInterval = TimeSpan.FromMilliseconds(250);
        static Dictionary<Keys, bool> keyStates = new Dictionary<Keys,bool>();

        static InputDeviceState()
        {
            keyStates.Add(Keys.A, false);
            keyStates.Add(Keys.Add, false);
            keyStates.Add(Keys.B, false);
            keyStates.Add(Keys.Back, false);
            keyStates.Add(Keys.C, false);
            keyStates.Add(Keys.D, false);
            keyStates.Add(Keys.Delete, false);
            keyStates.Add(Keys.Down, false);
            keyStates.Add(Keys.E, false);
            keyStates.Add(Keys.End, false);
            keyStates.Add(Keys.Enter, false);
            keyStates.Add(Keys.Escape, false);
            keyStates.Add(Keys.F, false);
            keyStates.Add(Keys.F1, false);
            keyStates.Add(Keys.F10, false);
            keyStates.Add(Keys.F11, false);
            keyStates.Add(Keys.F12, false);
            keyStates.Add(Keys.F13, false);
            keyStates.Add(Keys.F14, false);
            keyStates.Add(Keys.F15, false);
            keyStates.Add(Keys.F16, false);
            keyStates.Add(Keys.F17, false);
            keyStates.Add(Keys.F18, false);
            keyStates.Add(Keys.F19, false);
            keyStates.Add(Keys.F2, false);
            keyStates.Add(Keys.F20, false);
            keyStates.Add(Keys.F21, false);
            keyStates.Add(Keys.F22, false);
            keyStates.Add(Keys.F23, false);
            keyStates.Add(Keys.F24, false);
            keyStates.Add(Keys.F3, false);
            keyStates.Add(Keys.F4, false);
            keyStates.Add(Keys.F5, false);
            keyStates.Add(Keys.F6, false);
            keyStates.Add(Keys.F7, false);
            keyStates.Add(Keys.F8, false);
            keyStates.Add(Keys.F9, false);
            keyStates.Add(Keys.G, false);
            keyStates.Add(Keys.H, false);
            keyStates.Add(Keys.Help, false);
            keyStates.Add(Keys.Home, false);
            keyStates.Add(Keys.I, false);
            keyStates.Add(Keys.Insert, false);
            keyStates.Add(Keys.J, false);
            keyStates.Add(Keys.K, false);
            keyStates.Add(Keys.L, false);
            keyStates.Add(Keys.Left, false);
            keyStates.Add(Keys.LeftAlt, false);
            keyStates.Add(Keys.LeftControl, false);
            keyStates.Add(Keys.LeftShift, false);
            keyStates.Add(Keys.LeftWindows, false);
            keyStates.Add(Keys.M, false);
            keyStates.Add(Keys.N, false);
            keyStates.Add(Keys.NumLock, false);
            keyStates.Add(Keys.NumPad0, false);
            keyStates.Add(Keys.NumPad1, false);
            keyStates.Add(Keys.NumPad2, false);
            keyStates.Add(Keys.NumPad3, false);
            keyStates.Add(Keys.NumPad4, false);
            keyStates.Add(Keys.NumPad5, false);
            keyStates.Add(Keys.NumPad6, false);
            keyStates.Add(Keys.NumPad7, false);
            keyStates.Add(Keys.NumPad8, false);
            keyStates.Add(Keys.NumPad9, false);
            keyStates.Add(Keys.O, false);
            keyStates.Add(Keys.P, false);
            keyStates.Add(Keys.PageDown, false);
            keyStates.Add(Keys.PageUp, false);
            keyStates.Add(Keys.Q, false);
            keyStates.Add(Keys.R, false);
            keyStates.Add(Keys.Right, false);
            keyStates.Add(Keys.RightAlt, false);
            keyStates.Add(Keys.RightControl, false);
            keyStates.Add(Keys.RightShift, false);
            keyStates.Add(Keys.RightWindows, false);
            keyStates.Add(Keys.S, false);
            keyStates.Add(Keys.Space, false);
            keyStates.Add(Keys.Subtract, false);
            keyStates.Add(Keys.T, false);
            keyStates.Add(Keys.Tab, false);
            keyStates.Add(Keys.U, false);
            keyStates.Add(Keys.Up, false);
            keyStates.Add(Keys.V, false);
            keyStates.Add(Keys.W, false);
            keyStates.Add(Keys.X, false);
            keyStates.Add(Keys.Y, false);
            keyStates.Add(Keys.Z, false);
        }

        public static TimeSpan ScanInterval
        {
            get { return InputDeviceState.scanInterval; }
            set { InputDeviceState.scanInterval = value; }
        }
        static bool up = false, down = false, left = false, right = false;
        static bool a = false, b = false, x = false, y = false, back = false, start = false;
        static bool leftButton = false, rightButton = false, middleButton = false;

        public static bool MiddleButton
        {
            get { return InputDeviceState.middleButton; }
            set { InputDeviceState.middleButton = value; }
        }

        public static bool RightButton
        {
            get { return InputDeviceState.rightButton; }
            set { InputDeviceState.rightButton = value; }
        }

        public static bool LeftButton
        {
            get { return InputDeviceState.leftButton; }
            set { InputDeviceState.leftButton = value; }
        }

        public static bool Start
        {
            get { return InputDeviceState.start; }
            set { InputDeviceState.start = value; }
        }

        public static bool Back
        {
            get { return InputDeviceState.back; }
            set { InputDeviceState.back = value; }
        }

        public static bool Y
        {
            get { return InputDeviceState.y; }
            set { InputDeviceState.y = value; }
        }

        public static bool X
        {
            get { return InputDeviceState.x; }
            set { InputDeviceState.x = value; }
        }

        public static bool B
        {
            get { return InputDeviceState.b; }
            set { InputDeviceState.b = value; }
        }

        public static bool A
        {
            get { return InputDeviceState.a; }
            set { InputDeviceState.a = value; }
        }

        public static bool Right
        {
            get { return InputDeviceState.right; }
            set { InputDeviceState.right = value; }
        }

        public static bool Left
        {
            get { return InputDeviceState.left; }
            set { InputDeviceState.left = value; }
        }

        public static bool Down
        {
            get { return InputDeviceState.down; }
            set { InputDeviceState.down = value; }
        }

        public static bool Up
        {
            get { return InputDeviceState.up; }
            set { InputDeviceState.up = value; }
        }

        static GamePadState oldGamePadState = GamePad.GetState(PlayerIndex.One); 
        static GamePadState gamePadState;
        static KeyboardState oldKeyboardState = Keyboard.GetState();
        static KeyboardState keyboardState;
        static MouseState oldMouseState = Mouse.GetState(), mouseState;

        //public static GamePadState OldGamePadState
        //{
        //    get { return InputDeviceState.oldGamePadState; }
        //    set { InputDeviceState.oldGamePadState = value; }
        //}

        static public void Initialize()
        {
            // TODO: Add your initialization code here
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        static public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            up = down = left = right = false;
            a = b = x = y = back = start = false;
            leftButton = middleButton = rightButton = false;
            Dictionary<Keys, bool>.KeyCollection allKeys = keyStates.Keys;
            for (int i = 0; i < allKeys.Count; i++)
                keyStates[allKeys.ElementAt(i)] = false;
         
            gamePadState = GamePad.GetState(PlayerIndex.One);
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            bool resetElapsedTime = false;
            if (oldGamePadState.DPad.Down == ButtonState.Released &&
                gamePadState.DPad.Down == ButtonState.Pressed ||
                gamePadState.DPad.Down == ButtonState.Pressed &&
                elapsedTime > scanInterval ||
                oldGamePadState.ThumbSticks.Left.Y == 0 &&
                gamePadState.ThumbSticks.Left.Y < 0 ||
                gamePadState.ThumbSticks.Left.Y < -0.3 &&
                elapsedTime > scanInterval ||
                oldKeyboardState.IsKeyUp(Keys.Down) &&
                keyboardState.IsKeyDown(Keys.Down) ||
                keyboardState.IsKeyDown(Keys.Down) &&
                elapsedTime > scanInterval)
            {
                down = true;
                resetElapsedTime = true;
            }
            if (oldGamePadState.DPad.Up == ButtonState.Released &&
                gamePadState.DPad.Up == ButtonState.Pressed ||
                gamePadState.DPad.Up == ButtonState.Pressed &&
                elapsedTime > scanInterval ||
                oldGamePadState.ThumbSticks.Left.Y == 0 &&
                gamePadState.ThumbSticks.Left.Y > 0 ||
                gamePadState.ThumbSticks.Left.Y > 0.3 &&
                elapsedTime > scanInterval ||
                oldKeyboardState.IsKeyUp(Keys.Up) &&
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.Up) &&
                elapsedTime > scanInterval)
            {
                up = true;
                resetElapsedTime = true;
            }
            if (oldGamePadState.DPad.Left == ButtonState.Released &&
                gamePadState.DPad.Left == ButtonState.Pressed ||
                gamePadState.DPad.Left == ButtonState.Pressed &&
                elapsedTime > scanInterval ||
                gamePadState.ThumbSticks.Left.X < -0.3 &&
                elapsedTime > scanInterval ||
                oldGamePadState.ThumbSticks.Left.X == 0 &&
                gamePadState.ThumbSticks.Left.X < 0 ||
                oldKeyboardState.IsKeyUp(Keys.Left) &&
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.Left) &&
                elapsedTime > scanInterval)
            {
                left = true;
                resetElapsedTime = true;
            }
            if (oldGamePadState.DPad.Right == ButtonState.Released &&
                gamePadState.DPad.Right == ButtonState.Pressed ||
                gamePadState.DPad.Right == ButtonState.Pressed &&
                elapsedTime > scanInterval ||
                gamePadState.ThumbSticks.Left.X > 0.3 &&
                elapsedTime > scanInterval ||
                oldGamePadState.ThumbSticks.Left.X == 0 &&
                gamePadState.ThumbSticks.Left.X > 0 ||
                oldKeyboardState.IsKeyUp(Keys.Right) &&
                keyboardState.IsKeyDown(Keys.Right) ||
                keyboardState.IsKeyDown(Keys.Right) &&
                elapsedTime > scanInterval)
            {
                right = true;
                resetElapsedTime = true;
            }
            if (oldGamePadState.Buttons.A == ButtonState.Released &&
                gamePadState.Buttons.A == ButtonState.Pressed ||
                gamePadState.Buttons.A == ButtonState.Pressed &&
                elapsedTime > scanInterval)
            {
                a = true;
                resetElapsedTime = true;
            }
            if (oldGamePadState.Buttons.B == ButtonState.Released &&
                gamePadState.Buttons.B == ButtonState.Pressed ||
                gamePadState.Buttons.B == ButtonState.Pressed &&
                elapsedTime > scanInterval)
            {
                b = true;
                resetElapsedTime = true;
            }
            if (oldGamePadState.Buttons.X == ButtonState.Released &&
                gamePadState.Buttons.X == ButtonState.Pressed ||
                gamePadState.Buttons.X == ButtonState.Pressed &&
                elapsedTime > scanInterval)
            {
                x = true;
                resetElapsedTime = true;
            }
            if (oldGamePadState.Buttons.Y == ButtonState.Released &&
                gamePadState.Buttons.Y == ButtonState.Pressed ||
                gamePadState.Buttons.Y == ButtonState.Pressed &&
                elapsedTime > scanInterval)
            {
                y = true;
                resetElapsedTime = true;
            }
            if (oldGamePadState.Buttons.Back == ButtonState.Released &&
                gamePadState.Buttons.Back == ButtonState.Pressed ||
                gamePadState.Buttons.Back == ButtonState.Pressed &&
                elapsedTime > scanInterval)
            {
                back = true;
                resetElapsedTime = true;
            }
            if (oldGamePadState.Buttons.Start == ButtonState.Released &&
                gamePadState.Buttons.Start == ButtonState.Pressed ||
                gamePadState.Buttons.Start == ButtonState.Pressed &&
                elapsedTime > scanInterval)
            {
                start = true;
                resetElapsedTime = true;
            }
            if (oldMouseState.LeftButton == ButtonState.Released &&
                mouseState.LeftButton == ButtonState.Pressed ||
                mouseState.LeftButton == ButtonState.Pressed &&
                elapsedTime > scanInterval)
            {
                leftButton = true;
                resetElapsedTime = true;
            }
            if (oldMouseState.MiddleButton == ButtonState.Released &&
                mouseState.MiddleButton == ButtonState.Pressed ||
                mouseState.MiddleButton == ButtonState.Pressed &&
                elapsedTime > scanInterval)
            {
                middleButton = true;
                resetElapsedTime = true;
            }
            if (oldMouseState.RightButton == ButtonState.Released &&
                mouseState.RightButton == ButtonState.Pressed ||
                mouseState.RightButton == ButtonState.Pressed &&
                elapsedTime > scanInterval)
            {
                rightButton = true;
                resetElapsedTime = true;
            }

            List<Keys> keys = new List<Keys>();

            foreach (KeyValuePair<Keys, bool> kvp in keyStates)
            {
                if (oldKeyboardState.IsKeyUp(kvp.Key) &&
                    keyboardState.IsKeyDown(kvp.Key) ||
                    keyboardState.IsKeyDown(kvp.Key) &&
                    elapsedTime > scanInterval)
                {
                    keys.Add(kvp.Key);
                    resetElapsedTime = true;
                }
            }

            foreach (Keys key in keys)
                keyStates[key] = true;

            if (resetElapsedTime)
                elapsedTime = TimeSpan.Zero;

            if (oldGamePadState != gamePadState || oldMouseState != mouseState ||
                oldKeyboardState != keyboardState)
                elapsedTime = TimeSpan.Zero;
            else if (elapsedTime > scanInterval)
                elapsedTime -= scanInterval;

            oldGamePadState = gamePadState;
            oldKeyboardState = keyboardState;
            oldMouseState = mouseState;
        }

        static public bool IsButtonValid(Buttons button)
        {
            switch (button)
            {
                case Buttons.A:
                    return a;
                case Buttons.B:
                    return b;
                case Buttons.X:
                    return x;
                case Buttons.Y:
                    return y;
                case Buttons.Back:
                    return back;
                case Buttons.Start:
                    return start;
                default:
                    return false;
            }
        }

        static public bool IsKeyValid(Keys key)
        {
            return keyStates[key];
        }

        static public void SetKeyState(Keys key, bool value)
        {
            keyStates[key] = value;
        }
    }
}