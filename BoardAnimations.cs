using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class AnimationAction
    {
        public Action action;
        public Vector2 startingPosition;
    }
    public class PlayerAnimations
    {
        public AnimationAction drawCard;
        public AnimationAction playFieldUnit;
        public AnimationAction playCommander;
        public AnimationAction playArmy;
        public AnimationAction playStrategy;

    }
    public class BoardAnimations
    {
        bool starter = false;
        PlayerAnimations enemyBoard;
        PlayerAnimations friendlyBoard;
        private void initializeActions(SpriteBatch spriteBatch)
        {
            friendlyBoard.drawCard.startingPosition = new Vector2(/*deck.pos.x, deck.pos.y*/);
            friendlyBoard.drawCard.action = () => {
                friendlyBoard.drawCard.startingPosition.X--;
                friendlyBoard.drawCard.startingPosition.Y--;
                /*spriteBatch.Draw(); */
                //card back
                //position--;
                //when pos pass
                //player1.drawCard();
                //deleteAction;
            };
        }
    }
}
