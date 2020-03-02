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
using CardGame;

namespace CardGame
{
    public class ComponentManager : PrimaryComponent
    {
        private PrimaryComponent[] primaryComponent;

        public enum PrimaryComponentType
        {
            LoadScreen,
            Menu,
            Game,
            Map,
            Settings,
            Story
        }

        private int controller = 1;
        private Transition fadeScreen;
        private int currentComponent;
        public static int LEVEL = 0;
        private int controllerSwitchToBeCompleted;
        private Menu menu = new Menu();
        private Menu othermenu = new Menu();
        private StorySlides storySlides;
        private Action<SwitcherButton> action;

        public void switchComponent(SwitcherButton button)
        {
            if (fadeScreen.transitionComplete())
            {
                switchPrimaryComponent(button.returnComponentToSwitchTo());
            }
        }

        public void assignExitLogic(Game1 game)
        {
            menu.closeWindowLogic(game);
        }

        public ComponentManager(ContentManager content)
        {
            primaryComponent = new PrimaryComponent[2];
            primaryComponent[0] = othermenu;
            primaryComponent[1] = menu;
            action = switchComponent;
            Properties.globalScale = Resolution.trueGameScale(Resolution.resolutions[1]);
        }



        public void Controls(ContentManager content)
        {
            if (fadeScreen.transitionPauseForContent())
            {
                completeComponentSwitch(controllerSwitchToBeCompleted, content);
                fadeScreen.processContentAndResume();
            }
        }


        public void switchPrimaryComponent(int toBeSelected)
        {
            controllerSwitchToBeCompleted = toBeSelected;
            currentComponent = controller;
            fadeScreen.startTransition();
        }

        public void completeComponentSwitch(int toBeSelected, ContentManager content)
        {

            primaryComponent[currentComponent].unloadGameComponent();

            primaryComponent[toBeSelected].initializeGameComponent(content);

            initSwitcherButtons(toBeSelected);

            controller = toBeSelected;

        }

        public override void initializeGameComponent(ContentManager content)
        {
            fadeScreen = new Transition(content);
            primaryComponent[controller].initializeGameComponent(content);
            initSwitcherButtons(controller);

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



        private void initSwitcherButtons(int toBeSel) //call this when initializing new component
        {
            foreach(SwitcherButton switcher in primaryComponent[toBeSel].switcherButtons)
            {
                switcher.setSwitcherAction(action);
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