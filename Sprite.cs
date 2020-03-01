using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SiegewithCleanCode
{
    public class AnimatedSprite
    {
        private Sprite thisSprite;
        private GameComponent thisComponent;
        private AnimationKey commonAnimationKey;
        public AnimatedSprite(Sprite sprite, GameComponent component)
        {
            thisSprite = sprite;
            thisComponent = component;
            setAnimationSpeed(.1);
        }
        private int componentWidth, componentHeight;

        private int columns = 4, rows = 0;
        public double currentFrame;

        public bool oneTimeAnimation = false;
        public void resetSprite()
        {
            currentFrame = 0;
            //completedAnimation = true;
        }
        private double animationSpeed;
        public void setAnimationSpeed(double speed)
        {
            animationSpeed = speed;
        }
        public void Draw(SpriteBatch spriteBatch, AnimationKey animationKey) //this function is going to animate the character and move frames
        {

            if (animationKey != commonAnimationKey && oneTimeAnimation == false)
            {
                currentFrame = 0;
                oneTimeAnimation = true;
            }

            rows = (int)animationKey.getColumnsAndRows().Y;
            columns = (int)animationKey.getColumnsAndRows().X;
            componentWidth = thisComponent.properties.getWidth();
            componentHeight = thisComponent.properties.getHeight();

            if (thisComponent.Animating)
            {
                currentFrame = currentFrame + animationSpeed;
                if (currentFrame + animationSpeed > columns && oneTimeAnimation)
                {
                    currentFrame = 0;
                    oneTimeAnimation = false;
                    completedAnimation = true;
                }
            }
            else
                currentFrame = 0;

            int column = (int)currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(componentWidth * column, componentHeight * rows,
                componentWidth, componentHeight);
            //executeSingleExecutionAnimation(column);

            spriteBatch.Draw(thisComponent.properties.sprite.getLoadedTexture(),
                thisComponent.properties.getDrawingPosition(), null, sourceRectangle, null, 0, null, thisComponent.color,
                effects: thisComponent.properties.spriteEffects, layerDepth: 0);



        }

        public double getCurrentFrame()
        {
            return currentFrame;
        }
        public bool completedAnimation;


        public void selectCommonAnimation(AnimationKey animationKey)
        {
            commonAnimationKey = animationKey;
            columns = (int)animationKey.getColumnsAndRows().X;
            rows = (int)animationKey.getColumnsAndRows().Y;
        }

        public void setSingleExecutionAnimation(AnimationKey animationKey)
        {
            columns = (int)animationKey.getColumnsAndRows().X;
            rows = (int)animationKey.getColumnsAndRows().Y;
            oneTimeAnimation = true;
        }

        public void revertToNormal()
        {
            rows = 0;
            columns = 4;
        }
    }

    public class Sprite
    {
        private SpriteComponent spriteComponent;

        public Sprite(ContentManager content, string contentName)
        {
            spriteComponent = new SpriteComponent();
            LoadSprite(content, contentName);
        }

        public void LoadSprite(ContentManager content, string contentName)
        {
            try
            {
                spriteComponent.loadedTexture = content.Load<Texture2D>(contentName);
                if (spriteComponent.loadedTexture == null)
                {
                    throw new Exception();
                }
                spriteComponent.textureParamaters = new Vector2(spriteComponent.loadedTexture.Width,
                    spriteComponent.loadedTexture.Height);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public void UnloadSprite()
        {
            spriteComponent.loadedTexture = null;
        }

        public Texture2D getLoadedTexture()
        {
            return spriteComponent.loadedTexture;
        }

        public Vector2 getTextureParamaters()
        {
            return spriteComponent.textureParamaters;
        }


    }


    public class SpriteComponent
    {
        public Texture2D loadedTexture;
        public Vector2 textureParamaters;
    }
}