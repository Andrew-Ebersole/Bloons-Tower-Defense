using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography.X509Certificates;

namespace DevcadeGame
{
    internal class Balloons : GameObject
    {
        // --- Fields --- //

        private int health;
        private int speed;
        private List<Vector2> path;
        private int pathNum;



        // --- Properties --- //





        // --- Constructor --- //

        public Balloons(Texture2D texture, int x, int y, int width, int height, int health, int speed, List<Vector2> path) : base(texture, (int)path[0].X, (int)path[0].Y, width, height)
        {
            this.health = health;
            this.speed = speed;
            this.path = path;
            pathNum = 0;
        }



        // --- Methods --- //

        public override void Update(GameTime gt, Rectangle windowDimensions)
        {
            // Move to next instruction if at current spot
            if (pathNum < path.Count)
            {
                // move horizontal
                if (X < path[pathNum].X)
                {
                    X += 1;
                }
                else if (X > path[pathNum].X)
                {
                    X -= 1;
                }
                // move verticaluarly
                if (Y < path[pathNum].Y)
                {
                    Y += 1;
                }
                else if (Y > path[pathNum].Y)
                {
                    Y -= 1;
                }

                if ((new Rectangle(rectangle.X,rectangle.Y,4,4 )).Intersects(new Rectangle((int)path[pathNum].X,
                    (int)path[pathNum].Y, 4, 4)))
                {
                    pathNum++;
                }
            }
            
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture,
                new Rectangle(rectangle.X - rectangle.Width / 2, rectangle.Y - rectangle.Height / 2,
                rectangle.Width, rectangle.Height),
                Color.Red);
        }


    }
}
