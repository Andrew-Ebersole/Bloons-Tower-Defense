using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Audio;
using System.Reflection.Metadata;

namespace DevcadeGame
{
    internal class BalloonManager
    {
        // --- Fields --- //

        
        private enum balloonType
        {
            Red,
            Blue,
            Green,
            Yellow,
        }
        private List<Vector2> Map1path;
        private List<Balloons> balloons;
        private Rectangle tileSize;
        private KeyboardState currentKB;
        private KeyboardState previousKB;
        private Texture2D balloonTexture;
        public event TakeDamage takeDamage;
        private SoundEffect pop;



        // --- Properties --- //






        // --- Constructor --- //

        /// <summary>
        /// Initialize the balloon manager
        /// </summary>
        /// <param name="tileSize"> size of one tile grid </param>
        /// <param name="balloons"> balloon texture </param>
        public BalloonManager(Rectangle tileSize,Texture2D balloons, SoundEffect pop)
        {
            this.balloons = new List<Balloons>();
            this.tileSize = tileSize;
            previousKB = new KeyboardState();
            currentKB = new KeyboardState();
            balloonTexture = balloons;
            initilizePath();
            this.pop = pop;
        }



        // --- Methods --- //

        /// <summary>
        /// Update all the balloons and check for keyboard input
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="window"> dimensions of the screen </param>
            public void Update(GameTime gt, Rectangle window)
        {
            //Keyboard input
            currentKB = Keyboard.GetState();

            #region manual controls
            // Manually Spawn Balloons
            if (currentKB.IsKeyDown(Keys.D1) && previousKB.IsKeyUp(Keys.D1))
            {
                balloons.Add(new Balloons(
                    balloonTexture,0,0,26*window.Width/420,30 * window.Width / 420,
                    1,
                    Map1path,
                    pop));
                balloons[balloons.Count - 1].takeDamage += TakeDamage;
            }
            if (currentKB.IsKeyDown(Keys.D2) && previousKB.IsKeyUp(Keys.D2))
            {
                balloons.Add(new Balloons(
                    balloonTexture, 0, 0, 26 * window.Width / 420, 30 * window.Width / 420,
                    2,
                    Map1path,
                    pop));
                balloons[balloons.Count - 1].takeDamage += TakeDamage;
            }
            if (currentKB.IsKeyDown(Keys.D3) && previousKB.IsKeyUp(Keys.D3))
            {
                balloons.Add(new Balloons(
                    balloonTexture, 0, 0, 26 * window.Width / 420, 30 * window.Width / 420,
                    3,
                    Map1path,
                    pop));
                balloons[balloons.Count - 1].takeDamage += TakeDamage;
            }
            if (currentKB.IsKeyDown(Keys.D4) && previousKB.IsKeyUp(Keys.D4))
            {
                balloons.Add(new Balloons(
                    balloonTexture, 0, 0, 26 * window.Width / 420, 30 * window.Width / 420,
                    4,
                    Map1path,
                    pop));
                balloons[balloons.Count - 1].takeDamage += TakeDamage;
            }
            if (currentKB.IsKeyDown(Keys.D5) && previousKB.IsKeyUp(Keys.D5))
            {
                balloons.Add(new Balloons(
                    balloonTexture, 0, 0, 26 * window.Width / 420, 30 * window.Width / 420,
                    5,
                    Map1path,
                    pop));
                balloons[balloons.Count - 1].takeDamage += TakeDamage;
            }

            // Manually Pop Balloons
            if (currentKB.IsKeyDown(Keys.P) && previousKB.IsKeyUp(Keys.P))
            {
                if (balloons.Count > 0)
                {
                    Balloons first = balloons[0];
                    foreach (Balloons b in  balloons)
                    {
                        if (b.DistanceTraveled > first.DistanceTraveled)
                        {
                            first = b;
                        }
                    }
                    first.Damage(1);
                }
            }
            if (currentKB.IsKeyDown(Keys.U) && previousKB.IsKeyUp(Keys.U))
            {
                if (balloons.Count > 0)
                {
                    Balloons first = balloons[0];
                    foreach (Balloons b in balloons)
                    {
                        if (b.DistanceTraveled > first.DistanceTraveled)
                        {
                            first = b;
                        }
                    }
                    first.Damage(3);
                }
            }
            #endregion

            // Remove popped balloons
            int removedBalloons = 0;
            for (int i = 0; i < balloons.Count; i++)
            {
                balloons[i - removedBalloons].Update(gt,window);
                if (balloons[i - removedBalloons].Health <= 0)
                {
                    balloons.RemoveAt(i);
                    removedBalloons++;
                }
            }

            // Previous Keyboard state
            previousKB = Keyboard.GetState();
        }

        /// <summary>
        /// when a balloon reaches the end sends event to remove lives
        /// </summary>
        /// <param name="damage"></param>
        private void TakeDamage(int damage)
        {
            takeDamage(damage);
        }

        /// <summary>
        /// Draw all the balloons
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            foreach (Balloons b in balloons)
            {
                b.Draw(sb);
            }
        }

        /// <summary>
        /// Create temporary code for balloons path
        /// </summary>
        private void initilizePath()
        {
            // HARD CODE, YEAH!
            Map1path = new List<Vector2>
            {
                new Vector2(tileSize.X * 2.5f, tileSize.Y * -0.5f),
                new Vector2(tileSize.X * 2.5f, tileSize.Y * 6.5f),
                new Vector2(tileSize.X * 4.5f, tileSize.Y * 6.5f),
                new Vector2(tileSize.X * 4.5f, tileSize.Y * 3.5f),
                new Vector2(tileSize.X * 8.5f, tileSize.Y * 3.5f),
                new Vector2(tileSize.X * 8.5f, tileSize.Y * 1.5f),
                new Vector2(tileSize.X * 10.5f, tileSize.Y * 1.5f),
                new Vector2(tileSize.X * 10.5f, tileSize.Y * 6.5f),
                new Vector2(tileSize.X * 7.5f, tileSize.Y * 6.5f),
                new Vector2(tileSize.X * 7.5f, tileSize.Y * 9.5f),
                new Vector2(tileSize.X * 5.5f, tileSize.Y * 9.5f),
                new Vector2(tileSize.X * 5.5f, tileSize.Y * 12.5f),
                new Vector2(tileSize.X * 9.5f, tileSize.Y * 12.5f),
                new Vector2(tileSize.X * 9.5f, tileSize.Y * 17.5f),
                new Vector2(tileSize.X * 7.5f, tileSize.Y * 17.5f),
                new Vector2(tileSize.X * 7.5f, tileSize.Y * 15.5f),
                new Vector2(tileSize.X * 2.5f, tileSize.Y * 15.5f),
                new Vector2(tileSize.X * 2.5f, tileSize.Y * 19.5f),
                new Vector2(tileSize.X * 7.5f, tileSize.Y * 19.5f),
                new Vector2(tileSize.X * 7.5f, tileSize.Y * 22.5f),
                new Vector2(tileSize.X * 13.5f, tileSize.Y * 22.5f)
            };
        }
        
        /// <summary>
        /// Destroy all balloons when game ends
        /// </summary>
        public void RemoveAllBalloons()
        {
            balloons.Clear();
        }
    }
}
