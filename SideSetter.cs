using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class SideSetter
    {
        public Action<Board> updateSide;



        public void resetSide(Side side, BoardFunctionality boardFunc)
        {
            foreach (FunctionalRow row in side.Rows)
            {
                foreach (Card card in row.cardsInContainer)
                {
                    card.cardProps.exhausted = false;
                }
            }
            foreach (FunctionalRow row in side.Rows)
            {
                foreach (Card card in row.cardsInContainer)
                {
                    if (card.cardProps.doubleExhausted == true)
                    {
                        card.cardProps.exhausted = true;
                        card.cardProps.doubleExhausted = false;
                    }
                }
            }
            resetAttributes(boardFunc.friendlySide);
            resetAttributes(boardFunc.enemySide);
            resetFog(boardFunc.enemySide);
            resetFog(boardFunc.friendlySide);
            boardFunc.enemySide.Resources = new List<Card.Race>();
            boardFunc.friendlySide.Resources = new List<Card.Race>();
            side.canPlayArmy = true;
        }
        public void resetAttributes(Side side)
        {
            foreach (FunctionalRow row in side.Rows)
            {
                foreach (Card card in row.cardsInContainer)
                {
                    if (card.cardProps.type != CardType.General)
                    {

                        card.cardProps.defense = card.cardProps.initialDefense;
                        card.cardProps.power = card.cardProps.initialPower;
                        card.cardProps.aiCalcDefense = card.cardProps.initialDefense;
                    }
                }
            }
        }
        public void resetAllExhaustedCardsOnSide(Side side)
        {
            foreach (FunctionalRow row in side.Rows)
            {
                foreach (Card card in row.cardsInContainer)
                {
                    card.cardProps.exhausted = false;
                }
            }
        }

        public void resetFog(Side side)
        {
            foreach (FunctionalRow row in side.Rows)
            {
                row.revealed = row.revealedTrueValue;
            }
        }


        public void initSide(Board board, Side friendlySide_relatively, BoardFunctionality boardFunc)
        {
            if (board.enemySide == friendlySide_relatively)
            {
                boardFunc.friendlySide = friendlySide_relatively;
                updateSide = (Board newBoard) => {
                    boardFunc.enemySide = newBoard.friendlySide;
                    boardFunc.friendlySide = newBoard.enemySide;
                    newBoard.controllingPlayer = boardFunc.controllingPlayer;
                };
                updateSide(board);
            }
            if (board.friendlySide == friendlySide_relatively)
            {
                boardFunc.friendlySide = friendlySide_relatively;
                updateSide = (Board newBoard) => {
                    boardFunc.enemySide = newBoard.enemySide;
                    boardFunc.friendlySide = newBoard.friendlySide;
                    newBoard.controllingPlayer = boardFunc.controllingPlayer;
                };
            }
            updateSide(board);
        }
    }
}
