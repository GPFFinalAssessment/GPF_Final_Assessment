using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace GPF_Final_Assessment
{
    class GUIControls
    {

		//=============================================================
        //Text
		// public SpriteFont font;
		//=============================================================

		//=============================================================
		//Menu Wait
		public int TimerBegin = 0;
		//=============================================================

		//=============================================================
		//Button Properties
		public Texture2D ButtonMenu1;
		public Texture2D ButtonMenu2;
		public Texture2D ButtonMenu3;
		public Texture2D ButtonMenu4;
		public Texture2D ButtonBack;
		public Texture2D ButtonForward;
		public Texture2D ButtonBackward;
		public Vector2 buttonSize = new Vector2(512, 128);
		//=============================================================


		//=============================================================
		//Debugger Used for testing button functionallity. Left in due to have implemented in as the spritebatch color draw for easy change of colors.
        public Color Debug1 = Color.White;
        public Color Debug2 = Color.White;
        public Color Debug3 = Color.White;
		public Color Debug4 = Color.White;
		//=============================================================


		//=============================================================
		//Gamestate input
		Game1 game;
		//=============================================================


		public GUIControls(Game1 game)
		{
			this.game = game;
		}

		//Update Main Game
        public void UpdateCollision(GameTime gameTime)
        {
	        //Rectangles for Collision
			Rectangle Button1 = new Rectangle(300, 50, (int)buttonSize.X, (int)buttonSize.Y);
			Rectangle Button2 = new Rectangle(300, 225, (int)buttonSize.X, (int)buttonSize.Y);
			Rectangle Button3 = new Rectangle(300, 425, (int)buttonSize.X, (int)buttonSize.Y);
			Rectangle Button4 = new Rectangle(300, 600, (int)buttonSize.X, (int)buttonSize.Y);
	
	        //Mouse Function For button
	        MouseState stateMouse = Mouse.GetState();
	        Point mousePosition = new Point(stateMouse.X, stateMouse.Y);
	
	        //Collision
	        //Button1
			if (Button1.Contains (mousePosition)) 
			{
                if (stateMouse.LeftButton == ButtonState.Pressed)
                {
                    game.gameState = Game1.GameState.PLAY;
                    game.ResetGameValues();
                    game.TimerBegin = 0;
                }
			}

			//Button2
			if (Button2.Contains (mousePosition)) 
			{
				if (stateMouse.LeftButton == ButtonState.Pressed) 
					game.gameState = Game1.GameState.OPTIONS;	
			}

			//Button3
			if (Button3.Contains (mousePosition)) 
			{
				if (stateMouse.LeftButton == ButtonState.Pressed) 
				    game.gameState = Game1.GameState.HELP;
			}
	
			//Button4
			if (Button4.Contains (mousePosition)) 
			{
				if (stateMouse.LeftButton == ButtonState.Pressed) 
					game.Exit ();	
			}
		}

		//Draw Main Menu
		public void DrawButton(SpriteBatch spriteBatch)
		{
			//Button1
			spriteBatch.Draw(ButtonMenu1, new Rectangle(300, 50, (int)buttonSize.X,(int)buttonSize.Y), Debug1);
			//spriteBatch.DrawString (font, "StartGame", new Vector2 (500,100), Color.Black);

			//Button2
			spriteBatch.Draw(ButtonMenu2, new Rectangle(300, 225, (int)buttonSize.X, (int)buttonSize.Y), Debug2);
			//spriteBatch.DrawString (font, "Options", new Vector2 (510,275), Color.Black);

			//Button3
			spriteBatch.Draw(ButtonMenu3, new Rectangle(300, 425, (int)buttonSize.X, (int)buttonSize.Y), Debug3);
			//spriteBatch.DrawString (font, "Help", new Vector2 (525,475), Color.Black);

			//Button4
			spriteBatch.Draw(ButtonMenu4, new Rectangle(300, 600, (int)buttonSize.X, (int)buttonSize.Y), Debug4);
			//spriteBatch.DrawString (font, "Quit", new Vector2 (525,650), Color.Black);
		}

		public void UpdateCollisionNavigation(GameTime gameTime)
		{
			//Rectangles for Collision
			Rectangle Buttonnext = new Rectangle(950-128, 450, 128, 128);
			Rectangle Buttonback = new Rectangle(150, 450, 128, 128);

			//Mouse Function For button
			MouseState stateMouse = Mouse.GetState();
			Point mousePosition = new Point(stateMouse.X, stateMouse.Y);

            if (((int)gameTime.TotalGameTime.TotalMilliseconds - game.OptionsClickTime) > 250)
            {
                if (Buttonback.Contains(mousePosition))
                {
                    if (stateMouse.LeftButton == ButtonState.Pressed)
                    {
                        game.PlayerSelect--;

                        if (game.PlayerSelect < 0)
                            game.PlayerSelect = 3;

                        game.OptionsClickTime = (int)gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }

                if (Buttonnext.Contains(mousePosition))
                {
                    if (stateMouse.LeftButton == ButtonState.Pressed)
                    {
                        game.PlayerSelect++;

                        if (game.PlayerSelect > 3)
                            game.PlayerSelect = 0;

                        game.OptionsClickTime = (int)gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
            }
		}
			
		//Draw Options
		public void DrawNavigation(SpriteBatch spriteBatch, Texture2D PlayerIconTexture)
		{
			spriteBatch.Draw(ButtonForward, new Rectangle(950-128, 450, 128, 128), Debug4);
			spriteBatch.Draw(ButtonBackward, new Rectangle(150, 450, 128, 128), Debug4);
			spriteBatch.Draw (PlayerIconTexture, new Rectangle (550-128, 250, 256,256), Color.White);
            
			//spriteBatch.DrawString (font, "Quit", new Vector2 (525,650), Color.Black);
		}
        
		//Draw Options
		public void DrawBack(SpriteBatch spriteBatch)
		{		
			spriteBatch.Draw(ButtonBack, new Rectangle(300, 600, (int)buttonSize.X, (int)buttonSize.Y), Debug4);				
		}


		//Update Options
		public void UpdateCollisionBack(GameTime gameTime)
		{
			//Rectangles for Collision
			Rectangle Button1 = new Rectangle(300, 600, (int)buttonSize.X, (int)buttonSize.Y);

			//Mouse Function For button
			MouseState stateMouse = Mouse.GetState();
			Point mousePosition = new Point(stateMouse.X, stateMouse.Y);

			if (Button1.Contains(mousePosition))
			{
				if (stateMouse.LeftButton == ButtonState.Pressed)
					game.gameState = Game1.GameState.MENU;
			}

		}

    }
}
