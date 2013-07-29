using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyLife
{
    static class GlobalVariables
    {
        static public Game Game;

        const float ScreenCoefficient = 1.5f;

        static public Finance Finance;

        static public DateTime Date;

#if WINDOWS_PHONE
        static public int ScreenWidth = 800;
        static public int ScreenHeight = 480;
#endif

#if WINDOWS
        static public int ScreenWidth = (int) (800 * ScreenCoefficient);
        static public int ScreenHeight = (int) (480 * ScreenCoefficient);
#endif
    }

}
