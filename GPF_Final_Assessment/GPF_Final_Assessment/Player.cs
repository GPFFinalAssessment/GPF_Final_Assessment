using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace GPF_Final_Assessment
{
    public class Player
    {
        public Texture2D playerTexture;
        public Vector2 playerPosition;
        public Vector2 playerOffset;
        public Vector2 playerOffsetPosition;
        public float playerDefaultSpeed;
        public float playerMovementSpeed;
        public float playerHealth;
        public int playerScore;
        public int playerWifi;
        public int playerWifiNeeded;        
        
        public Player(Texture2D texture, Vector2 pos, int WifiNeeded)
        {
            this.playerTexture = texture;
            this.playerPosition = pos;
            this.playerDefaultSpeed = 2f;
            this.playerMovementSpeed = playerDefaultSpeed;
            this.playerHealth = 100;
            this.playerOffset = new Vector2(playerTexture.Width / 2, 0);
            this.playerScore = 0;
            this.playerWifi = 0;
            this.playerWifiNeeded = WifiNeeded;
        }

        public void Update(GameTime gametime, GraphicsDeviceManager gdManager, int haltMovement)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && playerPosition.Y > 120 && haltMovement != 2) playerPosition.Y -= playerMovementSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && (playerPosition.Y < (gdManager.PreferredBackBufferHeight - playerMovementSpeed - this.playerTexture.Height)) && haltMovement != 3) playerPosition.Y += playerMovementSpeed;
            playerOffsetPosition = playerPosition - playerOffset;
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            spritebatch.Draw(playerTexture, playerPosition - playerOffset, Color.White);
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