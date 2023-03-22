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



        // --- Properties --- //





        // --- Constructor --- //

        public BalloonManager(Rectangle tileSize,Texture2D balloons)
        {
            this.balloons = new List<Balloons>();
            this.tileSize = tileSize;
            previousKB = new KeyboardState();
            currentKB = new KeyboardState();
            balloonTexture = balloons;
            initilizePath();
        }



        // --- Methods --- //

            public void Update(GameTime gt, Rectangle window)
        {
            currentKB = Keyboard.GetState();

            if (currentKB.IsKeyDown(Keys.D1) && previousKB.IsKeyUp(Keys.D1))
            {
                balloons.Add(new Balloons(
                    balloonTexture,0,0,26*window.Width/420,30 * window.Width / 420,
                    1,
                    Map1path));
                balloons[balloons.Count - 1].takeDamage += TakeDamage;
            }
            if (currentKB.IsKeyDown(Keys.D2) && previousKB.IsKeyUp(Keys.D2))
            {
                balloons.Add(new Balloons(
                    balloonTexture, 0, 0, 26 * window.Width / 420, 30 * window.Width / 420,
                    2,
                    Map1path));
                balloons[balloons.Count - 1].takeDamage += TakeDamage;
            }
            if (currentKB.IsKeyDown(Keys.D3) && previousKB.IsKeyUp(Keys.D3))
            {
                balloons.Add(new Balloons(
                    balloonTexture, 0, 0, 26 * window.Width / 420, 30 * window.Width / 420,
                    3,
                    Map1path));
                balloons[balloons.Count - 1].takeDamage += TakeDamage;
            }
            if (currentKB.IsKeyDown(Keys.D4) && previousKB.IsKeyUp(Keys.D4))
            {
                balloons.Add(new Balloons(
                    balloonTexture, 0, 0, 26 * window.Width / 420, 30 * window.Width / 420,
                    4,
                    Map1path));
                balloons[balloons.Count - 1].takeDamage += TakeDamage;
            }
            if (currentKB.IsKeyDown(Keys.D5) && previousKB.IsKeyUp(Keys.D5))
            {
                balloons.Add(new Balloons(
                    balloonTexture, 0, 0, 26 * window.Width / 420, 30 * window.Width / 420,
                    5,
                    Map1path));
                balloons[balloons.Count - 1].takeDamage += TakeDamage;
            }

            foreach (Balloons b in balloons)
            {
                b.Update(gt, window);
            }

            previousKB = Keyboard.GetState();
        }

        private void TakeDamage(int damage)
        {
            takeDamage(damage);
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Balloons b in balloons)
            {
                b.Draw(sb);
            }
        }


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
        public void RemoveAllBalloons()
        {
            balloons.Clear();
        }
    }
}
