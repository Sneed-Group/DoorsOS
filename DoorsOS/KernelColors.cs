using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorsOS
{
    internal class KernelColors
    {
        //private copy for getColor
        public static int Black = 0;
        public static int DarkBlue = 1;
        public static int DarkGreen = 2;
        public static int DarkCyan = 3;
        public static int DarkRed = 4;
        public static int DarkMagenta = 5;
        public static int DarkYellow = 6;
        public static int Gray = 7;
        public static int DarkGray = 8;
        public static int Blue = 9;
        public static int Green = 10;
        public static int Cyan = 11;
        public static int Red = 12;
        public static int Magenta = 13;
        public static int Yellow = 14;
        public static int White = 15;
        public struct colors
        {
            public static int Black = 0;
            public static int DarkBlue = 1;
            public static int DarkGreen = 2;
            public static int DarkCyan = 3;
            public static int DarkRed = 4;
            public static int DarkMagenta = 5;
            public static int DarkYellow = 6;
            public static int Gray = 7;
            public static int DarkGray = 8;
            public static int Blue = 9;
            public static int Green = 10;
            public static int Cyan = 11;
            public static int Red = 12;
            public static int Magenta = 13;
            public static int Yellow = 14;
            public static int White = 15;
        }

        public static int getColor(string colorName)
        {
            try
            {
                Type classType = typeof(KernelColors);
                object obj = Activator.CreateInstance(classType);
                int color = (int)classType.GetField(colorName).GetValue(obj);
                return color;
            }
            catch
            {
                return 1;
            }
        }
    }
}
