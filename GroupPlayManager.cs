using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class GroupPlayManager
    {
        public List<GroupPlay> groupPlays = new List<GroupPlay>();

        public void AddGroup(Dictionary<Card, CardPlayShell> friendlyCards)
        {
            GroupPlay groupPlay = new GroupPlay();
            groupPlay.addGroupPlay(friendlyCards);
            groupPlays.Add(groupPlay);
        }
        public void executeBestPlay(BoardFunctionality boardFunc)
        {

            int PLAYCOUNTER = 0;
            int highestValue = 0;
            GroupPlay selectedCombination = null;
            foreach (GroupPlay group in groupPlays)
            {
                if (highestValue == 0)
                {
                    highestValue = group.fullPlayValue;
                    selectedCombination = group;
                }
                if (group.fullPlayValue > highestValue)
                {
                    highestValue = group.fullPlayValue;
                    selectedCombination = group;
                }
                PLAYCOUNTER++;
            }
            boardFunc.enemySide.boardFunc.BOARDMESSAGE.addMessage("Current play value = " + highestValue.ToString());
            boardFunc.enemySide.boardFunc.BOARDMESSAGE.addMessage("Amount of group combos parsed = " + PLAYCOUNTER.ToString());
            //throw new Exception(highestValue.ToString());

            if (selectedCombination == null)
            {
                boardFunc.BOARDMESSAGE.addMessage("For some reason the AI is not functioning~");
            }
            else
            {
                foreach (Play play in selectedCombination.plays)
                {
                    play.realPlay();
                }
            }
            groupPlays = new List<GroupPlay>();
        }
    }

}
