using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Common
{
    public static class Colors
    {
        public static Vector4 GreenBright => new Vector4(0, 1, 0, 1);
        public static Vector4 GreenHalf => new Vector4(0, 0.5f, 0, 1);
        public static Vector4 RedBright => new Vector4(1, 0, 0, 1);

        public static Vector4 RedHalf => new Vector4(0.5f, 0, 0, 1);
        public static Vector4 BlueBright => new Vector4(0, 0, 1, 1);
        public static Vector4 BlueHalf => new Vector4(0, 0, 0.5f, 1);

        public static Vector4 White => new Vector4(1, 1, 1, 1);
        public static Vector4 Black => new Vector4(0, 0, 0, 0);
        public static Vector4 GreyDark => new Vector4(0.25f, 0.25f, 0.25f, 1);

        public static Vector4 GreyHalf => new Vector4(0.5f, 0.5f, 0.5f, 1);

        public static Vector4 GreyBright => new Vector4(0.75f, 0.75f, 0.75f, 1);

    }
}
