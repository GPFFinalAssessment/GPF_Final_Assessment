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
        public SpriteFont font;
		//=============================================================


		//=============================================================
        //Button Properties
        public Texture2D ButtonMenu;
		public Vector2 buttonSize = new Vector2(512, 128);
		//=============================================================


		//=============================================================
		//Debugger Used for testing button functionallity.
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
	        if (Button1.Contains(mousePosition))
	        {
	            if (stateMouse.LeftButton == ButtonState.Pressed)
	            {
	                Debug1 = Color.Bisque;

					game.gameState = Game1.GameState.PLAY;

	            }
	
	
	        }
	        //Button2
	        if (Button2.Contains(mousePosition))
	        {
	            if (stateMouse.LeftButton == ButtonState.Pressed)
	            {
	                Debug2 = Color.Bisque;

	            }
	
	        }
	        //Button3
	        if (Button3.Contains(mousePosition))
	        {
	            if (stateMouse.LeftButton == ButtonState.Pressed)
	            {
	                Debug3 = Color.Bisque;
	            }
	
	        }
	
			//Button4
			if (Button4.Contains(mousePosition))
			{
				if (stateMouse.LeftButton == ButtonState.Pressed)
				{
					Debug4 = Color.Bisque;

					game.Exit ();
				}
	
			}

        }
			

        //Draw Game
		public void DrawButton(SpriteBatch spriteBatch)
        {

           //Button1
           spriteBatch.Draw(ButtonMenu,
				new Rectangle(300, 50, (int)buttonSize.X,(int)buttonSize.Y),
            		Debug1);
			spriteBatch.DrawString (font, "StartGame", new Vector2 (500,100), Color.Black);

           //Button2
           spriteBatch.Draw(ButtonMenu,
				new Rectangle(300, 225, (int)buttonSize.X, (int)buttonSize.Y),
           		Debug2);
			spriteBatch.DrawString (font, "Options", new Vector2 (510,275), Color.Black);

           //Button3
           spriteBatch.Draw(ButtonMenu,
				new Rectangle(300, 425, (int)buttonSize.X, (int)buttonSize.Y),
           		Debug3);
			spriteBatch.DrawString (font, "Help", new Vector2 (525,475), Color.Black);

			//Button4
			spriteBatch.Draw(ButtonMenu,
				new Rectangle(300, 600, (int)buttonSize.X, (int)buttonSize.Y),
				Debug4);
			spriteBatch.DrawString (font, "Quit", new Vector2 (525,650), Color.Black);
        }



    }
}
