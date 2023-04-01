using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Runtime.CompilerServices;

namespace DevcadeGame
{
    internal class Monkey : GameObject
    {
        // --- Fields --- //

        private int damage;
        private int attackSpeed;
        private int range;
        private int cost;
        private Vector2 direction;
        Balloons target;
        private double timeSinceLastShot;
        private Texture2D circle;

        // --- Properties --- //





        // --- Constructor --- //

        public Monkey(Texture2D texture, Texture2D circle, int x, int y, int width, int height, int damage, int attackSpeed, int range, int cost) 
            : base(texture, x, y, width, height)
        {
            this.damage = damage;
            this.attackSpeed = attackSpeed;
            this.range = range;
            this.cost = cost;
            this.circle = circle;
        }



        // --- Methods --- //

        public void Update(GameTime gt, Rectangle windowDimensions, List<Balloons> bloons)
        {
            timeSinceLastShot += gt.ElapsedGameTime.TotalMilliseconds;

            List<Balloons> inRange = new List<Balloons>();
            inRange.Clear();
            if (bloons.Count > 0)
            {
                foreach (Balloons b in bloons)
                {
                    // Determine the farthest balloon that is in range
                    if (BalloonInRange(b))
                    {
                        inRange.Add(b);
                    }
                }
            }

            if (inRange.Count > 0)
            {
                Balloons target = inRange[0];
                foreach (Balloons b in inRange)
                {
                    if (b.DistanceTraveled > target.DistanceTraveled)
                    {
                        target = b;                    
                    }
                }
                
                // Show what the tower is targeting
                //target.HighlightRed();

                if (timeSinceLastShot > 1000 / attackSpeed
                    && BalloonInRange(target))
                {
                    timeSinceLastShot = 0;
                    shoot(target);
                }
            }
            
        }

        private void shoot(Balloons b)
        {
            b.Damage(damage);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

        public void drawRange(SpriteBatch sb)
        {
            sb.Draw(circle,
                new Rectangle((int)((rectangle.X + rectangle.Width / 2) - range), (int)((rectangle.Y + rectangle.Height / 2) - range),
                (int)range*2,(int)range*2),
                Color.White * 0.2f);
        }

        private bool BalloonInRange(Balloons b)
        {
            if (range + b.Rectangle.Width / 2 >=
                new Vector2(Math.Abs(Rectangle.X - b.Rectangle.X), Math.Abs(Rectangle.Y - b.Rectangle.Y)).Length())
            {
                return true;
            }
            return false;
        }
    }
}
