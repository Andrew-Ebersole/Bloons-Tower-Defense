using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;

namespace DevcadeGame
{
    internal class Balloons : GameObject
    {
        // --- Fields --- //

        private int health;
        private float speed;
        private List<Vector2> path;
        private int pathNum;
        private Color tint;



        // --- Properties --- //





        // --- Constructor --- //

        public Balloons(Texture2D texture, int x, int y, int width, int height, int health, List<Vector2> path) : base(texture, (int)path[0].X, (int)path[0].Y, width, height)
        {
            this.health = health;
            this.speed = speed;
            this.path = path;
            pathNum = 0;
            tint = Color.White;
        }



        // --- Methods --- //

        public override void Update(GameTime gt, Rectangle windowDimensions)
        {
            // Move to next instruction if at current spot
            if (pathNum < path.Count)
            {
                // move horizontal
                if (position.X < path[pathNum].X)
                {
                    position.X += speed;
                }
                else if (position.X > path[pathNum].X)
                {
                    position.X -= speed;
                }
                // move verticaluarly
                if (position.Y < path[pathNum].Y)
                {
                    position.Y += speed;
                }
                else if (position.Y > path[pathNum].Y)
                {
                    position.Y -= speed;
                }

                if ((new Rectangle(rectangle.X,rectangle.Y,4,4 )).Intersects(new Rectangle((int)path[pathNum].X,
                    (int)path[pathNum].Y, 4, 4)))
                {
                    pathNum++;
                }
            }
            
            switch(health)
            {
                case 0:
                    tint = Color.White;
                    speed = 0;
                    break;

                case 1:
                    tint = Color.Red;
                    speed = 1 * windowDimensions.Width / 420;
                    break;

                case 2:
                    tint = Color.Blue;
                    speed = 1.2f * windowDimensions.Width / 420;
                    break;

                case 3:
                    tint = Color.Green;
                    speed = 1.4f * windowDimensions.Width / 420;
                    break;

                case 4:
                    tint = Color.Yellow;
                    speed = 1.6f * windowDimensions.Width / 420;
                    break;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, rectangle.Width, rectangle.Height);

            sb.Draw(texture,
                new Rectangle(rectangle.X - rectangle.Width / 2, rectangle.Y - rectangle.Height / 2,
                rectangle.Width, rectangle.Height),
                tint);
        }


    }
}
