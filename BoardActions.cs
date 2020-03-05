using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class BoardActions
    {
        private bool active;
        
        public List<Action> actions;
        public BoardActions()
        {
            actions = new List<Action>();
        }
        public void nextAction()
        {
            actions.Remove(actions[0]);
            if(!thereAreActionsLeft())
            {
                active = false;
            }
        }
        public void AddAction(Action action)
        {
            actions.Add(action);
            active = true;
        }
        private bool thereAreActionsLeft()
        {
            if(actions.Count > 0)
            {
                return true;
            }
            return false;
        }
        public void updateAnimations()
        {
            if (thereAreActionsLeft())
                actions[0]();
        }
    }
}
