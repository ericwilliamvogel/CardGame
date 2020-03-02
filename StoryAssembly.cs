using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CardGame
{
    public class StorySlideAssembly
    {
        StorySlideConstructor tempConstructor;
        public void assembleStorySlide(StorySlideConstructor constructor, int selectedLevel)
        {
            tempConstructor = constructor;
            tempConstructor.setSlide();
            setCorrectSlide(selectedLevel);
        }

        private void setCorrectSlide(int selectedLevel)
        {
            string fileName = "DELETELATERPORTRAIT";
            CharacterProfile narrator = new CharacterProfile("Narrator", fileName);
            CharacterProfile main = new CharacterProfile("Forgotten Soul", fileName);
            CharacterProfile mainSub = new CharacterProfile("Distant Voice", fileName);

            CharacterProfile soldier = new CharacterProfile("Forgotten Soldier", fileName);
            CharacterProfile groupOfSoldiers = new CharacterProfile("Group of Forgotten Men", fileName);
            CharacterProfile forgottenMan = new CharacterProfile("Forgotten Man", fileName);
            tempConstructor.selectBackground("blueBackground");
            if (selectedLevel == 0)
            {
                tempConstructor.addSlide("Not all good will come from good. Not all evil will come from evil.", narrator);
                tempConstructor.addSlide("Our story starts in a world that only knows good.", narrator);
                tempConstructor.addSlide("A just government, a just community, and peace in even the darkest places.", narrator);
                tempConstructor.addSlide("But there is a growing unrest amongst this perfect world, for a new emotion has surfaced.", narrator);
                tempConstructor.addSlide("An emotion that has not been recalled for centuries, an emotion that has just now rediscovered its name...", narrator);
                tempConstructor.addSlide("Fear.", narrator);

                tempConstructor.addSlide("The air smells fresh.", main);
                tempConstructor.addSlide("I vaguely recall this place.", main);
                tempConstructor.addSlide("Yes... I used to visit this place when I was a child.", main);
                tempConstructor.addSlide("I think this place once brought me joy...", main);
                tempConstructor.addSlide("...But now it just fills me with anger.", main);

                tempConstructor.addSlide("Sir?", soldier);

                tempConstructor.addSlide("Is it time?", main);

                tempConstructor.addSlide("We're ready when you are.", soldier);

                tempConstructor.addSlide("Let's begin.", main);
            }
            else if (selectedLevel == 1)
            {
                tempConstructor.addSlide("And so, with one decision, the world was sent down an unknown path.", narrator);
                tempConstructor.addSlide("Perhaps there was pain in this perfect world...", narrator);
                tempConstructor.addSlide("...and how dangerous it is for pain to fester.", narrator);

                tempConstructor.addSlide("We... actually did it!?", soldier);
                tempConstructor.addSlide("I've never felt this before...", soldier);
                tempConstructor.addSlide("It feels like...", soldier);
                tempConstructor.addSlide("A chapter that we create. A chapter that isn't made for us.", soldier);

                tempConstructor.addSlide("It's our own path. We will make the way for others.", main);
                tempConstructor.addSlide("Stick by my side. Get used to this feeling.", main);
                tempConstructor.addSlide("There are no locks on the doors. The guards are unsuspecting and weak.", main);
                tempConstructor.addSlide("We are nothing but a bedtime story that they tell their kids.", main);
                tempConstructor.addSlide("Let's remind them of reality.", main);

            }
            else if (selectedLevel == 2)
            {
                tempConstructor.addSlide("On they went, seeming to be evil in its purest form...", narrator);
                tempConstructor.addSlide("However... ", narrator);
                tempConstructor.addSlide("These nameless servants of destruction were recruiting more soldiers to their army.", narrator);
                tempConstructor.addSlide("Their attacks struck a chord in the hearts of the dispossessed.", narrator);
                tempConstructor.addSlide("This dispossession was not seen in the world as it was.", narrator);
                tempConstructor.addSlide("Light was starting spark, showing that the world may not have been so joyful as it seemed.", narrator);

                tempConstructor.addSlide("*Approaches*", groupOfSoldiers);
                tempConstructor.addSlide("Bring us with you, we don't care where you're going.", forgottenMan);

                tempConstructor.addSlide("Ah... more soldiers for the front line.", main);
                tempConstructor.addSlide("Don't you all have homes you want to return to?", main);

                tempConstructor.addSlide("Comfort is exhausting. There is no meaning anymore. Our homes are full of nice things, yet so empty.", forgottenMan);
                tempConstructor.addSlide("This perfect world wasn't meant for humans. I'd rather feel pain than nothing.", forgottenMan);

                tempConstructor.addSlide("*Cheers*", groupOfSoldiers);

                tempConstructor.addSlide("The road will be painful. I can assure you of that.", main);
                tempConstructor.addSlide("Death is in our future.", main);

                tempConstructor.addSlide("*Cheers*", groupOfSoldiers);

                tempConstructor.addSlide("It doesn't have to be.", mainSub);

                tempConstructor.addSlide("What was that?", main);
                tempConstructor.addSlide("..........", main);
                tempConstructor.addSlide("Must've been my imagination.", main);

                tempConstructor.addSlide("Must've been.", mainSub);
            }
            else
            {
                tempConstructor.addSlide("Start Game", new CharacterProfile("", "DELETELATERPORTRAIT"));
            }
        }

    }
}