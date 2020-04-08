using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class CardSupplementSetter
    {
        public void initSupplements(Card card)
        {
            float w = Properties.globalScale.X + card.properties.scale.X;
            //

            card.suppTextures.supplements[card.suppTextures.portrait].setOffset(20 * w, 100 * w);
            card.suppTextures.supplements[card.suppTextures.cardBorder].setOffset(0 * w, 0 * w);
            card.suppTextures.supplements[card.suppTextures.cardShading].setOffset(0 * w, 0 * w);
            card.suppTextures.supplements[card.suppTextures.cardBody].setOffset(0 * w, 0 * w);
            card.suppTextures.supplements[card.suppTextures.cardBack].setOffset(0 * w, 0 * w);
            card.suppTextures.supplements[card.suppTextures.cardFilling].setOffset(0 * w, 0 * w);
            card.suppTextures.supplements[card.suppTextures.abilityDisplay].setOffset(card.properties.width * w, 0);

            int afterCardImageBorder = (int)(540 * w);
            card.suppTextures.supplements[card.suppTextures.generalSymbol].setOffset(0 * w, afterCardImageBorder);
            card.suppTextures.supplements[card.suppTextures.armySymbol].setOffset(0 * w, afterCardImageBorder);
            card.suppTextures.supplements[card.suppTextures.fieldUnitSymbol].setOffset(0 * w, afterCardImageBorder);
            card.suppTextures.supplements[card.suppTextures.manueverSymbol].setOffset(0 * w, afterCardImageBorder);

            float symbolTransparency = .2f;
            card.suppTextures.supplements[card.suppTextures.generalSymbol].properties.transparency = symbolTransparency;
            card.suppTextures.supplements[card.suppTextures.armySymbol].properties.transparency = symbolTransparency;
            card.suppTextures.supplements[card.suppTextures.fieldUnitSymbol].properties.transparency = symbolTransparency;
            card.suppTextures.supplements[card.suppTextures.manueverSymbol].properties.transparency = symbolTransparency;

            Color symbolColor = Color.Gray;
            card.suppTextures.supplements[card.suppTextures.generalSymbol].properties.color = symbolColor;
            card.suppTextures.supplements[card.suppTextures.armySymbol].properties.color = symbolColor;
            card.suppTextures.supplements[card.suppTextures.fieldUnitSymbol].properties.color = symbolColor;
            card.suppTextures.supplements[card.suppTextures.manueverSymbol].properties.color = symbolColor;


            int attackPosY = (int)(card.properties.height * w - card.suppTextures.supplements[card.suppTextures.attackIcon].getHeight() * 7 / 8);
            int attackPosX = (int)-(card.suppTextures.supplements[card.suppTextures.attackIcon].getWidth() * 3 / 16);

            int defensePosY = (int)(card.properties.height * w - card.suppTextures.supplements[card.suppTextures.attackIcon].getHeight() * 7 / 8);
            int defensePosX = (int)(card.properties.width * w - card.suppTextures.supplements[card.suppTextures.attackIcon].getWidth() * 7 / 8);

            card.suppTextures.supplements[card.suppTextures.attackIcon].setOffset(attackPosX, attackPosY);
            card.suppTextures.supplements[card.suppTextures.defenseIcon].setOffset(defensePosX, defensePosY);


            card.properties.width = card.suppTextures.supplements[card.suppTextures.cardBack].properties.width;
            card.properties.height = card.suppTextures.supplements[card.suppTextures.cardBack].properties.height;
        }
    }
}
