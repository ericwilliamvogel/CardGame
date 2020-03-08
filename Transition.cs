using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CardGame;

namespace CardGame
{
    public class Transition : GameComponent
    {
        public Sprite blackScreen;
        private bool start;
        private bool finish;
        private float fader = 0f;
        private bool process = false;

        public Transition(ContentManager content)
        {

            blackScreen = new Sprite(content, "blackscreen");
        }

        public override void updateGameComponent()
        {

            runTransition();

        }

        public override void drawSprite(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(blackScreen.getLoadedTexture(), new Vector2(0, 0), Color.White * fader);

        }

        public bool fadingOut()
        {
            if (start == true && fader < 1f)
            {
                return true;
            }

            return false;
        }

        public bool fadingIn()
        {
            if (finish == true && fader < 1f)
            {
                return true;
            }

            return false;
        }

        public bool transitionComplete()
        {
            if (finish == false && start == false && fader <= 0f)
            {
                return true;
            }
            return false;
        }

        public void startTransition()
        {
            start = true;
        }

        public void runTransition()
        {
            runStart();
            runFinish();
        }

        private void runStart()
        {
            if (start == true)
            {
                fader += .08f;
                if (fader >= 1f)
                {
                    start = false;
                    finish = true;
                    process = true;
                }
            }
        }


        public void processContentAndResume()
        {
            process = false;
        }

        public bool transitionPauseForContent()
        {
            if (process == true)
            {
                return true;
            }

            return false;
        }
        private void runFinish()
        {
            if (finish == true && process == false)
            {

                fader -= .08f;
                if (fader <= 0f)
                {
                    finish = false;
                }
            }
        }
    }
}
