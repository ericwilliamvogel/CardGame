using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CardGame
{
    public class MoveHistory : GameComponent
    {
        public class AttackIcon : GameComponent
        {
            public static Texture2D attackIcon;
        }
        public class TargetIcon : GameComponent
        {
            public static Texture2D buttonIcon;
        }
        public class Move : CardContainer
        {
            public Card fromCard;
            public Ability ability;
            public Card toCard;
        }
        public class Section : GameComponent
        {
            Move move;
            AttackIcon attackIcon;
            State state;
            TargetIcon targetIcon;

            public enum State
            {
                Attack,
                TargetAbility,
                SoloAbility
            };

            public Section(Move move, State state)
            {
                this.move = move;
                switch (state)
                {
                    case State.Attack:
                        attackIcon = new AttackIcon();
                        attackIcon.setTexture(AttackIcon.attackIcon);
                        break;
                    case State.SoloAbility:
                        if(move.fromCard.playState!=Card.PlayState.Hidden)
                        {
                            targetIcon = new TargetIcon();
                            targetIcon.setTexture(TargetIcon.buttonIcon);
                        }

                        break;
                    case State.TargetAbility:
                        targetIcon = new TargetIcon();
                        targetIcon.setTexture(TargetIcon.buttonIcon);
                        break;
                }



                this.state = state;
                //attackIcon.setScale(CardScale.Hand);
            }
            public override void drawSprite(SpriteBatch spriteBatch)
            {
                int adjustToCenter = GraphicsSettings.toResolution(15);
                move.fromCard.drawSprite(spriteBatch);
                switch (state)
                {
                    case State.Attack:
                        move.toCard.drawSprite(spriteBatch);
                        attackIcon.drawSprite(spriteBatch);
                        break;
                    case State.SoloAbility:
                        if (move.fromCard.playState != Card.PlayState.Hidden)
                        {
                            spriteBatch.DrawString(Game1.spritefont, move.ability.exchangeValue.ToString(), new Vector2(move.fromCard.getPosition().X + move.fromCard.getWidth() / 2 - adjustToCenter, move.fromCard.getPosition().Y + move.fromCard.getHeight() / 2 - adjustToCenter), Color.White, 0, new Vector2(0, 0), getScale(), SpriteEffects.None, 0);
                        }
                        break;
                    case State.TargetAbility:

                        move.toCard.drawSprite(spriteBatch);
                        spriteBatch.DrawString(Game1.spritefont, move.ability.exchangeValue.ToString(), new Vector2(move.fromCard.getPosition().X + move.fromCard.getWidth() / 2 - adjustToCenter, move.fromCard.getPosition().Y + move.fromCard.getHeight() / 2 - adjustToCenter), Color.White, 0, new Vector2(0, 0), getScale(), SpriteEffects.None, 0);
                        targetIcon.drawSprite(spriteBatch);
                        break;
                }
            }
            public override void updateGameComponent()
            {


                
                switch(state)
                {
                    case State.Attack:
                        attackIcon.setPos((int)getPosition().X + move.toCard.getWidth() - attackIcon.getWidth() / 2, (int)getPosition().Y + move.fromCard.getHeight() / 2 - attackIcon.getHeight() / 2);
                        move.toCard.setPos((int)getPosition().X + move.toCard.getWidth(), (int)getPosition().Y);
                        move.toCard.updateGameComponent();
                        move.fromCard.setPos(getPosition());
                        break;
                    case State.SoloAbility:
                        move.fromCard.setPos((int)(getPosition().X + move.fromCard.getWidth() / 2), (int)getPosition().Y);
                        if(move.fromCard.playState != Card.PlayState.Hidden)
                        targetIcon.setPos((int)getPosition().X + move.fromCard.getWidth() - targetIcon.getWidth() / 2, (int)getPosition().Y + move.fromCard.getHeight() / 2 - targetIcon.getHeight() / 2);

                        break;
                    case State.TargetAbility:
                        //attackIcon.setPos((int)getPosition().X + move.toCard.getWidth() - attackIcon.getWidth() / 2, (int)getPosition().Y + move.fromCard.getHeight() / 2 - attackIcon.getHeight() / 2);
                        move.toCard.setPos((int)getPosition().X + move.toCard.getWidth(), (int)getPosition().Y);
                        move.toCard.updateGameComponent();
                        move.fromCard.setPos(getPosition());

                        targetIcon.setPos((int)getPosition().X + move.toCard.getWidth() - targetIcon.getWidth() / 2, (int)getPosition().Y + move.fromCard.getHeight() / 2 - targetIcon.getHeight() / 2);
                        break;

                }

                move.fromCard.updateGameComponent();

            }
        }

        public List<List<Section>> historicalTurns = new List<List<Section>>();
        public List<Section> turns = new List<Section>();
        public void AddHiddenMove(Card fromCard)
        {
            Move move = setMove(fromCard);
            move.fromCard.playState = Card.PlayState.Hidden;
            turns.Add(new Section(move, Section.State.SoloAbility));
        }
        public void AddNewAttackMove(Card fromCard, Card toCard)
        {
            //add an attack symbol
            Move move = setMove(fromCard, toCard);
            turns.Add(new Section(move, Section.State.Attack));
        }
        public void AddTargetAbilityMove(Card fromCard, Ability ability, Card toCard)
        {
            Move move = setMove(fromCard, ability, toCard);
            turns.Add(new Section(move, Section.State.TargetAbility));
        }
        public void AddSoloAbilityMove(Card fromCard, Ability ability)
        {
            Move move = setMove(fromCard, ability);
            turns.Add(new Section(move, Section.State.SoloAbility));
        }
        private Move setMove(Card fromCard, Ability ability, Card toCard)
        {
            Move move = new Move();
            move.ability = ability;
            move.fromCard = fromCard;
            move.toCard = toCard;
            return move;
        }
        private Move setMove(Card fromCard, Ability ability)
        {
            Move move = new Move();
            move.ability = ability;
            move.fromCard = fromCard;
            return move;
        }
        private Move setMove(Card fromCard, Card toCard)
        {
            Move move = new Move();
            move.fromCard = fromCard;
            move.toCard = toCard;
            return move;
        }
        private Move setMove(Card fromCard)
        {
            Move move = new Move();
            move.fromCard = fromCard;
            return move;
        }
        public void storeTurnAndReset()
        {
            historicalTurns.Add(turns);
            turns = new List<Section>();
        }
        public override void drawSprite(SpriteBatch spriteBatch)
        {
            foreach(Section section in turns)
            {
                section.drawSprite(spriteBatch);
            }
        }
        public override void updateGameComponent()
        {
            
            setPos(GraphicsSettings.toResolution(1700), 30);
            int counter = 0;
            int spacing = GraphicsSettings.toResolution(150);
            foreach (Section section in turns)
            {
                section.updateGameComponent();
                section.setPos((int)getPosition().X, counter * spacing);
                counter++;
            }
        }
    }
}
