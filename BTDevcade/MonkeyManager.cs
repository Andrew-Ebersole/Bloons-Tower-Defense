using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using BTDevcade;
using Devcade;

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
        List<Texture2D> towerTextures;
        Texture2D circle;
        Texture2D dart;

        // Tile size
        float tileSize;

        // Event
        public event LoseResource buyTower;

        // Collision Grid
        private bool[,] canPlace;
         
        // --- Properties --- //





        // --- Constructor --- //

        public MonkeyManager(List<Texture2D> towerTextures, float tileSize, Texture2D circle, Texture2D dart)
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
            this.towerTextures = towerTextures;
            this.circle = circle;
            this.dart = dart;

            selectedMonkey = -1;
            monkeyType = PlaceMonkeyType.Dart;

            // Make sure the towers aren't placed on map
            canPlace = new bool[12, 28];
        }



        // --- Methods --- //

        public void Update(GameTime gt, Rectangle windowDimensions, int money, List<Balloons> balloons, float gameSpeed)
        {
            currentKS = Keyboard.GetState();
            foreach(Monkey m in monkeys)
            {
                m.Update(gt, windowDimensions, balloons, gameSpeed);
            }
            switch (gameState)
            {
                case GameState.Passive:

                    // Place monkey if you have enough money
                    if ((SingleKeyPress(Keys.Q)
                        || Input.GetButtonDown(1, Input.ArcadeButtons.A1))
                        && money >= 200)
                    {
                        gameState = GameState.Place;
                        monkeyType = PlaceMonkeyType.Dart;
                    }
                    if ((SingleKeyPress(Keys.E) 
                        || Input.GetButtonDown(1, Input.ArcadeButtons.A4))
                        && money >= 2500)
                    {
                        gameState = GameState.Place;
                        monkeyType = PlaceMonkeyType.Super;
                    }
                    if ((SingleKeyPress(Keys.R) 
                        || Input.GetButtonDown(1, Input.ArcadeButtons.A3))
                        && money >= 350)
                    {
                        gameState = GameState.Place;
                        monkeyType = PlaceMonkeyType.Sniper;
                    }
                    if ((SingleKeyPress(Keys.T) 
                        || Input.GetButtonDown(1, Input.ArcadeButtons.A2))
                        && money >= 280)
                    {
                        gameState = GameState.Place;
                        monkeyType = PlaceMonkeyType.Tack;
                    }

                    // View range and upgrades
                    if (SingleKeyPress(Keys.Right) 
                        || Input.GetButtonDown(1, Input.ArcadeButtons.StickRight))
                    {
                        gameState = GameState.Upgrade;
                        selectedMonkey = 0;
                    }
                    break;

                case GameState.Place:
                    #region Place Monkeys

                    // Place monkey depending on type of tower
                    switch (monkeyType)
                    {
                        case PlaceMonkeyType.Dart:
                            if (money >= 200)
                            {
                                MoveOverlay();

                                // Place monkey
                                if ((SingleKeyPress(Keys.Enter) || Input.GetButtonDown(1,Input.ArcadeButtons.B3))
                                    && canPlace[(int)OverlayPos.X, (int)OverlayPos.Y])
                                {
                                    gameState = GameState.Passive;
                                    monkeys.Add(new Monkey(
                                        towerTextures[0],  // Monkey Texutre
                                        circle,         // Range Circle texture
                                        dart,           // Projectile Texture
                                        (int)(OverlayPos.X * tileSize + tileSize * 0.45f),   // X
                                        (int)(OverlayPos.Y * tileSize + tileSize * 0.45f),   // Y
                                        (int)(tileSize * 0.9f),                     // Width
                                        (int)(tileSize * 0.9f),                     // Height
                                        1,                  // Damage
                                        1,                  // Attack Speed
                                        (int)(2 * tileSize),// Range
                                        200,                // Cost
                                        2,                  // Pierce
                                        balloons,           // Balloons list
                                        new Vector2((int)(tileSize * 0.9f) * 1.5f, // Orgin X
                                        (int)(tileSize * 0.9f) * 2.0f)));         // Orgin Y
                                    buyTower(200);
                                    canPlace[(int)OverlayPos.X, (int)OverlayPos.Y] = false;
                                }
                            }
                            break;

                        case PlaceMonkeyType.Tack:
                            if (money >= 280)
                            {
                                MoveOverlay();

                                // Place monkey
                                if (SingleKeyPress(Keys.Enter)
                                    && canPlace[(int)OverlayPos.X, (int)OverlayPos.Y])
                                {
                                    gameState = GameState.Passive;
                                    monkeys.Add(new TackShooter(
                                        towerTextures[1],  // Monkey Texutre
                                        circle,         // Range Circle texture
                                        dart,           // Projectile Texture
                                        (int)(OverlayPos.X * tileSize + tileSize * 0.45f),   // X
                                        (int)(OverlayPos.Y * tileSize + tileSize * 0.45f),   // Y
                                        (int)(tileSize * 1f),                     // Width
                                        (int)(tileSize * 1f),                     // Height
                                        1,                  // Damage
                                        0.679f,                  // Attack Speed 0.679f
                                        (int)(1.4375f * tileSize),// Range
                                        280,                // Cost
                                        1,                  // Pierce
                                        balloons,           // Balloons list
                                        new Vector2((int)(tileSize * 1f) * 1.0f, // Orgin X
                                        (int)(tileSize * 1f) * 1.0f)));         // Orgin Y
                                    buyTower(280);
                                    canPlace[(int)OverlayPos.X, (int)OverlayPos.Y] = false;
                                }
                            }
                            break;

                        case PlaceMonkeyType.Sniper:
                            if (money >= 350)
                            {
                                MoveOverlay();

                                // Place monkey
                                if (SingleKeyPress(Keys.Enter)
                                    && canPlace[(int)OverlayPos.X, (int)OverlayPos.Y])
                                {
                                    gameState = GameState.Passive;
                                    monkeys.Add(new Sniper(
                                        towerTextures[2],  // Monkey Texutre
                                        circle,         // Range Circle texture
                                        dart,           // Projectile Texture
                                        (int)(OverlayPos.X*tileSize+tileSize*0.5f),   // X
                                        (int)(OverlayPos.Y * tileSize+tileSize*0.35f),   // Y
                                        (int)(tileSize * 0.9f),                     // Width
                                        (int)(tileSize * 1.5f),                     // Height
                                        2,                  // Damage
                                        0.597f,                  // Attack Speed
                                        (int)(1.25f * tileSize),// Range
                                        350,                // Cost
                                        1,                  // Pierce
                                        balloons,           // Balloons list
                                        new Vector2((int)(tileSize * 1f) * 1.0f, // Orgin X
                                        (int)(tileSize * 1.5f) * 1.0f)));         // Orgin Y
                                    buyTower(350);
                                    canPlace[(int)OverlayPos.X, (int)OverlayPos.Y] = false;
                                }
                            }
                            break;

                        case PlaceMonkeyType.Super:
                            if (money >= 2500)
                            {
                                MoveOverlay();

                                // Place monkey
                                if (SingleKeyPress(Keys.Enter)
                                    && canPlace[(int)OverlayPos.X,(int)OverlayPos.Y])
                                {
                                    gameState = GameState.Passive;
                                    monkeys.Add(new Monkey(
                                        towerTextures[3],  // Monkey Texutre
                                        circle,         // Range Circle texture
                                        dart,           // Projectile Texture
                                        (int)(OverlayPos.X * tileSize + tileSize * 0.5f),   // X
                                        (int)(OverlayPos.Y * tileSize + tileSize * 0.5f),   // Y
                                        (int)(tileSize * 0.9f),                     // Width
                                        (int)(tileSize * 0.9f),                     // Height
                                        1,                  // Damage
                                        21.1f,                  // Attack Speed
                                        (int)(3.1f * tileSize),// Range
                                        2500,                // Cost
                                        1,                  // Pierce
                                        balloons,           // Balloons list
                                        new Vector2 ((int)(tileSize * 0.9f) * 1.0f, // Orgin X
                                        (int)(tileSize * 0.9f) * 1.0f)));         // Orgin Y
                                    buyTower(2500);
                                    canPlace[(int)OverlayPos.X,(int)OverlayPos.Y] = false;
                                }
                            }
                            break;
                    }
                    
                    // Cancel purchase
                    if (SingleKeyPress(Keys.Left))
                    {
                        gameState = GameState.Passive;
                    }
                    #endregion
                    break;

                case GameState.Upgrade:

                    // Change monkey that is selected
                    if (SingleKeyPress(Keys.Left) 
                        || Input.GetButtonDown(1, Input.ArcadeButtons.StickLeft))
                    {
                        selectedMonkey--;
                    }
                    if ((SingleKeyPress(Keys.Right) 
                        || Input.GetButtonDown(1, Input.ArcadeButtons.StickRight)) 
                        && selectedMonkey < monkeys.Count)
                    {
                        selectedMonkey++;
                    }
                    if (selectedMonkey >= monkeys.Count)
                    {
                        selectedMonkey = 0;
                    }
                    if (selectedMonkey < 0)
                    {
                        selectedMonkey = monkeys.Count - 1;
                    }
                    // Exit selected monkey viewing mode
                    if (SingleKeyPress(Keys.Up)
                        || SingleKeyPress(Keys.Down)
                        || Input.GetButtonDown(1, Input.ArcadeButtons.StickUp)
                        || Input.GetButtonDown(1, Input.ArcadeButtons.StickDown))
                    {
                        gameState = GameState.Passive;
                    }
                    break;
                    
            }
            
            previousKS = currentKS;
        }

        public void Draw(SpriteBatch sb)
        {
            // Draw All the monkeys
            foreach(Monkey m in monkeys)
            {
                m.Draw(sb);
            }
            switch (gameState)
            {
                case GameState.Passive:
                    break;

                case GameState.Place:
                    // Draw place overlay to show where to place the tower
                    DrawOverlay(sb);
                    break;

                case GameState.Upgrade:
                    // Draw the range of the selected tower
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
            if ((SingleKeyPress(Keys.W)
                || Input.GetButtonDown(1, Input.ArcadeButtons.StickUp))
                && OverlayPos.Y > 0)
            {
                OverlayPos.Y -= 1;
            }
            if ((SingleKeyPress(Keys.A)
                || Input.GetButtonDown(1, Input.ArcadeButtons.StickLeft))
                && OverlayPos.X > 0)
            {
                OverlayPos.X -= 1;
            }
            if ((SingleKeyPress(Keys.S) 
                || Input.GetButtonDown(1, Input.ArcadeButtons.StickDown))
                && OverlayPos.Y < 27)
            {
                OverlayPos.Y += 1;
            }
            if ((SingleKeyPress(Keys.D) 
                || Input.GetButtonDown(1, Input.ArcadeButtons.StickRight))
                && OverlayPos.X < 11)
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
            Texture2D towerTexture = null;
            float radius = 0;
            Color color = Color.White;

            // Determine monkey type
            switch (monkeyType)
            {
                case PlaceMonkeyType.Dart:
                    towerTexture = towerTextures[0];
                    radius = 2f;
                    break;
                case PlaceMonkeyType.Tack:
                    towerTexture = towerTextures[1];
                    radius = 1.43f;
                    break;
                case PlaceMonkeyType.Sniper:
                    towerTexture = towerTextures[2];
                    radius = 1.25f;
                    break;
                case PlaceMonkeyType.Super:
                    towerTexture = towerTextures[3];
                    radius = 3.1f;
                    break;
            }

            // Determine if can place or not
            if (canPlace[(int)OverlayPos.X, (int)OverlayPos.Y])
            {
                color = Color.White;
            } else
            {
                color = Color.Red;
            }

            // Draw Tower
            sb.Draw(towerTexture,                                  // Texture
                new Rectangle((int)((OverlayPos.X*tileSize) + tileSize * 0.05f), // X
                (int)((OverlayPos.Y * tileSize) + tileSize * 0.05f),           // Y
                (int)(tileSize * 0.9f), (int)(tileSize * 0.9f)),    // Size   
                Color.White * 0.5f);                                // Tint

            // Draw Range
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

        public void DisablePathSpawning(List<Vector2> path)
        {
            // set all values to true
            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 28; y++)
                {
                    canPlace[x, y] = true;
                }
            }

            // Connect every tile in the path with false
            // Vectors in the list are corners so go until you get to the next corner
            int pathPosition = 0;
            Vector2 currentPosition = path[0];
            while (pathPosition < path.Count)
            {
                // Set current tile in the path to false
                Vector2 tilePos = new Vector2(
                    (int)((currentPosition.X - tileSize / 2)/ tileSize),
                    (int)((currentPosition.Y - tileSize / 2)/ tileSize));
                
                if (tilePos.X >= 0 && tilePos.X < 12 && tilePos.Y >= 0 && tilePos.Y < 28)
                {
                    canPlace[(int)tilePos.X, (int)tilePos.Y] = false;
                }


                if (path[pathPosition].X < currentPosition.X)
                {
                    currentPosition.X -= tileSize;
                }
                if (path[pathPosition].Y < currentPosition.Y)
                {
                    currentPosition.Y -= tileSize;
                }
                if (path[pathPosition].X > currentPosition.X)
                {
                    currentPosition.X += tileSize;
                }
                if (path[pathPosition].Y > currentPosition.Y)
                {
                    currentPosition.Y += tileSize;
                }
                if (currentPosition == path[pathPosition])
                {
                    pathPosition++;
                }
            }
        }
    }
}
