﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Utilities;
using static CardGame.BoardFunctionality;

namespace CardGame
{
    public class BoardCardViewer
    {
        public bool createButtonsOnView;
        public List<Button> abilityButtons = new List<Button>();
        public List<Action<MouseState>> selection;

        public void drawSprite(SpriteBatch spriteBatch)
        {
            if (abilityButtons != null)
            {
                foreach (Button button in abilityButtons)
                {
                    button.drawSprite(spriteBatch);
                }
            }

            if(SelectionStillActive())
            spriteBatch.DrawString(Game1.spritefont, selection.Count.ToString(), new Vector2(0, 200), Color.Black);
        }
        public void updateButtonsOnPopup(MouseState mouseState, ContentManager content)
        {
            if (abilityButtons != null)
            {
                foreach (Button button in abilityButtons)
                {
                    button.mouseStateLogic(mouseState, content);
                }
            }
        }
        public void viewFullSizeCard(MouseState mouseState, Card card, BoardFunctionality boardFunc)
        {
            if (boardFunc.state == State.CardView)
            {
                boardFunc.boardPosLogic.scaleToView(card);
                card.setPos(Game1.windowW / 2 - card.getWidth() / 2, Game1.windowH / 2 - card.getHeight() / 2);
                boardFunc.SELECTEDCARD = null;
                boardFunc.SELECTEDCARD = new Card(card);

                if (boardFunc.SELECTEDCARD != null)
                {
                    boardFunc.boardPosLogic.scaleToView(boardFunc.SELECTEDCARD);
                    boardFunc.SELECTEDCARD.setPos(Game1.windowW / 2 - card.getWidth() / 2, Game1.windowH / 2 - card.getHeight() / 2);
                    boardFunc.SELECTEDCARD.updateGameComponent();
                }
                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    resetSelectedCard(boardFunc);
                    card.setRegular();
                    boardFunc.boardPosLogic.updateBoard(boardFunc);
                    boardFunc.state = State.Regular;
                }
            }
        }
        public void viewCardWithAbilities(MouseState mouseState, Card card, BoardFunctionality boardFunc)
        {
            if (boardFunc.state == State.CardView)
            {
                showCardFullSizeCenterScreen(card, boardFunc);
                if (createButtonsOnView == false)
                {
                    showButtonsOnView(mouseState, card, boardFunc);
                }

                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    resetSelectedCard(boardFunc);
                    card.setRegular();
                    boardFunc.boardPosLogic.updateBoard(boardFunc);
                    createButtonsOnView = false;
                    abilityButtons = new List<Button>();
                    boardFunc.state = State.Regular;
                }
            }
        }
        private void showButtonsOnView(MouseState mouseState, Card card, BoardFunctionality boardFunc)
        {
            createButtonsOnView = true;
            int counter = 0;
            for (int i = 0; i < card.cardProps.abilities.Count; i++)
            {
                Vector2 throwAwayLocation = new Vector2(0, 0);
                abilityButtons.Add(new Button(null, throwAwayLocation));
                abilityButtons[i].setTexture(card.suppTextures.supplements[card.suppTextures.abilityDisplay].getTexture());
                abilityButtons[i].setPos(new Vector2(card.getPosition().X + card.getWidth(), card.getPosition().Y + abilityButtons[i].getHeight() * i));
                abilityButtons[i].setButtonText(card.cardProps.abilities[i].description);
                abilityButtons[i].wantedScale = 1f;
                card.cardProps.abilities[i].clickedInAbilityBox = false;

                //THE REASONING BEHIND THIS IS THAT THE I ITERATOR WILL END OUTSIDE OF THE ARRAY, AND EACH TIME THE BUTTONS ARE PRESSED THEY
                //WILL TRIGGER THE FUNCTION AT ITS MAXIMUM

                //IS THERE A BETTER WAY TO DO THIS?
                //PROBABLY
                //BUT ITS OK
                Action<MouseState> action = (MouseState newMouseState) => { };
                if (i == 0)
                {
                    action = (MouseState newMouseState) => {
                        card.cardProps.abilities[0].setTarget();
                        card.cardProps.abilities[0].activateAbilityOnSelection(newMouseState, boardFunc);
                        resetCardSelectionOnRightClick(newMouseState, boardFunc);
                    };
                }
                if (i == 1)
                {
                    action = (MouseState newMouseState) => {
                        card.cardProps.abilities[1].setTarget();
                        card.cardProps.abilities[1].activateAbilityOnSelection(newMouseState, boardFunc);
                        resetCardSelectionOnRightClick(newMouseState, boardFunc);
                    };
                }
                if (i == 2)
                {
                    action = (MouseState newMouseState) => {
                        card.cardProps.abilities[2].setTarget();
                        card.cardProps.abilities[2].activateAbilityOnSelection(newMouseState, boardFunc);
                        resetCardSelectionOnRightClick(newMouseState, boardFunc);
                    };
                }
                if (i == 3)
                {
                    action = (MouseState newMouseState) => {
                        card.cardProps.abilities[3].setTarget();
                        card.cardProps.abilities[3].activateAbilityOnSelection(newMouseState, boardFunc);
                        resetCardSelectionOnRightClick(newMouseState, boardFunc);
                    };
                }
                else
                {
                    Console.WriteLine(card.cardProps.name + " was loaded with more than 4 abilities. Maybe it loaded twice?");
                }
                //THIS ACTUALLY WORKED PERFECTLY IM SO MAD

                abilityButtons[i].setAction(() => {
                    setSelectionState(action, card, boardFunc);
                });
                counter = i;
            }
        }
        public void setSelectionState(Action<MouseState> action, Card card, BoardFunctionality boardFunc)
        {
            if(selection==null)
            {
                selection = new List<Action<MouseState>>();
            }
            MouseTransformer.Set(MouseTransformer.State.Tgt);
            boardFunc.state = State.Selection;
            createButtonsOnView = false;
            selection.Add(action);
            //resetSelectedCard(boardFunc);
            //card.setRegular();
            //boardFunc.boardPosLogic.updateBoard(boardFunc);
            abilityButtons = new List<Button>();
        }
        private void showCardFullSizeCenterScreen(Card card, BoardFunctionality boardFunc)
        {
            boardFunc.boardPosLogic.scaleToView(card);
            card.setPos(Game1.windowW / 2 - card.getWidth() / 2, Game1.windowH / 2 - card.getHeight() / 2);
            boardFunc.SELECTEDCARD = null;
            boardFunc.SELECTEDCARD = new Card(card);
            if (boardFunc.SELECTEDCARD != null)
            {
                boardFunc.boardPosLogic.scaleToView(boardFunc.SELECTEDCARD);
                boardFunc.SELECTEDCARD.setPos(Game1.windowW / 2 - card.getWidth() / 2, Game1.windowH / 2 - card.getHeight() / 2);
                boardFunc.SELECTEDCARD.updateGameComponent();
            }
        }
        public bool SelectionStillActive()
        {
            if(selection != null && selection.Count > 0)
            {
                return true;
            }
            return false;
        }
        public void resetCardSelectionOnRightClick(MouseState mouseState, BoardFunctionality boardFunc)
        {
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                hardResetSelection(boardFunc);
                abilityButtons = new List<Button>();
                createButtonsOnView = false;
                
            }
        }
        public void hardResetSelection(BoardFunctionality boardFunc)
        {
            boardFunc.state = State.Regular;
            selection = new List<Action<MouseState>>();
            resetSelectedCard(boardFunc);
            boardFunc.boardPosLogic.updateBoard(boardFunc);
            MouseTransformer.Set(MouseTransformer.State.Reg);
        }
        public void NextSelection()
        {
            if(SelectionStillActive())
            {
                selection.Remove(selection[0]);
            }

        }
        public void resetSelectedCard(BoardFunctionality boardFunc)
        {

            if (boardFunc.SELECTEDCARD != null)
            {
                boardFunc.SELECTEDCARD.setRegular();
                boardFunc.SELECTEDCARD.resetCardSelector();
            }

            boardFunc.SELECTEDCARD = null;

        }

    }
}
