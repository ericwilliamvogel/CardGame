using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public static class VolumeSettings
    {

    }
    public static class GraphicsSettings
    {
        public static List<Vector2> resolutions;
        public static int currentResolution;
        public static int maxResolutions;
        public static Vector2 maxRes;
        public static int highestRes;
        public static bool isFullScreen;
        static GraphicsSettings()
        {
            resolutions = new List<Vector2>();
            addNewReso(1920, 1080);
            addNewReso(1600, 900);
            addNewReso(1366, 768);
            addNewReso(1280, 720);
            maxRes = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        }
        private static void addNewReso(int x, int y)
        {
            resolutions.Add(new Vector2(x, y));
            maxResolutions++;
        }
        private static void setLargestResolution()
        {
            for(int i = 0; i < maxResolutions; i++) 
            {
                if(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width <= resolutions[i].X)
                {
                    highestRes = i;
                }
            }
        }
        public static void correctResolutionForMonitor()
        {
            setLargestResolution();
            //currentResolution = highestRes;
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

            float currentResX = resolutions[currentResolution].X;
            float currentResY = resolutions[currentResolution].Y;

            Vector2 newScale = new Vector2(value.X * currentResX / maxResX , value.Y * currentResY / maxResY);
            return newScale;
        }


        public static int toResolution(int value)
        {
            float maxResX = resolutions[0].X;
            float currentResX = resolutions[currentResolution].X;

            float resolutionTrim = currentResX / maxResX;

            float returnValue = value * resolutionTrim;

            return (int)returnValue;
        }
        public static Vector2 toLocalResolution(Vector2 value)
        {
            float maxResX = resolutions[highestRes].X;
            float maxResY = resolutions[highestRes].Y;

            float currentResX = resolutions[currentResolution].X;
            float currentResY = resolutions[currentResolution].Y;

            Vector2 newScale = new Vector2(value.X * currentResX / maxResX, value.Y * currentResY / maxResY);
            return newScale;
        }
        public static int toLocalResolution(int value)
        {
            float maxResX = resolutions[highestRes].X;
            float currentResX = resolutions[currentResolution].X;

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
            return Game1.windowW;
        }
        public static int realScreenHeight()
        {
            return Game1.windowH;
        }

    }
}
