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



        // --- Properties --- //





        // --- Constructor --- //

        public BalloonManager(Rectangle tileSize,Texture2D balloons)
        {
            this.tileSize = tileSize;
            previousKB = new KeyboardState();
            currentKB = new KeyboardState();
            balloonTexture = balloons;
        }



        // --- Methods --- //

            public void Update(GameTime gt, Rectangle windowDimensions)
        {
            currentKB = Keyboard.GetState();

            if (currentKB.IsKeyDown(Keys.Space) && previousKB.IsKeyUp(Keys.Space))
            {
                balloons.Add(new Balloons(
                    balloonTexture,
                    0,
                    0,
                    50,
                    50,
                    1,
                    10,
                    Map1path));
            }

            foreach (Balloons b in balloons)
            {
                b.Update(gt, windowDimensions);
            }

            previousKB = Keyboard.GetState();
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
            Map1path.Add(new Vector2(tileSize.X * 2.5f, tileSize.Y *-0.5f));
            Map1path.Add(new Vector2(tileSize.X * 2.5f, tileSize.Y * 6.5f));
            Map1path.Add(new Vector2(tileSize.X * 4.5f, tileSize.Y * 6.5f));
            Map1path.Add(new Vector2(tileSize.X * 4.5f, tileSize.Y * 3.5f));
            Map1path.Add(new Vector2(tileSize.X * 8.5f, tileSize.Y * 3.5f));
            Map1path.Add(new Vector2(tileSize.X * 8.5f, tileSize.Y * 1.5f));
            Map1path.Add(new Vector2(tileSize.X * 8.5f, tileSize.Y * 6.5f));
            Map1path.Add(new Vector2(tileSize.X * 5.5f, tileSize.Y * 6.5f));
            Map1path.Add(new Vector2(tileSize.X * 5.5f, tileSize.Y * 9.5f));
            Map1path.Add(new Vector2(tileSize.X * 6.5f, tileSize.Y * 9.5f));
            Map1path.Add(new Vector2(tileSize.X * 6.5f, tileSize.Y * 12.5f));
            Map1path.Add(new Vector2(tileSize.X * 9.5f, tileSize.Y * 12.5f));
            Map1path.Add(new Vector2(tileSize.X * 9.5f, tileSize.Y * 17.5f));
            Map1path.Add(new Vector2(tileSize.X * 7.5f, tileSize.Y * 17.5f));
            Map1path.Add(new Vector2(tileSize.X * 7.5f, tileSize.Y * 15.5f));
            Map1path.Add(new Vector2(tileSize.X * 2.5f, tileSize.Y * 15.5f));
            Map1path.Add(new Vector2(tileSize.X * 2.5f, tileSize.Y * 19.5f));
            Map1path.Add(new Vector2(tileSize.X * 7.5f, tileSize.Y * 19.5f));
            Map1path.Add(new Vector2(tileSize.X * 7.5f, tileSize.Y * 22.5f));
            Map1path.Add(new Vector2(tileSize.X * 12.5f, tileSize.Y * 22.5f));
        }
    }
}
