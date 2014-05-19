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
			player = new Player(playerTexture, new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2));
			//=============================================================

            //=============================================================
            //Enemy and Obstacle Texture
            enemyTexture = Content.Load<Texture2D>("enemy");
            obstacleTexture = Content.Load<Texture2D>("obstacle");
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

                        if (enemies[i].enemyposition.X < -graphics.PreferredBackBufferWidth)
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

                    CollisionsWithEnemy();
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

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(gameTime, spriteBatch);
            }

            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i].Draw(gameTime, spriteBatch);
            }

			spriteBatch.End();
		}

		//=============================================================

        void SpawnObstacle()
        {
            int randomLane = rand.Next(1, 4);
            float oPos = (randomLane * Obstacle.obstaclePlacementSpace) + Obstacle.obstaclePlacementOffset;
            Obstacle o = new Obstacle(obstacleTexture, new Vector2(graphics.PreferredBackBufferWidth, oPos), defaultSpeed);
            o.obstacleScrollSpeed = scrollSpeed;
            obstacles.Add(o);
        }

        void SpawnEnemy()
        {
            float randomX = rand.Next(1150, 1150);
            float randomY = rand.Next(120, 600);
            Enemy e = new Enemy(enemyTexture, new Vector2(randomX, randomY), defaultSpeed);
            e.enemyScrollSpeed = scrollSpeed;
            enemies.Add(e);
        }

        void CollisionsWithEnemy()
        {
            Rectangle playerRect = new Rectangle((int)player.offsetposition.X, (int)player.offsetposition.Y, player.texture.Width, player.texture.Height);

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Rectangle enemiesRect = new Rectangle((int)(enemies[i].enemyoffsetposition.X), (int)(enemies[i].enemyoffsetposition.Y), enemies[i].enemyTexture.Width, enemies[i].enemyTexture.Height);
                
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

            Rectangle playerRect = new Rectangle((int)player.offsetposition.X, (int)player.offsetposition.Y, player.texture.Width, player.texture.Height);

            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                Rectangle obstaclesRect = new Rectangle((int)(obstacles[i].obstacleOffsetPosition.X), (int)(obstacles[i].obstacleOffsetPosition.Y), obstacles[i].obstacleTexture.Width, obstacles[i].obstacleTexture.Height);

                intersectRect = Rectangle.Intersect(playerRect, obstaclesRect);
                if (intersectRect != Rectangle.Empty)
                {
                    //we have an intersection...will this cause a problem for the player moving forward (1)? Or just up (2)/down(3)?                    
				    if (intersectRect.Right == playerRect.Right && intersectRect.Width <= Math.Abs(player.movementSpeed))
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

        public void UpdateSpeed()
        {
            //need to set all speeds based on current health
            float currHealth = (float)(player.playerHealth / 100);

            //but don't make them go less than 25% speed
            if (currHealth < 0.25)
                currHealth = 0.25f;

            scrollSpeed = defaultSpeed * currHealth;
            player.movementSpeed = player.defaultSpeed * currHealth;
            background.backgroundScrollSpeed = scrollSpeed;

            for (int i = enemies.Count - 1; i >= 0; i--)
                enemies[i].enemyScrollSpeed = enemies[i].enemyDefaultSpeed * currHealth;

            for (int i = obstacles.Count - 1; i >= 0; i--)
                obstacles[i].obstacleScrollSpeed = obstacles[i].obstacleDefaultSpeed * currHealth;
        }
	}
}