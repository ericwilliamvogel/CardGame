using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public static class BoardActions
    {
        public static bool active;
        //public static int controller; //goes into higher function to switch when complete
        public static List<Action> actions;
        public static void assignAnimation(Action action)
        {
            Action newAction = action;
            //Action setController = () => { controller++; };
            //Action here = newAction + setController;
            actions.Add(newAction);
        }
        public static void nextAction()
        {
            actions.Remove(actions[0]);
        }
        public static void updateAnimations()
        {
            if (actions != null)
                actions[0]();
        }
    }
}
