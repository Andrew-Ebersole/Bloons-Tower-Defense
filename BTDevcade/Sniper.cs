using DevcadeGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTDevcade
{
    internal class Sniper : Monkey
    {
        // --- Fields --- //

        float rotation;
        private double attackSpeed;
        private double timeSinceLastShot;
        private int damage;
        private Vector2 orgin;

        // --- Properties --- //



        // --- Constructor --- //


        public Sniper(Texture2D texture, Texture2D circle, Texture2D dart, int x, int y, int width, int height,
            int damage, double attackSpeed, double range, int cost, int pierce, List<Balloons> balloons, Vector2 orgin) 
            : base(texture, circle, dart, x, y, width, height, damage, attackSpeed, range, cost, pierce, balloons, orgin)
        {
            this.attackSpeed = attackSpeed;
            this.damage = damage;
            this.orgin = orgin;
            timeSinceLastShot = 0;
        }

        // --- Methods --- //

        public override void Update(GameTime gt, Rectangle windowDimensions,List<Balloons> balloons, float gameSpeed)
        {
            timeSinceLastShot += gt.ElapsedGameTime.TotalMilliseconds;

            if (balloons.Count > 0)
            {
                
                Balloons target = balloons[balloons.Count-1];
                foreach (Balloons b in balloons)
                {
                    if (b.DistanceTraveled > target.DistanceTraveled && b.Health > 0)
                    {
                        target = b;
                    }
                }

                // Show what the tower is targeting
                //target.HighlightRed();
                rotation = RotationAngle(target); 

                if (timeSinceLastShot > 950 / (attackSpeed * gameSpeed))
                {
                    // Randomize so snipers will not all shoot at the same time
                    Random rng = new Random();
                    timeSinceLastShot = rng.Next(-100,100);
                    target.Damage(damage);
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture,
                rectangle,
                null,
                Color.White,
                rotation,
                orgin,
                SpriteEffects.None,
                0);
        }

    }
}
