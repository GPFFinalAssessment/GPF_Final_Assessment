using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GPF_Final_Assessment
{
    class Background
    {
        public Texture2D backgroundTexture;
        public Texture2D backgroundEndTexture;
        public Vector2 backgroundPosition;
        public double backgroundPixelsToFinishScreen;
        public float backgroundNumberOfScreens;
        public float backgroundScrollSpeed;
        public float pixelsPassed;

        public Background(Texture2D texture, Texture2D endtexture, Vector2 pos, float secondsToFinish, float scrollSpeed)
        {
            backgroundTexture = texture;
            backgroundEndTexture = endtexture;
            backgroundPosition = pos;
            backgroundScrollSpeed = scrollSpeed;
            pixelsPassed = 0;

            //calculate how many screens we need to display by the number of seconds at regular speed
            backgroundPixelsToFinishScreen = secondsToFinish * backgroundScrollSpeed;
            backgroundNumberOfScreens = (float)(backgroundPixelsToFinishScreen / backgroundTexture.Width);
        }

        public void Update(GameTime gameTime)
        {
            float intMovement;
 
            //update the positioning of the scrolling background
            //use the number of pixels we have passed to figure out if we are done yet
            if (!IsEnd())
            {
                intMovement = (float)(gameTime.ElapsedGameTime.TotalSeconds * backgroundScrollSpeed);
                pixelsPassed += intMovement;
                backgroundPosition.X -= intMovement;
                backgroundPosition.X = backgroundPosition.X % backgroundTexture.Width;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (DisplayEnd())
            {
                if (backgroundPosition.X > -(backgroundEndTexture.Width - 550))
                    spriteBatch.Draw(backgroundEndTexture, backgroundPosition, null, Color.White);
                else if (backgroundPosition.X > -backgroundTexture.Width && backgroundPosition.X < -(backgroundTexture.Width - 550))
                    spriteBatch.Draw(backgroundTexture, backgroundPosition, null, Color.White);
                
                spriteBatch.Draw(backgroundEndTexture, (backgroundPosition + new Vector2(backgroundTexture.Width, 0)), null, Color.White);
            }
            else
            {
                // Draw the texture if still on screen.
                if (backgroundPosition.X > -backgroundTexture.Width)
                    spriteBatch.Draw(backgroundTexture, backgroundPosition, null, Color.White);

                // Draw the texture a second time, behind the first,
                // to create the scrolling illusion.
                spriteBatch.Draw(backgroundTexture, (backgroundPosition + new Vector2(backgroundTexture.Width, 0)), null, Color.White);
            }
        }

        public bool DisplayEnd()
        {
            if (pixelsPassed >= backgroundPixelsToFinishScreen)
                return true;
            else
                return false;
        }

        public bool IsEnd()
        {
            if (DisplayEnd() && backgroundPosition.X < -705 && backgroundPosition.X >  -720)
                return true;
            else
                return false;
        }
    }
}
