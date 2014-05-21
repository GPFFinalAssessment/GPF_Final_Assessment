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
        public Vector2 enemyPosition;
        public Vector2 enemyOffset;
        public Vector2 enemyOffsetPosition;
        public float enemyDefaultSpeed;
        public float enemyScrollSpeed;

        public Enemy(Texture2D texture, Vector2 position, float speed)
        {
            this.enemyTexture = texture;
            this.enemyPosition = position;
            //we need to calculate the speed of the enemy movement for the same speed of the background
            this.enemyDefaultSpeed = speed / 60;
            this.enemyScrollSpeed = enemyDefaultSpeed;
            this.enemyOffset = new Vector2(enemyTexture.Width, enemyTexture.Height) / 2.0f;
        }

        public void Update(GameTime gameTime)
        {
            //we need to add some gravity here!
            enemyPosition.X -= enemyScrollSpeed;
            enemyOffsetPosition = enemyPosition - enemyOffset;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(enemyTexture, enemyPosition - enemyOffset, Color.White);
        }
    }
}

