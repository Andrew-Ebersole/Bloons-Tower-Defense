using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;

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
        private enum PlaceMonkeyType
        {
            Dart,
            Tack,
            Sniper,
            Super
        }
        private PlaceMonkeyType monkeyType; // like the game evan plays
        private Vector2 OverlayPos;
        private int selectedMonkey;

        // Keystates
        private KeyboardState currentKS;
        private KeyboardState previousKS;
        
        // Textures
        Texture2D monkeyTexture;
        Texture2D circle;
        Texture2D dart;

        // Tile size
        float tileSize;

        // Event
        public event LoseResource buyTower;

        // --- Properties --- //





        // --- Constructor --- //

        public MonkeyManager(Texture2D monkeyTexture, float tileSize, Texture2D circle, Texture2D dart)
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
            this.dart = dart;

            selectedMonkey = -1;
            monkeyType = PlaceMonkeyType.Dart;
        }



        // --- Methods --- //

        public void Update(GameTime gt, Rectangle windowDimensions, int money, List<Balloons> balloons)
        {
            currentKS = Keyboard.GetState();

            foreach(Monkey m in monkeys)
            {
                m.Update(gt, windowDimensions, balloons);
            }
            switch(gameState)
            {
                case GameState.Passive:
                    if (SingleKeyPress(Keys.Q))
                    {
                        gameState = GameState.Place;
                        monkeyType = PlaceMonkeyType.Dart;
                    }
                    if (SingleKeyPress(Keys.E))
                    {
                        gameState = GameState.Place;
                        monkeyType = PlaceMonkeyType.Super;
                    }
                    if (SingleKeyPress(Keys.Right))
                    {
                        gameState = GameState.Upgrade;
                        selectedMonkey = 0;
                    }
                    break;

                case GameState.Place:
                    #region Place Monkeys

                    switch (monkeyType)
                    {
                        case PlaceMonkeyType.Dart:
                            if (money >= 200)
                            {
                                MoveOverlay();

                                // Place monkey
                                if (SingleKeyPress(Keys.Enter))
                                {
                                    gameState = GameState.Passive;
                                    monkeys.Add(new Monkey(
                                        monkeyTexture,  // Monkey Texutre
                                        circle,         // Range Circle texture
                                        dart,           // Projectile Texture
                                        (int)((OverlayPos.X + 0.05f) * tileSize),   // X
                                        (int)((OverlayPos.Y + 0.05f) * tileSize),   // Y
                                        (int)(tileSize * 0.9f),                     // Width
                                        (int)(tileSize * 0.9f),                     // Height
                                        1,                  // Damage
                                        1,                  // Attack Speed
                                        (int)(2 * tileSize),// Range
                                        200,                // Cost
                                        2,                  // Pierce
                                        balloons));         // Balloons List
                                    buyTower(200);

                                }
                            }
                            break;

                        case PlaceMonkeyType.Super:
                            if (money >= 2500)
                            {
                                MoveOverlay();

                                // Place monkey
                                if (SingleKeyPress(Keys.Enter))
                                {
                                    gameState = GameState.Passive;
                                    monkeys.Add(new Monkey(
                                        monkeyTexture,  // Monkey Texutre
                                        circle,         // Range Circle texture
                                        dart,           // Projectile Texture
                                        (int)((OverlayPos.X + 0.05f) * tileSize),   // X
                                        (int)((OverlayPos.Y + 0.05f) * tileSize),   // Y
                                        (int)(tileSize * 0.9f),                     // Width
                                        (int)(tileSize * 0.9f),                     // Height
                                        1,                  // Damage
                                        21.1f,                  // Attack Speed
                                        (int)(3.1f * tileSize),// Range
                                        2500,                // Cost
                                        1,                  // Pierce
                                        balloons));         // Balloons List
                                    buyTower(2500);

                                }
                            }
                            break;
                    }
                    
                    if (SingleKeyPress(Keys.Left))
                    {
                        gameState = GameState.Passive;
                    }
                    #endregion
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
                    switch (monkeyType)
                    {
                        case PlaceMonkeyType.Dart:
                            DrawOverlay(sb, 2, Color.White);
                            break;
                        case PlaceMonkeyType.Super:
                            DrawOverlay(sb, 3.1f, Color.Red);
                            break;
                    }

                    break;

                case GameState.Upgrade:
                    if (monkeys.Count > selectedMonkey)
                    {
                        monkeys[selectedMonkey].drawRange(sb);
                    }
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
                && OverlayPos.Y < 27)
            {
                OverlayPos.Y += 1;
            }
            if (SingleKeyPress(Keys.D)
                && OverlayPos.X < 11)
            {
                OverlayPos.X += 1;
            }
        }

        /// <summary>
        /// Draw the overlay before placing tower
        /// </summary>
        /// <param name="sb"></param>
        public void DrawOverlay(SpriteBatch sb, double radius, Color color)
        {
            sb.Draw(monkeyTexture,                                  // Texture
                new Rectangle((int)((OverlayPos.X+0.05f)*tileSize), // X
                (int)((OverlayPos.Y + 0.05f) * tileSize),           // Y
                (int)(tileSize * 0.9f), (int)(tileSize * 0.9f)),    // Size   
                Color.White * 0.5f);                                // Tint

            sb.Draw(circle,
                        new Rectangle(
                            (int)(((OverlayPos.X + 0.05f) * tileSize + (tileSize * 0.9f) / 2) - (radius * tileSize)),    // Y
                            (int)(((OverlayPos.Y + 0.05f) * tileSize + (tileSize * 0.9f) / 2) - (radius * tileSize)),    // X
                            (int)(radius * tileSize) * 2, (int)(radius * tileSize) * 2),                                      // Radius
                            color * 0.2f);                                                                    // Tint
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
