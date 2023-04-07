using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Devcade;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using BTDevcade;
using System.Linq;


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

		// Menu 
		private int menuSelector;
		private Color[] menuColor;

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
		private float gameSpeed;
		private bool autoStartRound;

		// Game Manager
		private ContentManager contentManager;
		private MonkeyManager monkeyManager;
		private BalloonManager balloonManager;
		
		// Fonts
		private SpriteFont testFont;
		private SpriteFont smallFont;

		// Textures
		private List<Texture2D> towerTextures;
		private Texture2D background;
		private Texture2D singleColor;
		private Texture2D buttonIcon;

		// Background Music
		private Song backgroundMusic;
		private float sfxVolume;
		private float musicVolume;

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
			smallFont = Content.Load<SpriteFont>("smallFont");
			// Game Stats
			gameState = GameState.Menu;
			gameSpeed = 1;
			autoStartRound = false;

			// Keyboard regions
			currentKB = new KeyboardState();
			previousKB = new KeyboardState();

			// sound and music
			sfxVolume = 5;
			musicVolume = 5;

			// Menu
			menuSelector = 0;
			menuColor = new Color[3];
			menuColor[0] = Color.Yellow;
			menuColor[1] = Color.White;
			menuColor[2] = Color.White;

			base.Initialize();

			StartGame();
		}

		/// <summary>
		/// Does any setup prior to the first frame that needs loaded content.
		/// </summary>
		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			singleColor = new Texture2D(GraphicsDevice, 1, 1);
			singleColor.GetData(new Color[] { Color.White });

			backgroundMusic = Content.Load<Song>("BackgroundMusic");
			MediaPlayer.Play(backgroundMusic);
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Volume = musicVolume/10;
			contentManager = new ContentManager(Content.Load<Texture2D>("TX Tileset Grass"), windowTileSize);

			balloonManager = new BalloonManager(new Rectangle(windowTileSize, windowTileSize,
				windowTileSize, windowTileSize),
				Content.Load<Texture2D>("defaultBloon"),
				Content.Load<SoundEffect>("Pop"));
			balloonManager.takeDamage += LoseHealth;
			balloonManager.gainMoney += GainMoney;

			towerTextures = new List<Texture2D>
			{
				Content.Load<Texture2D>("dartMonkey"),
				Content.Load<Texture2D>("TackShooter"),
				Content.Load<Texture2D>("Sniper"),
				Content.Load<Texture2D>("SuperMonkey")
			};
			background = Content.Load<Texture2D>("BackgroundBTD");
			buttonIcon = Content.Load<Texture2D>("buttonIcon");

			monkeyManager = new MonkeyManager(towerTextures,
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

            // Music
            MediaPlayer.Volume = musicVolume / 10;

            // Toggle auto start round
            if (singleKeyPress(Keys.Y)
                || Input.GetButton(1, Input.ArcadeButtons.B2))
			{
				if (autoStartRound)
				{
					autoStartRound = false;
				} else
				{
					autoStartRound= true;
				}
			}
			// Toggle 2X speed
			if (singleKeyPress(Keys.I)
                || Input.GetButton(1, Input.ArcadeButtons.B1))
			{
				gameSpeed = (gameSpeed % 2) + 1;
			} 
			
			// Game Finite state machine
			switch (gameState)
			{
				case GameState.Menu:
					
					// Update selector
					if ((singleKeyPress(Keys.Down)
                        || Input.GetButton(1, Input.ArcadeButtons.StickDown))
						&& menuSelector < 2)
					{
						menuSelector++;
					}
					if ((singleKeyPress(Keys.Up)
                        || Input.GetButton(1, Input.ArcadeButtons.StickUp))
						&& menuSelector > 0)
					{
						menuSelector--;
					}

                    // Update highlighted text
                    for (int i = 0; i < menuColor.Length; i++)
					{
						menuColor[i] = Color.White;
					}
					menuColor[menuSelector] = Color.Yellow;

					// Do something based on what menu button is selected
					switch (menuSelector)
					{
						case 0:
							if (singleKeyPress(Keys.Enter)
                                || Input.GetButton(1, Input.ArcadeButtons.B4))
							{
								gameState = GameState.Game;
								StartGame();
							}
							break;

						case 1:
							if ((singleKeyPress(Keys.Left)
                                || Input.GetButton(1, Input.ArcadeButtons.StickLeft))
                                && sfxVolume > 0)
							{
								sfxVolume--;
							}
							if ((singleKeyPress(Keys.Right)
                                || Input.GetButton(1, Input.ArcadeButtons.StickRight))
                                && sfxVolume < 10)
                            {
                                sfxVolume++;
                            }
                            break;

						case 2:
                            if ((singleKeyPress(Keys.Left) 
								|| Input.GetButton(1, Input.ArcadeButtons.StickLeft))
                                && musicVolume > 0)
                            {
                                musicVolume--;
                            }
                            if ((singleKeyPress(Keys.Right) 
								|| Input.GetButton(1, Input.ArcadeButtons.StickRight))
                                && musicVolume < 10)
                            {
                                musicVolume++;
                            }
                            break;

					}
					break;

				case GameState.Game:
                    balloonManager.Update(gameTime, new Rectangle(0, 0, windowWidth, windowHeight), gameSpeed, sfxVolume);
					monkeyManager.Update(gameTime, new Rectangle(0,0, windowWidth, windowHeight),
						money, balloonManager.Balloons, gameSpeed);
					if (lives <= 0)
					{
						gameState = GameState.GameOver;
					}
                    if (singleKeyPress(Keys.Space)
                        || Input.GetButton(1, Input.ArcadeButtons.Menu))
                    {
                        gameState = GameState.GameOver;
                    }
                    if ((singleKeyPress(Keys.K)
                        || Input.GetButton(1, Input.ArcadeButtons.B3))
                        && !balloonManager.RoundEnded)
                    {
                        if (gameSpeed > 0)
                        {
                            gameSpeed = 0;
                        }
                        else
                        {
                            gameSpeed = 1;
                        }
                    }
                    if (balloonManager.RoundEnded
						&& ((singleKeyPress(Keys.M)
						|| Input.GetButton(1, Input.ArcadeButtons.B4))
						|| autoStartRound))
					{
						round++;
						balloonManager.StartRound(round);
					}
					if (singleKeyPress(Keys.N))
					{
						round++;
					}
					if (singleKeyPress(Keys.B))
					{
						money += 1000;
					}
                    
                    break;

				case GameState.GameOver:
                    if (singleKeyPress(Keys.Enter) 
						|| Input.GetButton(1, Input.ArcadeButtons.B4))
                    {
                        gameState = GameState.Menu;
                    }
                    balloonManager.Update(gameTime, new Rectangle(0, 0, windowWidth, windowHeight), gameSpeed, sfxVolume);
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
					_spriteBatch.Draw(background,
						new Rectangle(0,0,
						windowWidth,windowHeight),
						Color.White);

                    _spriteBatch.Draw(background,
                        new Rectangle((int)(windowWidth * 0.15f),
                        (int)(windowHeight * 0.4f),
                        (int)(windowWidth * 0.7f),
                        (int)(windowHeight * 0.2f)),
                        Color.Black * 0.6f);

					_spriteBatch.DrawString(testFont,
						"Play Game",
						new Vector2(windowWidth * 0.29f,windowHeight * 0.42f),
						menuColor[0]);

                    _spriteBatch.DrawString(testFont,
                        $"Sfx Volume: <{sfxVolume}>",
                        new Vector2(windowWidth * 0.20f, windowHeight * 0.47f),
						menuColor[1]);

                    _spriteBatch.DrawString(testFont,
                        $"Music Volume: <{musicVolume}>",
                        new Vector2(windowWidth * 0.20f, windowHeight * 0.52f),
						menuColor[2]);

                    break;

				case GameState.Game:
					// Call the manager
                    contentManager.Draw(_spriteBatch);
                    balloonManager.Draw(_spriteBatch);
					monkeyManager.Draw(_spriteBatch);

                    DrawText(_spriteBatch);
					DrawButtons(_spriteBatch);

					if (gameSpeed == 0)
					{
                        _spriteBatch.Draw(background,
                        new Rectangle((int)(windowWidth * 0.2f),
                        (int)(windowHeight * 0.45f),
                        (int)(windowWidth * 0.6f),
                        (int)(windowHeight * 0.1f)),
                        Color.Black * 0.4f);

                        _spriteBatch.DrawString(
                            testFont,
                            "PAUSED",
                            new Vector2(windowWidth * 0.37f, windowHeight * 0.48f),
                            Color.White);
                    }
                    break;

				case GameState.GameOver:

                    contentManager.Draw(_spriteBatch);
                    balloonManager.Draw(_spriteBatch);
					monkeyManager.Draw(_spriteBatch);
                    DrawText(_spriteBatch);


					_spriteBatch.Draw(background,
						new Rectangle((int)(windowWidth * 0.2f),
                        (int)(windowHeight * 0.4f),
                        (int)(windowWidth * 0.6f),
                        (int)(windowHeight * 0.2f)),
						Color.Black *0.4f);

					_spriteBatch.DrawString(
						testFont,
						"GAME OVER",
						new Vector2(windowWidth * 0.29f, windowHeight * 0.45f),
						Color.White);

                    _spriteBatch.DrawString(
                        testFont,
                        "HAHA LOSER",
                        new Vector2(windowWidth * 0.28f, windowHeight * 0.5f),
                        Color.White);

                    _spriteBatch.Draw(buttonIcon,
                new Rectangle((int)(windowWidth * 0.80f), (int)(windowHeight * 0.94f),
                (int)(windowHeight * 0.03f), (int)(windowHeight * 0.03f)),
                Color.Purple);

                    _spriteBatch.DrawString(
                        smallFont,
                        $"Return\n" +
                        $"to Menu",
                        new Vector2(windowWidth * 0.87f, windowHeight * 0.942f),
                        Color.LightGoldenrodYellow);
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
			money = 650;
			lives = 150;
			round = 0;
			balloonManager.RemoveAllBalloons();
			balloonManager.LoadRounds();
			monkeyManager.KillAllTowers();
			monkeyManager.DisablePathSpawning(balloonManager.Map1Path);
			autoStartRound = false;
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

		public void DrawButtons(SpriteBatch sb)
		{
			sb.Draw(buttonIcon,
				new Rectangle((int)(windowWidth * 0.05f), (int)(windowHeight * 0.90f),
				(int)(windowHeight * 0.03f), (int)(windowHeight * 0.03f)),
				Color.Red);

            sb.DrawString(
                smallFont,
                $"Dart",
                new Vector2(windowWidth * 0.13f, windowHeight * 0.91f),
                Color.LightGoldenrodYellow);

            sb.Draw(buttonIcon,
                new Rectangle((int)(windowWidth * 0.30f), (int)(windowHeight * 0.90f),
                (int)(windowHeight * 0.03f), (int)(windowHeight * 0.03f)),
                Color.Blue);

            sb.DrawString(
                smallFont,
                $"Tack ",
                new Vector2(windowWidth * 0.38f, windowHeight * 0.91f),
                Color.LightGoldenrodYellow);

            sb.Draw(buttonIcon,
                new Rectangle((int)(windowWidth * 0.55f), (int)(windowHeight * 0.90f),
                (int)(windowHeight * 0.03f), (int)(windowHeight * 0.03f)),
                Color.Green);

            sb.DrawString(
                smallFont,
                $"Sniper",
                new Vector2(windowWidth * 0.63f, windowHeight * 0.91f),
                Color.LightGoldenrodYellow);

            sb.Draw(buttonIcon,
                new Rectangle((int)(windowWidth * 0.80f), (int)(windowHeight * 0.90f),
                (int)(windowHeight * 0.03f), (int)(windowHeight * 0.03f)),
                Color.White);

            sb.DrawString(
                smallFont,
                $"Super ",
                new Vector2(windowWidth * 0.88f, windowHeight * 0.91f),
                Color.LightGoldenrodYellow);

            sb.Draw(buttonIcon,
                new Rectangle((int)(windowWidth * 0.05f), (int)(windowHeight * 0.94f),
                (int)(windowHeight * 0.03f), (int)(windowHeight * 0.03f)),
                Color.Purple);

            sb.DrawString(
                smallFont,
                $"Speed: {gameSpeed}",
                new Vector2(windowWidth * 0.13f, windowHeight * 0.95f),
                Color.LightGoldenrodYellow);

            sb.Draw(buttonIcon,
                new Rectangle((int)(windowWidth * 0.30f), (int)(windowHeight * 0.94f),
                (int)(windowHeight * 0.03f), (int)(windowHeight * 0.03f)),
                Color.Purple);

            sb.DrawString(
                smallFont,
                $"Auto Start:\n{autoStartRound}",
                new Vector2(windowWidth * 0.38f, windowHeight * 0.942f),
                Color.LightGoldenrodYellow);

            sb.Draw(buttonIcon,
                new Rectangle((int)(windowWidth * 0.55f), (int)(windowHeight * 0.94f),
                (int)(windowHeight * 0.03f), (int)(windowHeight * 0.03f)),
                Color.Purple);

            sb.DrawString(
                smallFont,
                $"Place",
                new Vector2(windowWidth * 0.63f, windowHeight * 0.95f),
                Color.LightGoldenrodYellow);

            sb.Draw(buttonIcon,
                new Rectangle((int)(windowWidth * 0.80f), (int)(windowHeight * 0.94f),
                (int)(windowHeight * 0.03f), (int)(windowHeight * 0.03f)),
                Color.Purple);

            sb.DrawString(
                smallFont,
                $"Pause/" +
                $"\nStart",
                new Vector2(windowWidth * 0.88f, windowHeight * 0.942f),
                Color.LightGoldenrodYellow);
        }
    }
}