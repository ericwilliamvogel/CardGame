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
            extendedPos = new Vector2(getPosition().X, getPosition().Y - positionOffset);
            base.initializeGameComponent();
        }
        public Action action;
        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            switch (state)
            {

                case State.Retracted:
                    setPos(initPos);
                    if (isWithinBox(mouseState))
                    {
                        state = State.Extended;
                        //action();
                    }
                    break;
                case State.Extended:
                    setPos(extendedPos);
                    if (!isWithinBox(mouseState))
                    {
                        state = State.Retracted;
                        //action();
                    }
                    break;


            }

        }
    }
}
