using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace GPF_Final_Assessment
{
    public class Player
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 offsetposition;
        public Game1 game;
        public float defaultSpeed;
        public float movementSpeed;
        public float playerHealth;
        
        public Player(Texture2D texture, Vector2 pos)
        {
            this.texture = texture;
            this.position = pos;
            this.defaultSpeed = 2f;
            this.movementSpeed = defaultSpeed;
            this.playerHealth = 100;
        }

        public void Update(GameTime gametime, GraphicsDeviceManager gdManager, int haltMovement)
        {
            Vector2 offset = new Vector2(texture.Width / 2, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && position.Y > 120 && haltMovement != 2) position.Y -= movementSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && (position.Y < (gdManager.PreferredBackBufferHeight - movementSpeed - this.texture.Height)) && haltMovement != 3) position.Y += movementSpeed;
            offsetposition = position - offset;
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            Vector2 offset = new Vector2(texture.Width / 2, 0);

            spritebatch.Draw(texture, position - offset, Color.White);
        }

        public void UpdateHealth(int intPoints, bool bolIncrease)
        {
            if (bolIncrease)
            {
                playerHealth += intPoints;

                if (playerHealth > 100)
                    playerHealth = 100;
            }
            else
            {
                playerHealth -= intPoints;

                if (playerHealth < 0)
                    playerHealth = 0;
            }         
        }

        public bool isAlive()
        {
            if(playerHealth > 0)
                return true;
            else
                return false;
        }
    }
}