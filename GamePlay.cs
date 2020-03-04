using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    class GamePlay : PrimaryComponent
    {
        GameState state;
        public GamePlay()
        {
            state = GameState.DrawHand;
        }
        public void assignAction()
        {

        }
        /*public enum TurnState
        {
            Player1Start,
            Player1Decision,
            Player2Start,
            Player2Decision
        }*/
        public override void initializeGameComponent(ContentManager content)
        {
            //load all card textures
            //load decks of players

        }
        Player controllingPlayer;
        Player player1;
        Player player2;

        public void handlePlayers(Player firstPlayer, Player secondPlayer)
        {
            player1 = firstPlayer;
            player2 = secondPlayer;

            controllingPlayer = firstPlayer;

            //controllingPlayer.Draw
        }
        public void StartGame(Player firstPlayer, Player secondPlayer)
        {
            firstPlayer.DrawHand();
            secondPlayer.DrawHand();
        }
        public void newTurnSwitchPlayer()
        {
            controllingPlayer = getOtherPlayer();
        }
        public void handleGameStates()
        {
            switch (state)
            {
                case GameState.DrawHand:
                    controllingPlayer.hasControl();
                    break;
                case GameState.Play:
                    controllingPlayer.hasControl();
                    break;
                case GameState.Pause:
                    controllingPlayer.loseControl();
                    break;
                case GameState.Pass:
                    newTurnSwitchPlayer();
                    break;
            }
        }
        private Player getOtherPlayer()
        {
            Player player = null;
            if (controllingPlayer == player1)
            {
                player = player2;
            }

            if (controllingPlayer == player2)
            {
                player = player1;
            }

            return player;
        }


    }
}
