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
        public enum GameState { PAUSE, PLAY, MENU, OPTIONS, HELP }
		public GameState gameState = GameState.MENU;
		//=============================================================

		//=============================================================
		//Menu GUI
        GUIControls GUI;
        public float TimerBegin = 0;
        Texture2D Gameguide;
        public Texture2D PlayerIconTexture;
        public Texture2D PlayerIcon2;
        public Texture2D PlayerIcon3;
        public Texture2D PlayerIcon4;
        public Texture2D PlayerIcon1;
        public int PlayerSelect = 1;
        Texture2D clockHud;
        Texture2D pointsHud;
		//=============================================================
        
		//=============================================================
		//Background.
		Background background;
		Texture2D backTexture;
        float secondsToComplete = 60;
        float defaultSpeed = 75;
        float scrollSpeed;
        int TopBoundary = 200;
        int BottomBoundary = 700;
        int OffScreenRightOffset = 300;
		//=============================================================
        
		//=============================================================
		//Timer
        Timer speedTimer;
        public int OptionsClickTime = 0;
		//=============================================================
        
		//=============================================================
		//Player
        Player player;
        public Texture2D playerTexture;
        public Texture2D playerTexture1;
        public Texture2D playerTexture2;
        public Texture2D playerTexture3;
        public Texture2D playerTexture4;
        Texture2D healthTexture;
        Texture2D wifiHudTexture;
        Texture2D[] scoreTextures = new Texture2D[10];
        Texture2D hudSepTexture;
        int WifiNeeded = 5;
        bool highSpeed = false;
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
        List<CollectableSpawnDetail> collectableSpawns = new List<CollectableSpawnDetail>();
        Texture2D collectWifiTexture;
        Texture2D collectPointsTexture;
        Texture2D collectHPLgeTexture;
        Texture2D collectHPSmlTexture;
        Texture2D collectBikeTexture;
        int collectWifiSpawnCount = 10;
        int collectPointsSpawnCount = 25;
        int collectHPSmlSpawnCount = 5;
        int collectHPLgeSpawnCount = 1;
        int collectBikeSpawnCount = 2;
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
            GUI.ButtonMenu1 = Content.Load<Texture2D>("button1");
            GUI.ButtonMenu2 = Content.Load<Texture2D>("button2");
            GUI.ButtonMenu3 = Content.Load<Texture2D>("button3");
            GUI.ButtonMenu4 = Content.Load<Texture2D>("button4");
            GUI.ButtonBack = Content.Load<Texture2D>("buttonback");
            PlayerIcon1 = Content.Load<Texture2D>("PlayerIcon1");
            PlayerIcon2 = Content.Load<Texture2D>("PlayerIcon2");
            PlayerIcon3 = Content.Load<Texture2D>("PlayerIcon3");
            PlayerIcon4 = Content.Load<Texture2D>("PlayerIcon4");
            PlayerIconTexture = PlayerIcon1;
            GUI.ButtonBackward = Content.Load<Texture2D>("Buttonbackwards");
            GUI.ButtonForward = Content.Load<Texture2D>("Buttonnext");
            Gameguide = Content.Load<Texture2D>("Gameguide2");
			//=============================================================

			//=============================================================
            //Player Texturing
            playerTexture1 = Content.Load<Texture2D>("player_green");
            playerTexture2 = Content.Load<Texture2D>("player_blue");
            playerTexture3 = Content.Load<Texture2D>("player_red");
            playerTexture4 = Content.Load<Texture2D>("player_yellow");
            playerTexture = playerTexture1;
			player = new Player(playerTexture, new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), WifiNeeded);
            healthTexture = Content.Load<Texture2D>("health");
            wifiHudTexture = Content.Load<Texture2D>("wifi_hud");
            for (var i = 0; i < 10; i++)
                scoreTextures[i] = Content.Load<Texture2D>("hud_" + i.ToString());
            hudSepTexture = Content.Load<Texture2D>("hud_sep");
            clockHud = Content.Load<Texture2D>("timer_hud3d");
            pointsHud = Content.Load<Texture2D>("points_hud");
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

            AddSpawnCollectables(Collectable.CollectType.WIFI, collectWifiSpawnCount);
            AddSpawnCollectables(Collectable.CollectType.POINTS, collectPointsSpawnCount);
            AddSpawnCollectables(Collectable.CollectType.HPSML, collectHPSmlSpawnCount);
            AddSpawnCollectables(Collectable.CollectType.HPLGE, collectHPLgeSpawnCount);
            AddSpawnCollectables(Collectable.CollectType.BIKE, collectBikeSpawnCount);

            //=============================================================

            //=============================================================
            //Timer
            speedTimer = new Timer(0, 0); //create a speed timer but don't leave it active
            speedTimer.TimerEnd = true;
            //=============================================================
		}

        public void AddSpawnCollectables(Collectable.CollectType SpawnType, int SpawnCount)
        {
            int intSepPos = (int)Math.Ceiling(background.backgroundPixelsToFinishScreen / (SpawnCount + 1));
            for (var i = 1; i <= SpawnCount; i++)
                collectableSpawns.Add(new CollectableSpawnDetail(SpawnType, intSepPos * i));
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
                    TimerBegin += (float)gameTime.ElapsedGameTime.TotalSeconds;
				    MenuUpdate (gameTime);
                    break;
                case GameState.OPTIONS:
                    OptionsUpdate(gameTime);
                    break;
                case GameState.HELP:
                    HelpUpdate(gameTime);
                    break;
                default:
                    TimerBegin += (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            if (TimerBegin >= 0.5) 
                GUI.UpdateCollision(gameTime);
		}

		//=============================================================


        //=============================================================
        //Help Game State Update
        public void OptionsUpdate(GameTime gameTime)
        {            
            GUI.UpdateCollisionNavigation(gameTime);

            switch (PlayerSelect)
            {
                case 1:
                    PlayerIconTexture = PlayerIcon2;
                    player.playerTexture = playerTexture2;
                    break;
                case 2:
                    PlayerIconTexture = PlayerIcon3;
                    player.playerTexture = playerTexture3;
                    break;
                case 3:
                    PlayerIconTexture = PlayerIcon4;
                    player.playerTexture = playerTexture4;
                    break;
                default:
                    PlayerIconTexture = PlayerIcon1;
                    player.playerTexture = playerTexture1;
                    break;
            }

            GUI.UpdateCollisionBack(gameTime);
            TimerBegin = 0;
        }

        //=============================================================


        //=============================================================
        //Option Game State Update
        public void HelpUpdate(GameTime gameTime)
        {
            GUI.UpdateCollisionBack(gameTime);
            TimerBegin = 0;
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
                if (highSpeed)
                {
                    if (speedTimer.TimerEnd)
                        highSpeed = false;
                    else
                        speedTimer.GameTimer(gameTime);
                }

                UpdateSpeed();

                int intHaltMovement = CollisionWithObstacles();

                if (!background.IsEnd())
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

                        if (enemies[i].enemyOffsetPosition.X < -(enemyTexture.Width + 50))
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

                        if (obstacles[i].obstacleOffsetPosition.X < -(obstacleTexture.Width + 50))
                            obstacles.RemoveAt(i);
                    }

                    //do we need to spawn a collectable? do it a little differently
                    //we want these appropriately spaced
                    for (int i = 0; i < collectableSpawns.Count; i++)
                    {
                        if (!collectableSpawns[i].Spawned && background.pixelsPassed > collectableSpawns[i].CollectXPos)
                        {
                            SpawnCollectable(collectableSpawns[i].Type);
                            collectableSpawns[i].Spawned = true;
                        }
                    }

                    for (int i = collectables.Count - 1; i >= 0; i--)
                    {
                        collectables[i].Update(gameTime);

                        if (collectables[i].collectOffsetPosition.X < -(collectables[i].collectTexture.Width + 50))
                            collectables.RemoveAt(i);
                    }

                    CollisionsWithEnemy();
                    CollisionsWithCollectables(gameTime);
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
                case GameState.OPTIONS:
                    OptionsDraw(gameTime);
                    break;
                case GameState.HELP:
                    HelpDraw(gameTime);
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
            spriteBatch.Begin();

            spriteBatch.Draw(backTexture,
                new Rectangle(-200, 0, 2048, 768),
                Color.White);

            //Draw Menu
            GUI.DrawButton(spriteBatch);

            spriteBatch.End();
        }
        //=============================================================

        //=============================================================
        //Pause Game State Update
        public void PauseDraw(GameTime gameTime)
		{
			spriteBatch.Begin ();


			spriteBatch.End();
		}
		//=============================================================



        //=============================================================
        //Options Game State Update
        public void OptionsDraw(GameTime gameTime)
        {

            spriteBatch.Begin();

            spriteBatch.Draw(backTexture, new Rectangle(-200, 0, 2048, 768), Color.White);

            GUI.DrawNavigation(spriteBatch, PlayerIconTexture);

            GUI.DrawBack(spriteBatch);

            spriteBatch.End();
        }
        //=============================================================


        //=============================================================
        //Help Game State Update
        public void HelpDraw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backTexture, new Rectangle(-200, 0, 2048, 768), Color.White);

            spriteBatch.Draw(Gameguide, new Vector2(0, 0), Color.White);

            GUI.DrawBack(spriteBatch);

            spriteBatch.End();
        }
        //=============================================================



		//=============================================================
		//Play Game State Update
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
            Vector2 Pos = new Vector2(graphics.PreferredBackBufferWidth + OffScreenRightOffset, oPos);

            while (!CollisionCheckForSpawn(Pos, offset, obstacleTexture.Width, obstacleTexture.Height))
            {
                if ((Pos.Y + 10) < BottomBoundary)
                    Pos.Y += 10;
                else
                    Pos.Y = TopBoundary;
            }

            Obstacle o = new Obstacle(obstacleTexture, Pos, defaultSpeed);
            o.obstacleScrollSpeed = scrollSpeed;
            obstacles.Add(o);
        }

        void SpawnEnemy()
        {
            float randomY = rand.Next(TopBoundary, BottomBoundary);
            Vector2 offset = new Vector2(enemyTexture.Width, enemyTexture.Height) / 2.0f;
            Vector2 Pos = new Vector2(graphics.PreferredBackBufferWidth + OffScreenRightOffset, randomY);

            while (!CollisionCheckForSpawn(Pos, offset, enemyTexture.Width, enemyTexture.Height))
            {
                if ((Pos.Y + 10) < BottomBoundary)
                    Pos.Y += 10;
                else
                    Pos.Y = TopBoundary;
            }

            Enemy e = new Enemy(enemyTexture, Pos, defaultSpeed);
            e.enemyScrollSpeed = scrollSpeed;
            enemies.Add(e);
        }

        void SpawnCollectable(Collectable.CollectType Type)
        {
            Texture2D collTexture = collectWifiTexture;

            switch (Type)
            {
                case Collectable.CollectType.BIKE:
                    collTexture = collectBikeTexture;
                    break;
                case Collectable.CollectType.POINTS:
                    collTexture = collectPointsTexture;
                    break;
                case Collectable.CollectType.WIFI:
                    collTexture = collectWifiTexture;
                    break;
                case Collectable.CollectType.HPSML:
                    collTexture = collectHPSmlTexture;
                    break;
                case Collectable.CollectType.HPLGE:
                    collTexture = collectHPLgeTexture;
                    break;
            }

            float randomY = rand.Next(TopBoundary, BottomBoundary);
            Vector2 offset = new Vector2(collTexture.Width, collTexture.Height) / 2.0f;
            Vector2 Pos = new Vector2(graphics.PreferredBackBufferWidth + OffScreenRightOffset, randomY);

            while (!CollisionCheckForSpawn(Pos, offset, collTexture.Width, collTexture.Height))
            {
                if ((Pos.Y + 10) < BottomBoundary)
                    Pos.Y += 10;
                else
                    Pos.Y = TopBoundary;
            }

            Collectable c = new Collectable(collTexture, Pos, defaultSpeed, Type);
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

        void CollisionsWithCollectables(GameTime gameTime)
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
                            speedTimer = new Timer(0, 5);
                            highSpeed = true;
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
        //return false is we didn't find a safe space
        public bool CollisionCheckForSpawn(Vector2 Pos, Vector2 Offset, int Width, int Height)
        {
            //first declare a new rect around the item we are trying to build
            //this rect should be on the offset of the item, and 10px larger above/below, and several times the width to make up for fast movement.
            Rectangle testRect;
            Rectangle newRect = new Rectangle((int)(Pos.X - Offset.X) - (Width * 4), (int)(Pos.Y - Offset.Y) - 10, Width * 9, Height + 20);

            //now check to make sure that item won't sit on an enemy
            for (int i = 0; i < enemies.Count; i++)
            {
                testRect = new Rectangle((int)enemies[i].enemyOffsetPosition.X, (int)enemies[i].enemyOffsetPosition.Y, enemies[i].enemyTexture.Width, enemies[i].enemyTexture.Height);
                if (Rectangle.Intersect(newRect, testRect) != Rectangle.Empty)
                    return false;
            }

            //okay, what about an obstacle?
            for (int i = 0; i < obstacles.Count; i++)
            {
                testRect = new Rectangle((int)obstacles[i].obstacleOffsetPosition.X, (int)obstacles[i].obstacleOffsetPosition.Y, obstacles[i].obstacleTexture.Width, obstacles[i].obstacleTexture.Height);
                if (Rectangle.Intersect(newRect, testRect) != Rectangle.Empty)
                    return false;
            }

            //and how about a collectable?
            for (int i = 0; i < collectables.Count; i++)
            {
                testRect = new Rectangle((int)collectables[i].collectOffsetPosition.X, (int)collectables[i].collectOffsetPosition.Y, collectables[i].collectTexture.Width, collectables[i].collectTexture.Height);
                if (Rectangle.Intersect(newRect, testRect) != Rectangle.Empty)
                    return false;
            }

            //if we made it here, we are safe
            return true;
        }

        public void UpdateSpeed()
        {
            //need to set all speeds based on current health
            float currHealth = (float)(player.playerHealth / 100);

            if (highSpeed)
            {
                //special case, treat them like they are at 250% health!
                currHealth = 2.5f;
            }
            else
            {
                //but don't make them go less than 25% speed
                if (currHealth < 0.25)
                    currHealth = 0.25f;
            }

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

            //draw game timer
            Color colTime = Color.White;

            spriteBatch.Draw(clockHud, new Vector2(intScorePosX, intScorePosY), Color.White);
            intScorePosX += clockHud.Width + 5;

            if ((TimerBegin / secondsToComplete) > 0.9)
                colTime = Color.Red;

            char[] TimeValues = GetAsTime((int)TimerBegin).ToCharArray();
            foreach (char t in TimeValues)
            {
                if (t.ToString() == ":")
                {
                    spriteBatch.Draw(hudSepTexture, new Vector2(intScorePosX, intScorePosY), colTime);
                    intScorePosX += hudSepTexture.Width;
                }
                else
                {
                    int intChar = Convert.ToInt16(t.ToString());

                    //draw each char after the other
                    spriteBatch.Draw(scoreTextures[intChar], new Vector2(intScorePosX, intScorePosY), colTime);
                    intScorePosX += scoreTextures[intChar].Width;
                }
            }
            
            intScorePosX = intPosLeft + 220;

            //draw wifi status
            spriteBatch.Draw(wifiHudTexture, new Vector2(intScorePosX, intScorePosY), Color.White);
            intScorePosX += wifiHudTexture.Width + 5;

            Color colWifi = Color.White;
            if (player.playerWifi >= player.playerWifiNeeded)
                colWifi = Color.PaleGreen;
            char[] WifiValues = player.playerWifi.ToString().ToCharArray();
            foreach (char w in WifiValues)
            {
                int intChar = Convert.ToInt16(w.ToString());

                //draw each char after the other
                spriteBatch.Draw(scoreTextures[intChar], new Vector2(intScorePosX, intScorePosY), colWifi);
                intScorePosX += scoreTextures[intChar].Width;
            }
            
            //draw score 
            intScorePosX = intPosLeft + 380;
            spriteBatch.Draw(pointsHud, new Vector2(intScorePosX, intScorePosY), Color.White);
            intScorePosX += pointsHud.Width + 5;
            char[] ScoreValues = player.playerScore.ToString().ToCharArray();
            foreach (char s in ScoreValues)
            {
                int intChar = Convert.ToInt16(s.ToString());

                //draw each char after the other
                spriteBatch.Draw(scoreTextures[intChar], new Vector2(intScorePosX, intScorePosY), Color.White);
                intScorePosX += scoreTextures[intChar].Width;
            }
        }

        public string GetAsTime(int TimeElapsed)
        {
            TimeSpan ts = new TimeSpan(0, 0, TimeElapsed);
            return ts.Minutes.ToString() + ":" + ts.Seconds.ToString("00");
        }

	}
}