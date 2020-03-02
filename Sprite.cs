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
                Console.Write(e);
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