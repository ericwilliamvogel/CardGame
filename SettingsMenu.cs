using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Text.RegularExpressions;

namespace CardGame
{
    public class Setting
    {
        public string key;
        public string value;
    }

    public class SettingsMenu : Menu
    {
        public Dictionary<string, int> settings;
        private FileParser parser;
        string filePath = ".\\test.txt";

        public SettingsMenu()
        {
            initDefaultSettings();
            parseSettingsFile();
        }

        private void initDefaultSettings()
        {
            settings = new Dictionary<string, int>();
            settings.Add("Resolution", 1);
            settings.Add("FullScreen", 1);
            settings.Add("Volume", 50);
        }
        private void parseSettingsFile()
        {
            parser = new FileParser(settings);
            parser.readSettingsFromFile();
        }

        private void adjustFile()
        {
            File.WriteAllText(filePath, string.Empty);
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, int> setting in settings)
            {
                sb.Append(setting.Key);
                sb.Append(';');
                sb.Append(setting.Value.ToString());
                sb.AppendLine();
            }
            string line = sb.ToString();
            File.WriteAllText(filePath, line);
        }

        public override void setButtons(ContentManager content)
        {
            buttons = new List<Button>();
            Button tempButton = new Button(content, new Vector2(0, 0));
            int counter = 0;
            int buttonPositionX = GraphicsSettings.realScreenWidth() / 2 - tempButton.getWidth() / 2;
            int buttonPositionY = counter * tempButton.getHeight() + tempButton.getHeight() / 3;


            switcherButtons.Add(new SwitcherButton(content, new Vector2(buttonPositionX, buttonPositionY), 1));
            switcherButtons[0].setButtonText("TO MAIN");
            counter++;
            buttonPositionY = counter * tempButton.getHeight() + tempButton.getHeight() / 3;
            buttons.Add(new Button(content, new Vector2(buttonPositionX, buttonPositionY)));
            buttons[0].setButtonText("FullScreen TOGGLE");

            counter++;
            buttonPositionY = counter * tempButton.getHeight() + tempButton.getHeight() / 3;
            buttons.Add(new Button(content, new Vector2(buttonPositionX, buttonPositionY)));
            setResolutionText();


            buttonPositionY = counter * tempButton.getHeight() + tempButton.getHeight() / 3;
            buttons.Add(new Button(content, new Vector2(buttonPositionX + 500, buttonPositionY)));
            buttons[2].setButtonText("RESOLUTION");

        }
        public void getPermissionToModifyGraphics(GraphicsDeviceManager graphics)
        {
            buttons[0].setAction(() =>
            {
                toggleFullScreen(graphics);
                graphics.ApplyChanges();
            });
            buttons[1].setAction(() =>
            {
                cycleThroughResolutions();
                setResolutionText();
            });
            buttons[2].setAction(() =>
            {
                adjustFile();

            });
        }

        private void setResolutionText()
        {
            buttons[1].setButtonText(GraphicsSettings.resolutions[GraphicsSettings.currentResolution].X.ToString() + "  " +
                GraphicsSettings.resolutions[GraphicsSettings.currentResolution].Y.ToString());
        }

        private void cycleThroughResolutions()
        {
            GraphicsSettings.currentResolution++;
            if (GraphicsSettings.currentResolution > GraphicsSettings.maxResolutions)
            {
                GraphicsSettings.currentResolution = GraphicsSettings.highestRes;
            }
            settings["Resolution"] = GraphicsSettings.currentResolution;
        }

        private void toggleFullScreen(GraphicsDeviceManager graphics)
        {
            if (graphics.IsFullScreen)
            {

                graphics.IsFullScreen = false;
                settings["FullScreen"] = 0;
                GraphicsSettings.isFullScreen = false;
            }
            else
            {

                graphics.IsFullScreen = true;
                settings["FullScreen"] = 1;
                GraphicsSettings.isFullScreen = true;
            }
            updateAllResolutionValues(graphics);

        }
        private void updateAllResolutionValues(GraphicsDeviceManager graphics)
        {
            if(!graphics.IsFullScreen)
            {
                GraphicsSettings.currentResolution = GraphicsSettings.highestRes;
            }
            else
            {
                GraphicsSettings.currentResolution = settings["Resolution"];
            }

            GraphicsSettings.correctResolutionForMonitor();
            graphics.PreferredBackBufferHeight = (int)GraphicsSettings.resolutions[GraphicsSettings.currentResolution].Y;
            graphics.PreferredBackBufferWidth = (int)GraphicsSettings.resolutions[GraphicsSettings.currentResolution].X;
            Properties.globalScale = GraphicsSettings.trueGameScale(GraphicsSettings.resolutions[GraphicsSettings.currentResolution]);

        }

    }

    
}