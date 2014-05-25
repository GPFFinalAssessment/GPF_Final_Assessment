using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace GPF_Final_Assessment
{
    public class Collectable
    {
        public Texture2D collectTexture;
        public Vector2 collectPosition;
        public Vector2 collectOffset;
        public Vector2 collectOffsetPosition;
        public float collectDefaultSpeed;
        public float collectScrollSpeed;
        public CollectType collectType;
        public enum CollectType { BIKE, POINTS, WIFI, HPSML, HPLGE }
        
        public Collectable(Texture2D texture, Vector2 position, float speed, CollectType type)
        {
            this.collectTexture = texture;
            this.collectPosition = position;
            //we need to calculate the speed of the collectable movement for the same speed of the background
            this.collectDefaultSpeed = speed / 60;
            this.collectScrollSpeed = collectDefaultSpeed;
            this.collectType = type;
            this.collectOffset = new Vector2(collectTexture.Width, collectTexture.Height) / 2.0f;
        }

        public void Update(GameTime gameTime)
        {
            collectPosition.X -= collectScrollSpeed;
            collectOffsetPosition = collectPosition - collectOffset;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(collectTexture, collectOffsetPosition, Color.White);
        }
    }
    

    public class CollectableSpawnDetail
    {
        public Collectable.CollectType Type;
        public int CollectXPos;
        public bool Spawned;

        public CollectableSpawnDetail(Collectable.CollectType collType, int collXPos)
        {
            this.Type = collType;
            this.CollectXPos = collXPos;
            this.Spawned = false;
        }
    }
}

