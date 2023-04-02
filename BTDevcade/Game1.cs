using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Devcade;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


// Andrew Ebersole
// Started 2.28.23
// Bloons Tower Defense

namespace DevcadeGame
{
    // Delegates
    public delegate void LoseResource(int amount);

    public class Game1 : Game
	{
        #region fields
        private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		// GameState
		private enum GameState
        {
			Menu,
			Game,
			GameOver,
			Instructions,
			Credits
        }
		GameState gameState;

		// Keyboard Input
		private KeyboardState currentKB;
		private KeyboardState previousKB;

		// Window size
		private int windowWidth;
		private int windowHeight;
		private int windowTileSize;

		// Game Fields
		private int round;
		private int money;
		private int lives;

		// Game Manager
		private ContentManager contentManager;
		private MonkeyManager monkeyManager;
		private BalloonManager balloonManager;
		
		// Fonts
		private SpriteFont testFont;

		

		#endregion
		/// <summary>
		/// Game constructor
		/// </summary>
		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = false;
		}

		/// <summary>
		/// Does any setup prior to the first frame that doesn't need loaded content.
		/// </summary>
		protected override void Initialize()
		{
			Input.Initialize(); // Sets up the input library

			// Set window size if running debug (in release it will be fullscreen)
			#region
#if DEBUG
			_graphics.PreferredBackBufferWidth = 420;
			_graphics.PreferredBackBufferHeight = 980;
			_graphics.ApplyChanges();
#else
			_graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
			_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
			_graphics.ApplyChanges();
#endif
			#endregion
			
			windowHeight = _graphics.PreferredBackBufferHeight;
			windowWidth = _graphics.PreferredBackBufferWidth;
			windowTileSize = windowWidth / 12;

			// Font
			testFont = Content.Load<SpriteFont>("testFont");

			gameState = GameState.Menu;

			currentKB = new KeyboardState();
			previousKB = new KeyboardState();

			base.Initialize();

			StartGame();
		}

		/// <summary>
		/// Does any setup prior to the first frame that needs loaded content.
		/// </summary>
		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
	
			contentManager = new ContentManager(Content.Load<Texture2D>("TX Tileset Grass"), windowTileSize);

            balloonManager = new BalloonManager(new Rectangle(windowTileSize,windowTileSize,
				windowTileSize,windowTileSize),
				Content.Load<Texture2D>("defaultBloon"),
				Content.Load<SoundEffect>("Pop"));
			balloonManager.takeDamage += LoseHealth;
			balloonManager.gainMoney += GainMoney;

			monkeyManager = new MonkeyManager(Content.Load<Texture2D>("dartMonkey"),
				windowTileSize, 
				Content.Load<Texture2D>("circle"),
				Content.Load<Texture2D>("dart"));
			monkeyManager.buyTower += UseMoney;

        }

        /// <summary>
        /// Your main update loop. This runs once every frame, over and over.
        /// </summary>
        /// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
        protected override void Update(GameTime gameTime)
		{
			Input.Update(); // Updates the state of the input library

			// Exit when both menu buttons are pressed (or escape for keyboard debuging)
			// You can change this but it is suggested to keep the keybind of both menu
			// buttons at once for gracefull exit.
			if (Keyboard.GetState().IsKeyDown(Keys.Escape) ||
				(Input.GetButton(1, Input.ArcadeButtons.Menu) &&
				Input.GetButton(2, Input.ArcadeButtons.Menu)))
			{
				Exit();
			}
			currentKB = Keyboard.GetState();

			switch (gameState)
			{
				case GameState.Menu:
					if (singleKeyPress(Keys.Enter))
					{
						StartGame();
						gameState = GameState.Game;
					}
					break;

				case GameState.Game:
                    balloonManager.Update(gameTime, new Rectangle(0, 0, windowWidth, windowHeight));
					monkeyManager.Update(gameTime, new Rectangle(0,0, windowWidth, windowHeight),
						money, balloonManager.Balloons);

					if (lives <= 0)
					{
						gameState = GameState.GameOver;
					}
                    if (singleKeyPress(Keys.Space))
                    {
                        gameState = GameState.GameOver;
						monkeyManager.KillAllTowers();
                    }
					if (balloonManager.RoundEnded
						&& singleKeyPress(Keys.M))
					{
						round++;
						balloonManager.StartRound(round);
					}
                    break;

				case GameState.GameOver:
                    if (singleKeyPress(Keys.Enter))
                    {
                        gameState = GameState.Menu;
                    }
                    break;
			}

			previousKB = Keyboard.GetState();
			base.Update(gameTime);
		}

		/// <summary>
		/// Your main draw loop. This runs once every frame, over and over.
		/// </summary>
		/// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin();

			switch (gameState)
			{
				case GameState.Menu:
					_spriteBatch.DrawString(
						testFont,
						"Press enter to start game",
						new Vector2(1, 1),
						Color.Black);
					break;

				case GameState.Game:
					// Call the manager
                    contentManager.Draw(_spriteBatch);
                    balloonManager.Draw(_spriteBatch);
					monkeyManager.Draw(_spriteBatch);

                    DrawText(_spriteBatch);
                    break;

				case GameState.GameOver:
                    contentManager.Draw(_spriteBatch);
                    balloonManager.Draw(_spriteBatch);
                    DrawText(_spriteBatch);
					_spriteBatch.DrawString(
						testFont,
						"GAME OVER!",
						new Vector2(windowWidth * 0.2f, windowHeight * 0.4f),
						Color.Red);
                    break;
			}
			


			_spriteBatch.End();

			base.Draw(gameTime);
		}

		public void DrawText(SpriteBatch sb)
		{
			sb.DrawString(
				testFont,
				$"${money}" +
				$"\nRound: {round}" +
				$"\nLives: {lives}",
				new Vector2(windowTileSize*3.2f,windowTileSize*0.1f),
				Color.LightGoldenrodYellow);
		}

		private void StartGame()
		{
			money = 550;
			lives = 150;
			round = 0;
			balloonManager.RemoveAllBalloons(); 
		}

        public void LoseHealth(int amount)
        {
			lives -= amount;
        }

		public void UseMoney(int amount)
		{
			money -= amount;
		}

		public void GainMoney(int amount)
		{
			money += amount;
		}

		public bool singleKeyPress(Keys key)
		{
			if (currentKB.IsKeyDown(key) && previousKB.IsKeyUp(key))
			{
				return true;
			}
			return false;
		}
    }
}