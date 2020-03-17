using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class HandSpace : GameComponent
    {
        int positionOffset;
        Vector2 initPos;
        Vector2 extendedPos;
        public HandSpace()
        {

            setContentName("handArea");
        }
        public enum State
        {
            Retracted,
            Extended
        }
        public State state;

        public override void initializeGameComponent()
        {
            positionOffset = (int)(150 * getScale().Y);
            initPos = getPosition();
            extendedPos = new Vector2(getPosition().X, getPosition().Y + positionOffset);
            state = State.Retracted;
            base.initializeGameComponent();
        }
        public Action action;
        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            //action();
            switch (state)
            {

                case State.Retracted:
                    setPos(extendedPos);

                    break;
                case State.Extended:
                    
                    setPos(initPos);
                    break;


            }

        }
    }
}
