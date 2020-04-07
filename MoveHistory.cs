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
            public static Texture2D targetIcon;
        }
        public class CastIcon : GameComponent
        {
            public static Texture2D castIcon;
        }
        public class PlayIcon : GameComponent
        {
            public static Texture2D playIcon;
        }
        public class Move : CardContainer
        {
            public Card fromCard;
            public Ability ability;
            public Card toCard;
        }
        public class Section : GameComponent
        {
            public Move move;
            AttackIcon attackIcon;
            CastIcon castIcon;
            PlayIcon playIcon;
            State state;
            TargetIcon targetIcon;

            public enum State
            {
                Attack,
                TargetAbility,
                SoloAbility,
                PlayCard
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
                        castIcon = new CastIcon();
                        castIcon.setTexture(CastIcon.castIcon);
                        break;
                    case State.TargetAbility:
                        targetIcon = new TargetIcon();
                        targetIcon.setTexture(TargetIcon.targetIcon);
                        break;
                    case State.PlayCard:
                        playIcon = new PlayIcon();
                        playIcon.setTexture(PlayIcon.playIcon);
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
                        if (move.fromCard.playState != Card.PlayState.Hidden && move.fromCard.cardProps.type != CardType.Manuever)
                        {
                            spriteBatch.DrawString(Game1.spritefont, move.ability.exchangeValue.ToString(), new Vector2(move.fromCard.getPosition().X + move.fromCard.getWidth() / 2 - adjustToCenter, move.fromCard.getPosition().Y + move.fromCard.getHeight() / 2 - adjustToCenter), Color.White, 0, new Vector2(0, 0), getScale(), SpriteEffects.None, 0);
                        }
                        castIcon.drawSprite(spriteBatch);
                        break;
                    case State.TargetAbility:

                        move.toCard.drawSprite(spriteBatch);
                        if(move.fromCard.cardProps.type != CardType.Manuever)
                        spriteBatch.DrawString(Game1.spritefont, move.ability.exchangeValue.ToString(), new Vector2(move.fromCard.getPosition().X + move.fromCard.getWidth() / 2 - adjustToCenter, move.fromCard.getPosition().Y + move.fromCard.getHeight() / 2 - adjustToCenter), Color.White, 0, new Vector2(0, 0), getScale(), SpriteEffects.None, 0);
                        targetIcon.drawSprite(spriteBatch);
                        break;
                    case State.PlayCard:
                        playIcon.drawSprite(spriteBatch);
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
                        //if(move.fromCard.playState != Card.PlayState.Hidden)
                        castIcon.setPos((int)move.fromCard.getPosition().X + move.fromCard.getWidth() - castIcon.getWidth() / 2, (int)move.fromCard.getPosition().Y + move.fromCard.getHeight() / 2 - castIcon.getHeight() / 2);

                        break;
                    case State.TargetAbility:
                        move.toCard.setPos((int)getPosition().X + move.toCard.getWidth(), (int)getPosition().Y);
                        move.toCard.updateGameComponent();
                        move.fromCard.setPos(getPosition());
                        targetIcon.setPos((int)getPosition().X + move.toCard.getWidth() - targetIcon.getWidth() / 2, (int)getPosition().Y + move.fromCard.getHeight() / 2 - targetIcon.getHeight() / 2);
                        break;
                    case State.PlayCard:
                        move.fromCard.setPos((int)(getPosition().X + move.fromCard.getWidth() / 2), (int)getPosition().Y);
                        playIcon.setPos((int)move.fromCard.getPosition().X + move.fromCard.getWidth() - playIcon.getWidth() / 2, (int)move.fromCard.getPosition().Y + move.fromCard.getHeight() / 2 - playIcon.getHeight() / 2);
                        break;
                }

                move.fromCard.updateGameComponent();

            }
        }
        public enum Type
        {
            Current,
            Previous
        }
        public Type type = Type.Current;
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
            amountOfTurns++;
            historicalTurns.Add(turns);
            turns = new List<Section>();
        }
        int amountOfTurns = 0;
        int selector = 0;
        int advancer = 0;
        public override void drawSprite(SpriteBatch spriteBatch)
        {
            int counter = 0;
            for (int i = advancer; i < advancer + correctIterationDown(upDownIterator); i++)
            {
                getCurrentList()[i].drawSprite(spriteBatch);
                counter++;
            }
            if(counter ==0 )
            {
                int offset = GraphicsSettings.toResolution(20);
                spriteBatch.DrawString(Game1.spritefont, "0 moves", new Vector2(getPosition().X + offset, getPosition().Y + offset), Color.Black);
            }

            if(type == Type.Current)
            {
                int offset = GraphicsSettings.toResolution(20);
                spriteBatch.DrawString(Game1.spritefont, "Current turn", new Vector2(getPosition().X, getPosition().Y - offset*5), Color.Black * .5f);
            }
            else
            {

                string whoseTurn = "Kappa";
                if (selector == 0)
                {
                    whoseTurn = "My turn";
                }
                if (selector % 2 == 0)
                {
                    whoseTurn = "My turn";
                }
                else
                {
                    whoseTurn = "Enemy turn";
                }
                int offset = GraphicsSettings.toResolution(20);
                spriteBatch.DrawString(Game1.spritefont, whoseTurn, new Vector2(getPosition().X, getPosition().Y - offset * 5), Color.Black* .5f);
            }


        }
        private int IterateSectionsDown(int input, List<Section> list)
        {
            if (advancer + input > list.Count )
            {
                return correctIterationDown(input - 1);
            }
            else
            {
                return input;
            }
        }
        private int IterateSectionsUp(int input, List<Section> list)
        {
            if (advancer - input < 0)
            {
                return correctIterationUp(input - 1);
            }
            else
            {
                return input;
            }
        }
        private int correctIterationUp(int input)
        {
            return IterateSectionsUp(input, getCurrentList());
        }
        private int correctIterationDown(int input)
        {
            return IterateSectionsDown(input, getCurrentList());
        }
        private List<Section> getCurrentList()
        {
            if (type == Type.Previous)
            {
                return historicalTurns[selector];
            }
            else
            {
                return turns;
            }
        }
        public int upDownIterator = 4;
        public void NextTurn()
        {
            advancer = 0;

            if(type == Type.Current)
            {
                //can't go into future
            }
            else if (selector + 1 <= historicalTurns.Count - 1)
            {
                selector += 1;
            }
            else
                type = Type.Current;


        }
        public void PreviousTurn()
        {
            advancer = 0;
            if(type == Type.Current && historicalTurns.Count > 0)
            {
                type = Type.Previous;
                selector = historicalTurns.Count - 1;
            }
            else
            {
                if(selector - 1 >= 0)
                {
                    selector -= 1;
                }
            }
        }
        public void ScrollDown()
        {
            if(advancer < getCurrentList().Count - upDownIterator)
            {
                advancer += correctIterationDown(upDownIterator);
            }

        }
        public void ScrollUp()
        {
            if (advancer >= 0)
            {
                advancer -= correctIterationUp(upDownIterator);
            }
        }
        public override void setPos(int x, int y)
        {
            base.setPos(x, y);
            if(type == Type.Current)
            {
                adjustPositionsInCollection(turns);
            }
            else
            {
                if (historicalTurns.Count > 1)
                {
                    adjustPositionsInCollection(historicalTurns[selector]);
                }
            }


        }
        public override void updateGameComponent()
        {
            if (type == Type.Current)
            {
                adjustPositionsInCollection(turns);
            }
            else
            {
                if (historicalTurns.Count > 1)
                {
                    adjustPositionsInCollection(historicalTurns[selector]);
                }
            }
        }

        public void adjustPositionsInCollection(List<Section> turns)
        {
            int counter = 0;
            int spacing = GraphicsSettings.toResolution(150);
            int xOffset = GraphicsSettings.toResolution(25);
            foreach (Section section in turns)
            {
                section.setPos((int)getPosition().X + xOffset, (int)getPosition().Y + counter * spacing);
                section.updateGameComponent();
                counter++;
                if (counter > 3)
                {
                    counter = 0;
                }
            }
        }
    }
}
