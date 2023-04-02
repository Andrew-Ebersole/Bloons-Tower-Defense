﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

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
        public event LoseResource takeDamage;
        public event LoseResource gainMoney;
        public float distanceTraveled;
        private SoundEffect pop;
        private ContentManager Content;
        public int targetDamage;




        // --- Properties --- //

        public int Health { get { return health; } }

        public float DistanceTraveled { get { return distanceTraveled; } }

        public int TargetDamage { get { return targetDamage; } set { targetDamage = value; } }

        public Vector2 Position { get { return new Vector2(rectangle.X + rectangle.Width / 2,
            rectangle.Y + rectangle.Height / 2); } }


        // --- Constructor --- //

        public Balloons(Texture2D texture, int x, int y, int width, int height, int health, List<Vector2> path, SoundEffect pop) 
            : base(texture, (int)path[0].X, (int)path[0].Y, width, height)
        {
            this.health = health;
            this.speed = speed;
            this.path = path;
            pathNum = 0;
            tint = Color.White;
            distanceTraveled = 0;
            this.pop = pop;
            targetDamage = 0;
        }



        // --- Methods --- //

        public override void Update(GameTime gt, Rectangle windowDimensions)
        {
            // Move to next instruction if at current spot
            if (pathNum < path.Count)
            {
                // move horizontal
                if (position.X < path[pathNum].X-2)
                {
                    position.X += speed;
                }
                else if (position.X > path[pathNum].X+2)
                {
                    position.X -= speed;
                }
                // move verticaluarly
                if (position.Y < path[pathNum].Y-2)
                {
                    position.Y += speed;
                }
                else if (position.Y > path[pathNum].Y+2)
                {
                    position.Y -= speed;
                }
                distanceTraveled += speed;

                if ((new Rectangle(rectangle.X,rectangle.Y,4,4 )).Intersects(new Rectangle((int)path[pathNum].X,
                    (int)path[pathNum].Y, 4, 4)))
                {
                    pathNum++;
                }
            }
            
            if (pathNum == path.Count)
            {
                takeDamage(health);
                health = 0;
            }
            
            switch(health)
            {
                default:
                    tint = Color.White;
                    speed = 0;
                    break;

                case 1:
                    tint = Color.Red;
                    speed = 1 * windowDimensions.Width / 330;
                    break;

                case 2:
                    tint = Color.Blue;
                    speed = 1.4f * windowDimensions.Width / 330;
                    break;

                case 3:
                    tint = Color.Green;
                    speed = 1.8f * windowDimensions.Width / 330;
                    break;

                case 4:
                    tint = Color.Yellow;
                    speed = 3.2f * windowDimensions.Width / 330;
                    break;
                case 5:
                    tint = Color.HotPink;
                    speed = 3.5f * windowDimensions.Width / 330;
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

        /// <summary>
        /// Removes amount of health
        /// </summary>
        /// <param name="damageAmount"> amount of health to remove </param>
        public void Damage(int damageAmount)
        {
            health -= damageAmount;
            pop.Play();
            gainMoney(damageAmount);
            targetDamage -= damageAmount;
        }

        public void HighlightRed()
        {
            tint = Color.DarkMagenta;
        }
    }
}
