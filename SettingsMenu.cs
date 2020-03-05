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
            string imgSrc = "menuButton";
            string smallImgSrc = "regularbutton";
            buttons = new List<Button>();
            Vector2 noVal = new Vector2(0, 0);
            Button tempButton = new Button(content, new Vector2(0, 0), imgSrc);

            switcherButtons.Add(new SwitcherButton(content, noVal, imgSrc, 1));
            
            buttons.Add(new Button(content, noVal,imgSrc));
            
            buttons.Add(new Button(content, noVal, imgSrc));
            
            buttons.Add(new Button(content, noVal,smallImgSrc));
            setButtonPositions();
        }
        public void setButtonPositions()
        {
            Button tempButton = buttons[0];
            int counter = 0;
            int buttonPositionX = GraphicsSettings.realScreenWidth() / 2 - tempButton.getWidth() / 2;
            int buttonPositionY = counter * tempButton.getHeight() + tempButton.getHeight() / 3;
            switcherButtons[0].setPos(new Vector2(buttonPositionX, buttonPositionY));
            switcherButtons[0].setButtonText("TO MAIN");
            counter++;
            buttonPositionY = counter * tempButton.getHeight() + tempButton.getHeight() / 3;
            buttons[0].setButtonText("FullScreen TOGGLE");
            buttons[0].setPos(new Vector2(buttonPositionX, buttonPositionY));
            counter++;
            buttonPositionY = counter * tempButton.getHeight() + tempButton.getHeight() / 3;
            setResolutionText();
            buttons[1].setPos(new Vector2(buttonPositionX, buttonPositionY));

            buttonPositionY = counter * tempButton.getHeight() + tempButton.getHeight() / 3;
            buttons[2].setButtonText("Apply");
            buttons[2].setPos(new Vector2(buttonPositionX + tempButton.getWidth(), buttonPositionY));
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
        int localReso = GraphicsSettings.currentResolution;
        private void setResolutionText()
        {
            buttons[1].setButtonText(GraphicsSettings.resolutions[localReso].X.ToString() + "  " +
                GraphicsSettings.resolutions[localReso].Y.ToString());
        }

        private void cycleThroughResolutions()
        {
            localReso++;
            if (localReso > GraphicsSettings.maxResolutions)
            {
                localReso  = GraphicsSettings.highestRes;
            }
            settings["Resolution"] = localReso;
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
            //updateAllResolutionValues(graphics);
            //graphics.ApplyChanges();
           
        }
        private void updateAllResolutionValues(GraphicsDeviceManager graphics)
        {
            /*if(!graphics.IsFullScreen)
            {
                GraphicsSettings.currentResolution = GraphicsSettings.highestRes;
            }
            else
            {
                GraphicsSettings.currentResolution = settings["Resolution"];
            }*/
            //Properties.globalScale = GraphicsSettings.trueGameScale(GraphicsSettings.resolutions[GraphicsSettings.currentResolution]);
            //GraphicsSettings.correctResolutionForMonitor();
            graphics.PreferredBackBufferHeight = (int)GraphicsSettings.resolutions[GraphicsSettings.currentResolution].Y;
            graphics.PreferredBackBufferWidth = (int)GraphicsSettings.resolutions[GraphicsSettings.currentResolution].X;
            Properties.globalScale = GraphicsSettings.trueGameScale(GraphicsSettings.resolutions[GraphicsSettings.currentResolution]);
            setButtonPositions();
        }

    }

    
}