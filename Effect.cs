using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Ability
    {
        public bool clickedInAbilityBox;
        public SelectUnitAction selectAction = new SelectUnitAction();
        public Card INITIALCARD;
        public string name;
        public string description;
        public int power;
        public int exchangeValue;

        public void displayGeneralIncrements(int exchangeValue)
        {
            this.exchangeValue = exchangeValue;
            string startingString = exchangeValue.ToString();
            if (exchangeValue == 0)
            {
                name = startingString + ":";

            }
            else if (exchangeValue > 0)
            {
                name = "+" + startingString + ":";
            }
            else
            {
                name = "" + startingString + ":";
            }
        }
        public enum Type
        {
            Targeted,
            Global,
            Upkeep
        }
        public Ability()
        {
            description = "no description";
        }
        
        public virtual void setCard(Card card)
        {
            INITIALCARD = card;
            if(name == null || name == "")
            name = getTrueName();
        }
        public virtual void activateAbilityOnSelection(MouseState mouseState, BoardFunctionality boardFunc)
        {
            if (INITIALCARD.cardProps.exhausted == false && clickedInAbilityBox == false)
            {

                boardFunc.cardViewer.resetCardSelectionOnRightClick(mouseState, boardFunc);
                useAbility(mouseState, boardFunc);
                clickedInAbilityBox = true;
                resetAllCards(boardFunc);
            }
            else if (INITIALCARD.cardProps.type == CardType.Manuever)
            {
                boardFunc.cardViewer.resetCardSelectionOnRightClick(mouseState, boardFunc);
                useAbility(mouseState, boardFunc);
                clickedInAbilityBox = true;
                resetAllCards(boardFunc);
            }
        }
        public virtual void setTarget()
        {
            MouseTransformer.Set(MouseTransformer.State.Tgt);
        }
        public virtual Card returnSelectedCard(MouseState mouseState, BoardFunctionality boardFunc)
        {
            return null;
        }
        public void resetAllCards(BoardFunctionality boardFunc)
        {
            resetSide(boardFunc.enemySide);
            resetSide(boardFunc.friendlySide);
            MouseTransformer.Set(MouseTransformer.State.Reg);
        }
        public string getTrueName()
        {
            string name;
            if (INITIALCARD.cardProps.type == CardType.Manuever)
            {
                name = "";
            }
            else
            {
                name = "->: ";
            }
            return name;
        }
        public virtual void useAbility(MouseState mouseState, BoardFunctionality boardFunc)
        {
            if (INITIALCARD.cardProps.type == CardType.General)
            {
                if (exchangeValue < 0)
                {
                    if(INITIALCARD.cardProps.defense >= GameComponent.ToAbsolute(exchangeValue))
                    {
                        INITIALCARD.cardProps.defense += exchangeValue;
                        abilityImplementation(mouseState, boardFunc);
                        boardFunc.cardViewer.NextSelection();
                    }
                    else
                    {
                        boardFunc.BOARDMESSAGE.addMessage(INITIALCARD.cardProps.name + " does not have enough power yet.");
                        clickedInAbilityBox = false;
                        resetAllCards(boardFunc);
                        boardFunc.cardViewer.hardResetSelection(boardFunc);
                    }

                }
                else
                {
                    INITIALCARD.cardProps.defense += exchangeValue;

                    abilityImplementation(mouseState, boardFunc);
                    boardFunc.cardViewer.NextSelection();
                }
            }
            else if(INITIALCARD.cardProps.type == CardType.Manuever)
            {
                abilityImplementation(mouseState, boardFunc);
                manueverImplementation(boardFunc);
            }
            else
            {

                abilityImplementation(mouseState, boardFunc);
                boardFunc.cardViewer.NextSelection();
            }


        }
        public virtual void manueverImplementation(BoardFunctionality boardFunc)
        {
                boardFunc.cardViewer.NextSelection();
                if (!boardFunc.cardViewer.SelectionStillActive())
                    boardFunc.actionConstructor.moveTo(boardFunc.castManuever, boardFunc.friendlySide.Oblivion, INITIALCARD, boardFunc);
        }
        public virtual void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {

        }
        public virtual void useAIAbility(AIPlayer player, BoardFunctionality boardFunc, Card targetCard)
        {

        }
        private void resetSide(Side side)
        {
            foreach (FunctionalRow row in side.Rows)
            {
                foreach(Card card in row.cardsInContainer)
                {
                    card.setRegular();
                }
            }
        }
    }
    public class CreateSpell : Ability
    {
        public int identifier;
        public string details;
        public CreateSpell(int identifier, string details)
        {
            this.identifier = identifier;
            description = "Create and draw " + details;

        }
        public CreateSpell(int identifier, int exchangeValue, string details) : this(identifier, details)
        {
            displayGeneralIncrements(exchangeValue);
        }
        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {
            boardFunc.CreateAndDrawSpell(INITIALCARD, this);
        }
    }
    public class Target : Ability
    {
        public override void activateAbilityOnSelection(MouseState mouseState, BoardFunctionality boardFunc)
        {
            
            if (returnSelectedCard(mouseState, boardFunc) != null && clickedInAbilityBox == false)
            {
                if (selectAction.TargetEnemyCard(mouseState, boardFunc, true).correctRow(boardFunc.enemySide).revealed)
                {

                    boardFunc.cardViewer.resetCardSelectionOnRightClick(mouseState, boardFunc);
                    useAbility(mouseState, boardFunc);
                    clickedInAbilityBox = true;
                    resetAllCards(boardFunc);
                }
            }
            selectAction.resetIfNoSelection(mouseState, INITIALCARD.getCurrentContainer(boardFunc.friendlySide), INITIALCARD, boardFunc);
        }
        public override Card returnSelectedCard(MouseState mouseState, BoardFunctionality boardFunc)
        {

            if (selectAction.TargetEnemyCard(mouseState, boardFunc, true) != null)
            {
                return selectAction.TargetEnemyCard(mouseState, boardFunc, true);
            }
            else
                return null;

            //throw new Exception();
        }
    }
    public class BothSidesDrawCard : Ability
    {
        private void setDesc()
        {
            if (power <= 1)
            {
                description = "Both draw card.";
            }
            else
            {
                description = "Both draw " + power.ToString() + " cards.";
            }
        }
        public BothSidesDrawCard(int amountOfCards)
        {
            this.power = amountOfCards;
            setDesc();

        }
        public BothSidesDrawCard(int amountOfCards, int exchangeValue)
        {
            this.power = amountOfCards;
            displayGeneralIncrements(exchangeValue);
            setDesc();
        }

        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {

            boardFunc.AbilityBothDrawCard(INITIALCARD, this, boardFunc.friendlySide);
        }

    }
    public class DrawCard : Ability
    {
        private void setDesc()
        {
            if (power <= 1)
            {
                description = "Draw a card.";
            }
            else
            {
                description = "Draw " + power.ToString() + " cards.";
            }
        }
        public DrawCard(int amountOfCards)
        {
            this.power = amountOfCards;
            setDesc();

        }
        public DrawCard(int amountOfCards, int exchangeValue)
        {
            this.power = amountOfCards;
            displayGeneralIncrements(exchangeValue);
            setDesc();
        }

        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {
            boardFunc.AbilityDrawCard(INITIALCARD, this, boardFunc.friendlySide);
        }
    }
    public class Exhaust : Ability
    {
        public Exhaust()
        {
            description = "Exhaust enemy unit.";
        }
        public Exhaust(int exchangeValue)
        {
            displayGeneralIncrements(exchangeValue);
        }
        public override Card returnSelectedCard(MouseState mouseState, BoardFunctionality boardFunc)
        {

            if (selectAction.TargetEnemyCard(mouseState, boardFunc, true) != null)
            {
                return selectAction.TargetEnemyCard(mouseState, boardFunc, true);
            }
            else
                return null;

            //throw new Exception();
        }
        public override void activateAbilityOnSelection(MouseState mouseState, BoardFunctionality boardFunc)
        {
            if (returnSelectedCard(mouseState, boardFunc) != null && INITIALCARD.cardProps.exhausted == false && clickedInAbilityBox == false)
            {
                if (selectAction.TargetEnemyCard(mouseState, boardFunc, true).correctRow(boardFunc.enemySide).revealed)
                {
                    boardFunc.cardViewer.resetCardSelectionOnRightClick(mouseState, boardFunc);
                    useAbility(mouseState, boardFunc);
                    clickedInAbilityBox = true;
                    resetAllCards(boardFunc);
                }
            }
            selectAction.resetIfNoSelection(mouseState, INITIALCARD.getCurrentContainer(boardFunc.friendlySide), INITIALCARD, boardFunc);
        }
        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {
            boardFunc.Exhaust(INITIALCARD, this, returnSelectedCard(mouseState, boardFunc));
        }

        public override void useAIAbility(AIPlayer player, BoardFunctionality boardFunc, Card targetCard)
        {

        }

    }
    public class SpawnCard : Ability
    {
        public int identifier;
        public string details;
        public SpawnCard(int identifier, string details)
        {
            this.identifier = identifier;
            description = "Spawn " + details;

        }
        public SpawnCard(int identifier, int exchangeValue, string details) : this(identifier, details)
        {
            displayGeneralIncrements(exchangeValue);
        }
        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {
            boardFunc.SpawnCard(INITIALCARD, this);
        }

        public override void useAIAbility(AIPlayer player, BoardFunctionality boardFunc, Card targetCard)
        {
            if (player != null)
            {
                boardFunc.boardDef.revealBoardForRemainderOfTurn(targetCard, this, boardFunc);
            }

        }

    }
    public class KillTarget : Target
    {
        public KillTarget()
        {
            description = "Kill unit.";
        }
        public KillTarget(int exchangeValue) : this()
        {
            displayGeneralIncrements(exchangeValue);

        }
        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {
            boardFunc.KillTarget(INITIALCARD, this, returnSelectedCard(mouseState, boardFunc)); //Kill(boardFunc.enemySide, returnSelectedCard(mouseState, boardFunc).correctRow(boardFunc.enemySide), returnSelectedCard(mouseState, boardFunc));
        }
    }

    public class BoardDamage : TargetDamage
    {
        public BoardDamage(int damage, int exchangeValue) : base(damage, exchangeValue)
        {
            description = "Deal " + damage + " to row.";
        }

        public override void abilityImplementation (MouseState mouseState, BoardFunctionality boardFunc)
        {
            if(returnSelectedCard(mouseState, boardFunc) != null)
            boardFunc.BoardDamage(INITIALCARD, this, returnSelectedCard(mouseState, boardFunc));
        }

        public override void useAIAbility(AIPlayer player, BoardFunctionality boardFunc, Card targetCard)
        {
            if (player != null)
            {
                foreach(Card card in targetCard.correctRow(boardFunc.enemySide).cardsInContainer)
                {
                    card.cardProps.aiCalcDefense -= this.power;
                }

            }

        }
    }
    public class Reveal : Ability
    {
        public Reveal()
        {
            description = "Reveal board.";
        }
        public Reveal(int exchangeValue) : this()
        {
            displayGeneralIncrements(exchangeValue);

        }
        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {
            boardFunc.RevealBoard(INITIALCARD, this);
        }

        public override void useAIAbility(AIPlayer player, BoardFunctionality boardFunc, Card targetCard)
        {
            if (player != null)
            {
                boardFunc.boardDef.revealBoardForRemainderOfTurn(targetCard, this, boardFunc);
            }

        }
    }
    public class LifeTargetDamage : Ability
    {
        public LifeTargetDamage(int damage)
        {
            power = damage;
            description = "Deal " + damage + " to enemy player.";

        }
        public LifeTargetDamage(int damage, int exchangeValue) : this(damage)
        {
            displayGeneralIncrements(exchangeValue);
        }
        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {
            boardFunc.LifeDamage(INITIALCARD, this);
        }
        }
    public class TargetDamage : Target
    {
        
        public TargetDamage(int damage)
        {
            power = damage;
            description = "Deal " + damage + " to unit.";
            
        }
        public TargetDamage(int damage, int exchangeValue) : this(damage)
        {
            displayGeneralIncrements(exchangeValue);

        }
        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {

            boardFunc.DirectDamage(INITIALCARD, this, returnSelectedCard(mouseState, boardFunc));
        }

        public override void useAIAbility(AIPlayer player, BoardFunctionality boardFunc, Card targetCard)
        {
            if (player != null)
            {
                targetCard.cardProps.aiCalcDefense -= this.power;
            }

        }
    }


    /*public class GeneralAbility : Ability
    {
        //public GeneralAbility() { }
    }*/
    public class EffectSensor //LATER
    {

    }
    public class Effect //LATER
    {
        public enum EffectType
        {
            SpawnUnit,
            DestroyUnitWithPowerUnder,
            DestroyUnitWithPowerOver,
            DamageDirectUnit,
            DamageAllUnits,
            Illuminate
        }
        public enum EffectTrigger
        {
            OnTurnStart,
            OnYourTurnStart,
            OnAttack,
            OnDefense,
            OnBaseHit,
            OnLeavePlay,
            OnEnterPlay
        }
        string text;
        //private Effect effect;
        public Effect(EffectType type, EffectTrigger trigger, int value)
        {

        }
        /*public Effect(EffectType type, EffectTrigger trigger, Card card, Effect newEffect)
        {
            effect = newEffect;
        }
        public bool Trigger(EffectTrigger trigger)
        {
            return true;/////
        }*/

        public void setTrigger()
        {

        }
    }
}
