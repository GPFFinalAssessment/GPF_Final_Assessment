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
		float secondsToComplete = 10;
		//=============================================================


		//=============================================================
		//Timer
		//=============================================================


		//=============================================================
		//Player
		Player player;
		Texture2D playerTexture;
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

			//=============================================================
			//Background Texturing
			backTexture = Content.Load<Texture2D>("ui_background");
			background = new Background(backTexture, new Vector2(0, 0), secondsToComplete);
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
		public void MenuUpdate(GameTime gametime)
		{
			GUI.UpdateCollision(gametime);
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
				background.Update(gameTime);
				player.Update(gameTime, graphics);
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
		public void MenuDraw(GameTime gametime)
		{
			spriteBatch.Begin ();

			//Draw Menu
			GUI.DrawButton(spriteBatch);


			spriteBatch.End();
		}

		//=============================================================

		//=============================================================
		//Menu Game State Update
		public void PauseDraw(GameTime gametime)
		{
			spriteBatch.Begin ();


			spriteBatch.End();
		}

		//=============================================================


		//=============================================================
		//Menu Game State Update
		public void PlayDraw(GameTime gametime)
		{
			spriteBatch.Begin ();

			//draw background
			background.Draw(spriteBatch);

			//Draw Player
			player.Draw(spriteBatch);

			spriteBatch.End();
		}

		//=============================================================
	}
}