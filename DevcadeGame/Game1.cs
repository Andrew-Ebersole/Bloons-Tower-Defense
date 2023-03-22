using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;

// Andrew Ebersole
// 2.28.23
// Bloons Tower Defense

namespace DevcadeGame
{
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

		// Textures

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

			gameState = GameState.Menu;
			base.Initialize();
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
				Content.Load<Texture2D>("defaultBloon"));

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

			balloonManager.Update(gameTime, new Rectangle(0,0,windowWidth,windowHeight));
			// TODO: Add your update logic here

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

			contentManager.Draw(_spriteBatch);

			balloonManager.Draw(_spriteBatch);

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}