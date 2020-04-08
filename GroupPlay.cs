using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class GroupPlay
    {
        public List<Play> plays;
        public int fullPlayValue;
        public void addGroupPlay(List<Play> newGroupPlay)
        {
            plays = new List<Play>();
            plays = newGroupPlay;
            fullPlayValue = calcPlayValue();
        }
        public void addGroupPlay(Dictionary<Card, CardPlayShell> friendlyCards)
        {
            plays = new List<Play>();
            foreach (KeyValuePair<Card, CardPlayShell> pair in friendlyCards)
            {
                Play play = new Play();
                play = pair.Value.play[pair.Value.iterator];
                plays.Add(play);
            }
            fullPlayValue = calcPlayValue();
        }
        public int calcPlayValue()
        {
            int totalValue = 0;
            foreach (Play play in plays)
            {
                totalValue += play.testPlay();

            }
            foreach (Play play in plays)
            {
                play.resetValues();
            }
            return totalValue;
        }
    }
}
