using System;
using System.Collections;
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
using SiegewithCleanCode;

namespace SiegewithCleanCode
{
    public class ComponentManager : PrimaryComponent
    {
        private PrimaryComponent[] primaryComponent;
        private int controller = 2;
        private int loadScreen = 0;
        private Dictionary<int, int> transitioner;
        private Transition fadeScreen;
        private bool upKeyBool = false;
        private int currentComponent;
        public static int LEVEL = 0;


        public ComponentManager(ContentManager content)
        {
            primaryComponent = new PrimaryComponent[0];
            //primaryComponent[0] = 
        }


        private int controllerSwitchToBeCompleted;
        public void Controls(ContentManager content)
        {
            if (Keyboard.GetState().IsKeyUp(Keys.Escape) && upKeyBool == true)
            {
                upKeyBool = false;

            }

            if (fadeScreen.transitionPauseForContent())
            {
                completeComponentSwitch(controllerSwitchToBeCompleted, content);
                fadeScreen.processContentAndResume();
            }

        }


        public void switchPrimaryComponent(int toBeSelected)
        {
            controllerSwitchToBeCompleted = toBeSelected;
            if (controller != loadScreen)
                currentComponent = controller;
            //primaryComponent[loadScreen].initializeGameComponent(content); unneccessary!
            controller = loadScreen
            fadeScreen.startTransition();

        }

        public void completeComponentSwitch(int toBeSelected, ContentManager content)
        {
            /*if (toBeSelected == 1)
            {
                //battlefield.attackingArmy.assembler.soldiersInArmy.AddRange(armyMenu.setArmyConfig());
                battlefield.attackingArmy = armyMenu.setArmyConfig();
                battlefield.defendingArmy = armyBuilder.getArmy(LEVEL);
            }
            if (toBeSelected == 3)
            {
                if (mapPicker.returnSelectedLevel() != -1)
                    LEVEL = mapPicker.returnSelectedLevel();
            }
            if (toBeSelected == 5)
            {
                assembleStory();
            }*/
            primaryComponent[currentComponent].unloadGameComponent();

            primaryComponent[toBeSelected].initializeGameComponent(content);

            controller = toBeSelected;

        }

        public override void initializeGameComponent(ContentManager content)
        {
            fadeScreen = new Transition(content);
            primaryComponent[controller].initializeGameComponent(content);


        }

        public override void unloadGameComponent()
        {
            primaryComponent[controller].unloadGameComponent();

        }

        public void optionToCloseWindow(Game1 game)
        {
            if (controller == 2)
            {
                //menu.closeWindowLogic(game);
            }

        }
        public override void updateGameComponent(ContentManager content)
        {
            primaryComponent[controller].updateGameComponent(content);
            Controls(content);

            fadeScreen.Update();

            checkSwitcherButtons(content);

            /*if (controller == 2)
            {
                menu.handleButtons();

            }

            if (controller == 3)
            {
                armyMenu.handleButtons(content);
            }*/
            updateSwitcherButtons();
        }

        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            primaryComponent[controller].mouseStateLogic(mouseState, content);
            updateSwitcherMouseLogic(mouseState);
        }

        public override void drawSprite(SpriteBatch spriteBatch)
        {
            primaryComponent[controller].drawSprite(spriteBatch);
            drawSwitcherButtons(spriteBatch);
            if (fadeScreen.blackScreen.getLoadedTexture() != null)
                fadeScreen.Draw(spriteBatch);


        }
        private Action<SwitcherButton> action;

        private void checkSwitcherButtons(ContentManager content)
        {
            foreach (SwitcherButton button in primaryComponent[controller].switcherButtons)
            {
                if (button.isPressed() && button.conditionsAreSatisfied() && upKeyBool == false && fadeScreen.transitionComplete())
                {
                    action = switchComponent;
                    switchPrimaryComponent(button.returnComponentToSwitchTo());
                }
            }
        }


        public void switchComponent(SwitcherButton button)
        {
                if (fadeScreen.transitionComplete())
                {
                    switchPrimaryComponent(button.returnComponentToSwitchTo());
                }
        }

        private void drawSwitcherButtons(SpriteBatch spriteBatch)
        {
            foreach (SwitcherButton switcher in primaryComponent[controller].switcherButtons)
            {
                switcher.drawSprite(spriteBatch);
            }
        }

        private void updateSwitcherButtons()
        {
            foreach (SwitcherButton switcher in primaryComponent[controller].switcherButtons)
            {
                switcher.updateGameComponent();
            }
        }

        private void updateSwitcherMouseLogic(MouseState mouseState)
        {
            foreach (SwitcherButton switcher in primaryComponent[controller].switcherButtons)
            {
                switcher.Update(mouseState);
            }
        }





        private void assembleStory()
        {
            StorySlideConstructor storySlideConstructor = new StorySlideConstructor();
            StorySlideAssembly storySlideAssembly = new StorySlideAssembly();

            storySlideAssembly.assembleStorySlide(storySlideConstructor, LEVEL);
            storySlides = storySlideConstructor.getSlides();

            primaryComponent[5] = storySlides;
        }
    }
}