#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace GPF_Final_Assessment
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		//=============================================================
		//GameState
		public enum GameState { PAUSE, PLAY, MENU }
		public GameState gameState = GameState.MENU;
		//=============================================================

		//=============================================================
		//Menu GUI
		GUIControls GUI;
		//=============================================================
        
		//=============================================================
		//Background.
		Background background;
		Texture2D backTexture;
        float secondsToComplete = 60;
        float defaultSpeed = 50;
        float scrollSpeed;
		//=============================================================
        
		//=============================================================
		//Timer
		//=============================================================
        
		//=============================================================
		//Player
		Player player;
		Texture2D playerTexture;
        Texture2D healthTexture;
        Texture2D wifiHudTexture;
        Texture2D[] scoreTextures = new Texture2D[10];
        int WifiNeeded = 5;
		//=============================================================
        
        //=============================================================
        //Enemies and Obstacles
        Texture2D enemyTexture;
        Texture2D obstacleTexture;
        List<Enemy> enemies = new List<Enemy>();
        List<Obstacle> obstacles = new List<Obstacle>();
        float enemyspawntime = 5.0f;
        float enemyspawncooldown = 0.0f;
        float obstaclespawntime = 12.0f;
        float obstaclespawncooldown = 0.0f;
        Random rand = new Random();
        //=============================================================

        
        //=============================================================
        //Collectables
        List<Collectable> collectables = new List<Collectable>(); 
        Texture2D collectWifiTexture;
        Texture2D collectPointsTexture;
        Texture2D collectHPLgeTexture;
        Texture2D collectHPSmlTexture;
        Texture2D collectBikeTexture;
        float collectspawntime = 5.0f;
        float collectspawncooldown = 0.0f;
        //=============================================================

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			graphics.IsFullScreen = false;
			graphics.PreferredBackBufferHeight = 768;
			graphics.PreferredBackBufferWidth = 1100;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content. Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			//GuiControls
			GUI = new GUIControls(this);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
            scrollSpeed = defaultSpeed;

			//=============================================================
			//Background Texturing
			backTexture = Content.Load<Texture2D>("ui_background");
			background = new Background(backTexture, new Vector2(0, 0), secondsToComplete, scrollSpeed);
			//=============================================================

			//=============================================================
			//Gui Loading
			GUI.ButtonMenu = Content.Load<Texture2D> ("button");
			GUI.font = Content.Load<SpriteFont> ("font");
			//=============================================================

			//=============================================================
			//Player Texturing
			playerTexture = Content.Load<Texture2D>("player_blue");
			player = new Player(playerTexture, new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), WifiNeeded);
            healthTexture = Content.Load<Texture2D>("health");
            wifiHudTexture = Content.Load<Texture2D>("wifi_hud");
            for (var i = 0; i < 10; i++)
                scoreTextures[i] = Content.Load<Texture2D>("hud_" + i.ToString());
			//=============================================================

            //=============================================================
            //Enemy and Obstacle Texture
            enemyTexture = Content.Load<Texture2D>("enemy");
            obstacleTexture = Content.Load<Texture2D>("obstacle");
            //=============================================================

            //=============================================================
            //Collectable Texture
            collectWifiTexture = Content.Load<Texture2D>("wifi");
            collectPointsTexture = Content.Load<Texture2D>("points");
            collectHPLgeTexture = Content.Load<Texture2D>("coffee64");
            collectHPSmlTexture = Content.Load<Texture2D>("coffee32");
            collectBikeTexture = Content.Load<Texture2D>("bike");
            //=============================================================
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here

			//=============================================================
			//Gamestate Update
			switch (gameState)
			{
			    case GameState.PAUSE:
				    PauseUpdate(gameTime);
				    break;
			    case GameState.MENU:
				    MenuUpdate (gameTime);
				    break;
			    default:
				    PlayUpdate(gameTime);
				    break;
			}
			//=============================================================

			base.Update(gameTime);
		}

		//=============================================================
		//Paused Game State Update
		public void PauseUpdate(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Space))
				gameState = GameState.PLAY;
		}
		//=============================================================


		//=============================================================
		//Menu Game State Update
        public void MenuUpdate(GameTime gameTime)
		{
            GUI.UpdateCollision(gameTime);
		}

		//=============================================================


		//=============================================================
		//InGame Game state Update
		public void PlayUpdate(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Space))
				gameState = GameState.PAUSE;
			else
			{
                UpdateSpeed();

                int intHaltMovement = CollisionWithObstacles();

                if(!background.IsEnd())
                    player.Update(gameTime, graphics, intHaltMovement);

                if (intHaltMovement != 1)
                {
                    background.Update(gameTime);

                    enemyspawncooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (enemyspawncooldown < 0.0f)
                    {
                        SpawnEnemy();
                        enemyspawncooldown = enemyspawntime;
                    }

                    for (int i = enemies.Count - 1; i >= 0; i--)
                    {
                        enemies[i].Update(gameTime);

                        if (enemies[i].enemyPosition.X < -graphics.PreferredBackBufferWidth)
                            enemies.RemoveAt(i);
                    }

                    obstaclespawncooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (obstaclespawncooldown < 0.0f)
                    {
                        SpawnObstacle();
                        obstaclespawncooldown = obstaclespawntime;
                    }

                    for (int i = obstacles.Count - 1; i >= 0; i--)
                    {
                        obstacles[i].Update(gameTime);

                        if (obstacles[i].obstaclePosition.X < -graphics.PreferredBackBufferWidth)
                            obstacles.RemoveAt(i);
                    }

                    collectspawncooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (collectspawncooldown < 0.0f)
                    {
                        SpawnCollectable();
                        collectspawncooldown = collectspawntime;
                    }

                    for (int i = collectables.Count - 1; i >= 0; i--)
                    {
                        collectables[i].Update(gameTime);

                        if (collectables[i].collectPosition.X < -graphics.PreferredBackBufferWidth)
                            collectables.RemoveAt(i);
                    }

                    CollisionsWithEnemy();
                    CollisionsWithCollectables();
                }
			}

		}
		//=============================================================

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear (Color.CornflowerBlue);

			// TODO: Add your drawing code here

			//=============================================================
			//Gamestate Update
			switch (gameState) {
			    case GameState.PAUSE:
				    PauseDraw(gameTime);
				    break;
			    case GameState.MENU:
				    MenuDraw (gameTime);
				    break;
			    default:
				    PlayDraw (gameTime);
				    break;
			}

			base.Draw(gameTime);

		}
		//=============================================================

		//=============================================================
		//Menu Game State Update
        public void MenuDraw(GameTime gameTime)
		{
			spriteBatch.Begin ();

			//Draw Menu
            GUI.DrawButton(spriteBatch);

			spriteBatch.End();
		}
		//=============================================================

		//=============================================================
		//Menu Game State Update
        public void PauseDraw(GameTime gameTime)
		{
			spriteBatch.Begin ();


			spriteBatch.End();
		}
		//=============================================================


		//=============================================================
		//Menu Game State Update
        public void PlayDraw(GameTime gameTime)
		{
			spriteBatch.Begin ();

			//draw background
			background.Draw(spriteBatch);

			//Draw Player
            player.Draw(spriteBatch, gameTime);

            //Draw HUD
            DrawHUD();

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(gameTime, spriteBatch);
            }

            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i].Draw(gameTime, spriteBatch);
            }

            for (int i = 0; i < collectables.Count; i++)
            {
                collectables[i].Draw(gameTime, spriteBatch);
            }

			spriteBatch.End();
		}

		//=============================================================

        void SpawnObstacle()
        {
            int randomLane = rand.Next(1, 4);
            float oPos = (randomLane * Obstacle.obstaclePlacementSpace) + Obstacle.obstaclePlacementOffset;
            Vector2 offset = new Vector2(obstacleTexture.Width, obstacleTexture.Height) / 2.0f;
            Vector2 Pos = new Vector2(graphics.PreferredBackBufferWidth + 100, oPos) - offset;

            if (CollisionCheckForSpawn(Pos, obstacleTexture.Width, obstacleTexture.Height))
            {
                randomLane = rand.Next(1, 4);
                Pos.Y = ((randomLane * Obstacle.obstaclePlacementSpace) + Obstacle.obstaclePlacementOffset) - offset.Y;

                while (CollisionCheckForSpawn(Pos, obstacleTexture.Width, obstacleTexture.Height))
                {
                    if ((Pos.Y + 10) < (600 - offset.Y))
                        Pos.Y += 10;
                    else
                        Pos.Y = 150 - offset.Y;
                }
            }

            Obstacle o = new Obstacle(obstacleTexture, new Vector2(Pos.X, Pos.Y), defaultSpeed);
            o.obstacleScrollSpeed = scrollSpeed;
            obstacles.Add(o);
        }

        void SpawnEnemy()
        {
            float randomY = rand.Next(150, 600);
            Vector2 offset = new Vector2(enemyTexture.Width, enemyTexture.Height) / 2.0f;
            Vector2 Pos = new Vector2(graphics.PreferredBackBufferWidth + 100, randomY) - offset;

            while (CollisionCheckForSpawn(Pos, enemyTexture.Width, enemyTexture.Height))
                Pos.Y = rand.Next(150, 600) - offset.Y;

            Enemy e = new Enemy(enemyTexture, new Vector2(Pos.X, Pos.Y), defaultSpeed);
            e.enemyScrollSpeed = scrollSpeed;
            enemies.Add(e);
        }

        void SpawnCollectable()
        {
            int randType = rand.Next(0, 4);
            Texture2D randTexture = collectWifiTexture;

            switch (randType)
            {
                case 0:
                    randTexture = collectBikeTexture;
                    break;
                case 1:
                    randTexture = collectPointsTexture;
                    break;
                case 2:
                    randTexture = collectWifiTexture;
                    break;
                case 3:
                    randTexture = collectHPSmlTexture;
                    break;
                case 4:
                    randTexture = collectHPLgeTexture;
                    break;
            }

            float randomY = rand.Next(150, 600);
            Vector2 offset = new Vector2(randTexture.Width, randTexture.Height) / 2.0f;
            Vector2 Pos = new Vector2(graphics.PreferredBackBufferWidth + 100, randomY) - offset;

            while (CollisionCheckForSpawn(Pos, randTexture.Width, randTexture.Height))
                Pos.Y = rand.Next(150, 600) - offset.Y;

            Collectable c = new Collectable(randTexture, new Vector2(Pos.X, Pos.Y), defaultSpeed, randType);
            c.collectScrollSpeed = scrollSpeed;
            collectables.Add(c);
        }

        void CollisionsWithEnemy()
        {
            Rectangle playerRect = new Rectangle((int)player.playerOffsetPosition.X, (int)player.playerOffsetPosition.Y, player.playerTexture.Width, player.playerTexture.Height);

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Rectangle enemiesRect = new Rectangle((int)(enemies[i].enemyOffsetPosition.X), (int)(enemies[i].enemyOffsetPosition.Y), enemies[i].enemyTexture.Width, enemies[i].enemyTexture.Height);
                
                if (Rectangle.Intersect(playerRect, enemiesRect) != Rectangle.Empty)
                {
                    //this should have hurt our player...take away 5 points
                    enemies.RemoveAt(i);
                    player.UpdateHealth(10, false);
                    break;
                }
            }
        }

        //if we collide with an obstacle all movement halts until the player has moved around it
        public int CollisionWithObstacles()
        {
            Rectangle intersectRect;

            Rectangle playerRect = new Rectangle((int)player.playerOffsetPosition.X, (int)player.playerOffsetPosition.Y, player.playerTexture.Width, player.playerTexture.Height);

            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                Rectangle obstaclesRect = new Rectangle((int)(obstacles[i].obstacleOffsetPosition.X), (int)(obstacles[i].obstacleOffsetPosition.Y), obstacles[i].obstacleTexture.Width, obstacles[i].obstacleTexture.Height);

                intersectRect = Rectangle.Intersect(playerRect, obstaclesRect);
                if (intersectRect != Rectangle.Empty)
                {
                    //we have an intersection...will this cause a problem for the player moving forward (1)? Or just up (2)/down(3)?                    
				    if (intersectRect.Right == playerRect.Right && intersectRect.Width <= Math.Abs(player.playerMovementSpeed))
                        return 1;
                    else if(intersectRect.Top == playerRect.Top)
                        return 2;
                    else if(intersectRect.Bottom == playerRect.Bottom)
                        return 3;
                    else
                        return 0;
                }
            }

            return 0;
        }

        void CollisionsWithCollectables()
        {
            Rectangle playerRect = new Rectangle((int)player.playerOffsetPosition.X, (int)player.playerOffsetPosition.Y, player.playerTexture.Width, player.playerTexture.Height);

            for (int i = collectables.Count - 1; i >= 0; i--)
            {
                Rectangle collectRect = new Rectangle((int)(collectables[i].collectOffsetPosition.X), (int)(collectables[i].collectOffsetPosition.Y), collectables[i].collectTexture.Width, collectables[i].collectTexture.Height);

                if (Rectangle.Intersect(playerRect, collectRect) != Rectangle.Empty)
                {
                    //figure out what this does for our player
                    switch (collectables[i].collectType)
                    {
                        case Collectable.CollectType.BIKE:

                            break;
                        case Collectable.CollectType.HPLGE:                           
                            player.UpdateHealth(50, true);
                            break;
                        case Collectable.CollectType.HPSML:
                            player.UpdateHealth(20, true);
                            break;
                        case Collectable.CollectType.WIFI:
                            player.playerWifi += 1;
                            break;
                        default:
                            //assume points!
                            player.playerScore += 25;
                            break;

                    }

                    collectables.RemoveAt(i);
                    break;
                }
            }
        }

        //we need to run a full collision check here to make sure the placement is okay for the next item to spawn
        public bool CollisionCheckForSpawn(Vector2 Pos, int Width, int Height)
        {
            //we want to make sure there's nothing that the new item will hit within 5px of it
            Rectangle newRect = new Rectangle((int)Pos.X - 5, (int)Pos.Y - 5, Width + 5, Height + 5);
            
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Rectangle enemiesRect = new Rectangle((int)(enemies[i].enemyOffsetPosition.X), (int)(enemies[i].enemyOffsetPosition.Y), enemies[i].enemyTexture.Width, enemies[i].enemyTexture.Height);

                if (Rectangle.Intersect(newRect, enemiesRect) != Rectangle.Empty)
                    return true;
            }

            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                Rectangle obstaclesRect = new Rectangle((int)(obstacles[i].obstacleOffsetPosition.X), (int)(obstacles[i].obstacleOffsetPosition.Y), obstacles[i].obstacleTexture.Width, obstacles[i].obstacleTexture.Height);

                if (Rectangle.Intersect(newRect, obstaclesRect) != Rectangle.Empty)
                    return true;
            }

            for (int i = collectables.Count - 1; i >= 0; i--)
            {
                Rectangle collectablesRect = new Rectangle((int)(collectables[i].collectOffsetPosition.X), (int)(collectables[i].collectOffsetPosition.Y), collectables[i].collectTexture.Width, collectables[i].collectTexture.Height);

                if (Rectangle.Intersect(newRect, collectablesRect) != Rectangle.Empty)
                    return true;
            }

            return false;
        }

        public void UpdateSpeed()
        {
            //need to set all speeds based on current health
            float currHealth = (float)(player.playerHealth / 100);

            //but don't make them go less than 25% speed
            if (currHealth < 0.25)
                currHealth = 0.25f;

            scrollSpeed = defaultSpeed * currHealth;
            player.playerMovementSpeed = player.playerDefaultSpeed * currHealth;
            background.backgroundScrollSpeed = scrollSpeed;

            for (int i = enemies.Count - 1; i >= 0; i--)
                enemies[i].enemyScrollSpeed = enemies[i].enemyDefaultSpeed * currHealth;

            for (int i = obstacles.Count - 1; i >= 0; i--)
                obstacles[i].obstacleScrollSpeed = obstacles[i].obstacleDefaultSpeed * currHealth;

            for (int i = collectables.Count - 1; i >= 0; i--)
                collectables[i].collectScrollSpeed = collectables[i].collectDefaultSpeed * currHealth;
        }

        public void DrawHUD()
        {
            float intPosLeft = 12;
            float intScorePosX = intPosLeft;
            float intScorePosY = 60;

            //draw healthbar
            float currHealth = (float)(player.playerHealth / 100);
            int intWidth = (int)Math.Ceiling((decimal)((healthTexture.Width - 32) * currHealth)) + 32;
            spriteBatch.Draw(healthTexture, new Vector2(12, 20), new Rectangle(0, 0, intWidth, 32), Color.White);

            //draw score 
            char[] ScoreValues = player.playerScore.ToString().ToCharArray();
            foreach (char s in ScoreValues)
            {
                int intChar = Convert.ToInt16(s.ToString());

                //draw each char after the other
                spriteBatch.Draw(scoreTextures[intChar], new Vector2(intScorePosX, intScorePosY), Color.White);
                intScorePosX += scoreTextures[intChar].Width;
            }

            //draw wifi status
            spriteBatch.Draw(wifiHudTexture, new Vector2(intPosLeft, 105), Color.White);
            spriteBatch.Draw(scoreTextures[player.playerWifi], new Vector2(intPosLeft + wifiHudTexture.Width + 5, 105), Color.White);

            //draw game timer
        }

	}
}