using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace GPF_Final_Assessment
{
    public class Enemy
    {
        public Texture2D enemyTexture;
        public Vector2 enemyposition;
        public Vector2 enemyoffsetposition;
        public float enemyDefaultSpeed;
        public float enemyScrollSpeed;
        public Game1 game;

        public Enemy(Texture2D texture, Vector2 position, float speed)
        {
            this.enemyTexture = texture;
            this.enemyposition = position;
            //we need to calculate the speed of the enemy movement for the same speed of the background
            this.enemyDefaultSpeed = speed / 60;
            this.enemyScrollSpeed = enemyDefaultSpeed;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 offset = new Vector2(enemyTexture.Width, enemyTexture.Height) / 2.0f;

            //we need to add some gravity here!
            enemyposition.X -= enemyScrollSpeed;
            enemyoffsetposition = enemyposition - offset;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 offset = new Vector2(enemyTexture.Width, enemyTexture.Height) / 2.0f;
            spriteBatch.Draw(enemyTexture, enemyposition - offset, Color.White);
        }
    }
}

