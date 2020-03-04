using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CardGame
{
    public class FileParser
    {
        Dictionary<string, int> settings;
        string filePath = ".\\test.txt";
        public FileParser(Dictionary<string, int> settings)
        {
            this.settings = settings;
        }
        public void readSettingsFromFile()
        {

            string[] lines = File.ReadAllLines(filePath);
            if (lines == null)
            {
                throw new Exception();
            }

            foreach (string line in lines)
            {
                processString(line);
            }

        }




        private void processString(string input)
        {
            StringBuilder id = new StringBuilder();
            StringBuilder val = new StringBuilder();
            Setting setting = new Setting();

            int charCounter = 0;
            for (int i = 0; input[i] != ';'; i++)
            {
                if (!charIsSpace(input[i]))
                    id.Append(input[i]);
                charCounter = i;
            }

            int spaceBetweenKeyAndValue = 2;
            for (int i = charCounter + spaceBetweenKeyAndValue; i < input.Length; i++)
            {
                if (!charIsSpace(input[i]))
                    val.Append(input[i]);
            }
            setting.key = id.ToString();
            setting.value = val.ToString();

            if (settingIsFound(settings, setting))
                parseAndStoreLine(setting.key, setting.value);
        }
        private bool charIsSpace(char input)
        {
            if (input == ' ')
            {
                return true;
            }
            return false;
        }

        private bool settingIsFound(Dictionary<string, int> settings, Setting setting)
        {
            if (settings.ContainsKey(setting.key))
            {
                return true;
            }
            return false;
        }

        private void parseAndStoreLine(string key, string value)
        {
            try
            {
                if (value != null || value != "")
                {

                    if (isInt(value))
                    {
                        settings[key] = storeIntFromString(value);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private bool isBool(string input)
        {
            if (input == "true" || input == "false")
            {
                return true;
            }
            return false;
        }
        private object storeBool(string input)
        {
            if (input == "true")
            {
                return true;
            }
            else if (input == "false")
            {
                return false;
            }

            return null;
        }
        private bool isInt(string input)
        {
            if (Regex.IsMatch(input, @"^\d+$"))
            {
                return true;
            }
            return false;
        }
        private int storeIntFromString(string input)
        {
            var value = int.TryParse(input, out int n);
            return n;
        }
        private string storeStringFromInt(int input)
        {
            string val = input.ToString();
            return val;
        }
    }
}
