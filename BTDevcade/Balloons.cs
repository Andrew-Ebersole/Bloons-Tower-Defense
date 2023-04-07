using System;
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
        private float sfxLevel;
        private int targetDamage;
        private bool didPop;
        private int count;



        // --- Properties --- //

        public int Health { get { return health; } }

        public float DistanceTraveled { get { return distanceTraveled; } }

        public bool DidPop
        {
            get
            {
                return didPop;
            }
        }


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
            didPop = false;
            count = 1;
        }



        // --- Methods --- //

        public void Update(GameTime gt, Rectangle windowDimensions, float gameSpeed, float sfxVolume, int count, bool didPop)
        {
            sfxLevel = sfxVolume;
            didPop = this.didPop;
            count = this.count;
            // Move to next instruction if at current spot
            if (pathNum < path.Count)
            {
                // move horizontal
                if (position.X < path[pathNum].X- windowDimensions.Width * 0.01f)
                {
                    position.X += speed * gameSpeed;
                }
                else if (position.X > path[pathNum].X+ windowDimensions.Width * 0.01f)
                {
                    position.X -= speed * gameSpeed;
                }
                // move verticaluarly
                if (position.Y < path[pathNum].Y-windowDimensions.Width*0.01f)
                {
                    position.Y += speed * gameSpeed;
                }
                else if (position.Y > path[pathNum].Y+ windowDimensions.Width * 0.01f)
                {
                    position.Y -= speed * gameSpeed;
                }
                distanceTraveled += speed * gameSpeed;

                if ((new Rectangle
                    (rectangle.X,rectangle.Y,(int)(windowDimensions.Width*0.02f),(int)(windowDimensions.Width*0.02f) ))
                    .Intersects(new Rectangle((int)path[pathNum].X,(int)path[pathNum].Y,
                    (int)(windowDimensions.Width * 0.02f), (int)(windowDimensions.Width * 0.02f))))
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
            if (damageAmount > health
                && health > 0)
            {
                gainMoney(health);
            } else
            {
                gainMoney(damageAmount);
            }
            health -= damageAmount;
            if (!didPop)
            {
                pop.Play((sfxLevel * 0.1f) / (int)Math.Sqrt(count), -0.15f, 0.0f);
                didPop = true;
            }

        }

        public void HighlightRed()
        {
            tint = Color.DarkMagenta;
        }
    }
}
