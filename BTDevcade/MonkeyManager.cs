using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace DevcadeGame
{
internal class MonkeyManager
    {
        // --- Fields --- //

        private List<Monkey> monkeys;
        private enum GameState
        {
            Passive,
            Place,
            Upgrade,
            Selection
        }
        private GameState gameState;
        private Vector2 OverlayPos;
        private int selectedMonkey;

        // Keystates
        private KeyboardState currentKS;
        private KeyboardState previousKS;

        // Textures
        Texture2D monkeyTexture;
        Texture2D circle;

        // Tile size
        float tileSize;

        // Event
        public event LoseResource buyTower;


        // --- Properties --- //





        // --- Constructor --- //

        public MonkeyManager(Texture2D monkeyTexture, float tileSize, Texture2D circle)
        {
            // Game State
            gameState = GameState.Passive;

            // Keyboard States
            currentKS = Keyboard.GetState();
            previousKS = Keyboard.GetState();

            // Window sizes
            OverlayPos = new Vector2(0, 0);
            this.tileSize = tileSize;

            // Lists and array
            monkeys = new List<Monkey>();

            // Textures
            this.monkeyTexture = monkeyTexture;
            this.circle = circle;

            selectedMonkey = -1;
        }



        // --- Methods --- //

        public void Update(GameTime gt, Rectangle windowDimensions, int money, List<Balloons> bloons)
        {
            currentKS = Keyboard.GetState();

            foreach(Monkey m in monkeys)
            {
                m.Update(gt, windowDimensions, bloons);
            }
            switch(gameState)
            {
                case GameState.Passive:
                    if (SingleKeyPress(Keys.Q))
                    {
                        gameState = GameState.Place;
                    }
                    if (SingleKeyPress(Keys.Right))
                    {
                        gameState = GameState.Upgrade;
                        selectedMonkey = 0;
                    }
                    break;

                case GameState.Place:
                    MoveOverlay();

                    // Place monkey
                    if (SingleKeyPress(Keys.Enter))
                    {
                        gameState = GameState.Passive;
                        monkeys.Add(new Monkey(
                            monkeyTexture,
                            circle,
                            (int)((OverlayPos.X + 0.05f) * tileSize),
                            (int)((OverlayPos.Y + 0.05f) * tileSize),
                            (int)(tileSize * 0.9f),
                            (int)(tileSize * 0.9f),
                            1,
                            1,
                            (int)(3 * tileSize),
                            150));
                        buyTower(150); 

                    }
                    break;
                case GameState.Upgrade:

                    if (SingleKeyPress(Keys.Left))
                    {
                        selectedMonkey--;
                    }
                    if (SingleKeyPress(Keys.Right) 
                        && selectedMonkey < monkeys.Count)
                    {
                        selectedMonkey++;
                    }
                    if (selectedMonkey >= monkeys.Count)
                    {
                        selectedMonkey = 0;
                    }
                    if (selectedMonkey == -1)
                    {
                        gameState = GameState.Passive;
                    }
                    break;
                    
            }
            
            previousKS = currentKS;
        }

        public void Draw(SpriteBatch sb)
        {
            foreach(Monkey m in monkeys)
            {
                m.Draw(sb);
            }
            switch (gameState)
            {
                case GameState.Passive:
                    break;

                case GameState.Place:
                    DrawOverlay(sb);

                    sb.Draw(circle,
                        new Rectangle((int)(((OverlayPos.X + 0.05f) * tileSize + (tileSize * 0.9f) / 2) - (3 * tileSize)),
                        (int)(((OverlayPos.Y + 0.05f) * tileSize + (tileSize * 0.9f) / 2) - (3 * tileSize)),
                        (int)(3 * tileSize) * 2, (int)(3 * tileSize) * 2),
                        Color.White * 0.2f);

                    break;

                case GameState.Upgrade:
                    monkeys[selectedMonkey].drawRange(sb);
                    break;
            }
        }

        /// <summary>
        /// Move the overlay before placing
        /// </summary>
        public void MoveOverlay()
        {
            // Change position or sum
            if (SingleKeyPress(Keys.W)
                && OverlayPos.Y > 0)
            {
                OverlayPos.Y -= 1;
            }
            if (SingleKeyPress(Keys.A)
                && OverlayPos.X > 0)
            {
                OverlayPos.X -= 1;
            }
            if (SingleKeyPress(Keys.S)
                && OverlayPos.Y < 28)
            {
                OverlayPos.Y += 1;
            }
            if (SingleKeyPress(Keys.D)
                && OverlayPos.X < 12)
            {
                OverlayPos.X += 1;
            }
        }

        /// <summary>
        /// Draw the overlay before placing tower
        /// </summary>
        /// <param name="sb"></param>
        public void DrawOverlay(SpriteBatch sb)
        {
            sb.Draw(monkeyTexture,                                  // Texture
                new Rectangle((int)((OverlayPos.X+0.05f)*tileSize), // X
                (int)((OverlayPos.Y + 0.05f) * tileSize),           // Y
                (int)(tileSize * 0.9f), (int)(tileSize * 0.9f)),    // Size   
                Color.White * 0.5f);                                // Tint
        }

        /// <summary>
        /// Checks if the key was pressed not held
        /// </summary>
        /// <param name="key"> the key to check </param>
        /// <returns> bool </returns>
        public bool SingleKeyPress(Keys key)
        {
            if(currentKS.IsKeyDown(key) && previousKS.IsKeyUp(key))
            {
                return true;
            }
            return false;
        }

        public void spendMoney(int amount)
        {
            buyTower(amount);
        }

        public void KillAllTowers()
        {
            monkeys.Clear();
        }
    }
}
