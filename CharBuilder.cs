
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class CharBuilder
    {
        int totalCharsInMessage;
        int charsInMessageCounter;
        List<char> charsInMessage;
        StringBuilder messageBuilder;
        CustomTimer timer;
        public CharBuilder()
        {
            timer = new CustomTimer(0);
        }
        private void setNewMessage()
        {
            charsInMessage = new List<char>();
            messageBuilder = new StringBuilder();
            totalCharsInMessage = 0;
            charsInMessageCounter = 0;
        }
        private void returnMessageInChars(string message)
        {
            setNewMessage();
            foreach (char c in message)
            {
                totalCharsInMessage++;
                charsInMessage.Add(c);
            }
        }
        bool inputNewString;
        public void resetStringBuilder()
        {
            inputNewString = false;
        }
        public string returnMessageStillBeingBuilt()
        {
            if (messageBuilder != null)
            {
                return messageBuilder.ToString();
            }
            return "";
        }
        public void updateMessage(string message)
        {
            if (!inputNewString)
            {
                returnMessageInChars(message);
                inputNewString = true;
            }
            if (inputNewString)
            {
                timer.timerRun(1);
                if (timer.timerPop() && currentCharsInMessageAreLessThanTotalChars())
                {

                    messageBuilder.Append(charsInMessage[charsInMessageCounter]);
                    charsInMessageCounter++;
                    timer.resetTimer();
                }
            }
        }
        private bool currentCharsInMessageAreLessThanTotalChars()
        {
            if (charsInMessageCounter < totalCharsInMessage)
            {
                return true;
            }
            return false;
        }
    }
    public class CustomTimer
    {
        private float waitTimer;
        private bool trigger;
        private Random random;
        private int range;
        public CustomTimer(int timerVariation)
        {
            range = timerVariation;
            random = new Random();
        }
        public void resetTimer()
        {
            trigger = false;
        }
        public bool timerPop()
        {
            if (waitTimer <= 0 && trigger == true)
            {
                return true;
            }
            return false;
        }
        public void timerRun(int x)
        {
            if (trigger == false)
            {
                setTimer(random.Next(x - range, x + range));
            }

            if (waitTimer > 0)
            {
                waitTimer -= 1;
            }
        }
        private void setTimer(float desiredWaitTime)
        {
            waitTimer = desiredWaitTime;

            trigger = true;
        }
    }
}