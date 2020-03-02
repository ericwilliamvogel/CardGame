using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public static class Resolution
    {
        public static List<Vector2> resolutions;
        static Resolution()
        {
            resolutions = new List<Vector2>();
            resolutions.Add(new Vector2(1920, 1080));
            resolutions.Add(new Vector2(1600, 900));
            resolutions.Add(new Vector2(1366, 768));
            resolutions.Add(new Vector2(1280, 720));
        }
        public static Vector2 trueGameScale(Vector2 resolution)
        {
            float maxResX = resolutions[0].X;
            float maxResY = resolutions[0].Y;

            float currentResX = resolution.X;
            float currentResY = resolution.Y;

            Vector2 newScale = new Vector2(currentResX / maxResX, currentResY / maxResY);
            return newScale;
        }

        public static Vector2 toResolution(Vector2 value)
        {
            float maxResX = resolutions[0].X;
            float maxResY = resolutions[0].Y;

            float currentResX = resolutions[1].X;
            float currentResY = resolutions[1].Y;

            Vector2 newScale = new Vector2(value.X * currentResX / maxResX , value.Y * currentResY / maxResY);
            return newScale;
        }
        public static int toResolution(int value)
        {
            float maxResX = resolutions[0].X;
            float currentResX = resolutions[1].X;

            float resolutionTrim = currentResX / maxResX;

            float returnValue = value * resolutionTrim;

            return (int)returnValue;
        }

        private static float fitResoToScreen(float val)
        {
            return val / 2;
        }
        private static int fitResoToScreen(int val)
        {
            return val / 2;
        }
        public static int realScreenWidth()
        {
            return toResolution(Game1.windowW);
        }
        public static int realScreenHeight()
        {
            return toResolution(Game1.windowH);
        }

    }
}
