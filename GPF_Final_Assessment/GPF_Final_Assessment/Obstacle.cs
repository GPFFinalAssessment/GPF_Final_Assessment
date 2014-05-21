using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace GPF_Final_Assessment
{
    public class Obstacle
    {
        public Texture2D obstacleTexture;
        public Vector2 obstaclePosition;
        public Vector2 obstacleOffset;
        public Vector2 obstacleOffsetPosition;
        public float obstacleDefaultSpeed;
        public float obstacleScrollSpeed;
        public static int obstaclePlacementSpace = 130;
        public static int obstaclePlacementOffset = 60;

        public Obstacle(Texture2D texture, Vector2 position, float speed)
        {
            this.obstacleTexture = texture;
            this.obstaclePosition = position;
            //we need to calculate the speed of the enemy movement for the same speed of the background
            this.obstacleDefaultSpeed = speed / 60;
            this.obstacleScrollSpeed = obstacleDefaultSpeed;
            this.obstacleOffset = new Vector2(obstacleTexture.Width, obstacleTexture.Height) / 2.0f;
        }

        public void Update(GameTime gameTime)
        {
            //we need to add some gravity here!
            obstaclePosition.X -= obstacleScrollSpeed;
            obstacleOffsetPosition = obstaclePosition - obstacleOffset;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(obstacleTexture, obstaclePosition - obstacleOffset, Color.White);
        }
    }
}

