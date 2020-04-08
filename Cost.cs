using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Cost
    {
        public Cost()
        {

        }
        public Cost(int unanimousCost, Race[] races)
        {
            raceCost = new List<Race>();
            this.unanimousCost = unanimousCost;
            raceCost.AddRange(races.ToList());
            totalCost = unanimousCost + raceCost.Count;
        }
        public List<Race> raceCost;
        public int unanimousCost = 0;
        public int totalCost = 0;
    }
}
