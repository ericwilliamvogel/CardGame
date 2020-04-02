using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace CardGame
{
    public class MoveHistory : GameComponent
    {
        public class AttackIcon : GameComponent
        {
            public static Texture2D attackIcon;
        }
        public class Move : CardContainer
        {
            public Card fromCard;
            public Card toCard;
        }
        public class Section : GameComponent
        {
            Move move;
            AttackIcon attackIcon;
            public Section(Move move)
            {
                this.move = move;
                attackIcon = new AttackIcon();
                attackIcon.setTexture(AttackIcon.attackIcon);
                //attackIcon.setScale(CardScale.Hand);
            }
            public override void drawSprite(SpriteBatch spriteBatch)
            {
                move.fromCard.drawSprite(spriteBatch);
                move.toCard.drawSprite(spriteBatch);
                attackIcon.drawSprite(spriteBatch);
            }
            public override void updateGameComponent()
            {
                move.fromCard.setPos(getPosition());
                move.toCard.setPos((int)getPosition().X + move.toCard.getWidth(), (int)getPosition().Y);
                attackIcon.setPos((int)getPosition().X + move.toCard.getWidth() - attackIcon.getWidth() / 2, (int)getPosition().Y + move.fromCard.getHeight()/2 - attackIcon.getHeight()/2);
                move.fromCard.updateGameComponent();
                move.toCard.updateGameComponent();
            }
        }

        public List<List<Section>> historicalTurns = new List<List<Section>>();
        public List<Section> turns = new List<Section>();
        public void AddNewAttackMove(Card fromCard, Card toCard)
        {
            //add an attack symbol
            Move move = new Move();
            move.fromCard = fromCard;
            move.toCard = toCard;
            turns.Add(new Section(move));
        }
        public void AddNewAbilityMove(Card fromCard, Card toCard)
        {

        }
        public void AddNewAbilityMove(Card fromCard)
        {

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
