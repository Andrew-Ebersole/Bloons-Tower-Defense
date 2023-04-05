using DevcadeGame;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BTDevcade
{
    internal class TackShooter : Monkey
    {
        // --- Fields --- //

        private int damage;
        private double attackSpeed;
        private double range;
        private int cost;
        private Vector2 direction;
        private double timeSinceLastShot;
        private Texture2D circle;
        private Texture2D dart;
        private float rotation;
        private float tileSize;
        private List<Projectile> projectiles;
        private List<Balloons> balloons;
        private int pierce;
        private Vector2 orgin;
        private int numberOfTacks;


        // --- Properties --- //




        // --- Constructor --- //

        public TackShooter(Texture2D texture, Texture2D circle, Texture2D dart, int x, int y, int width, int height,
            int damage, double attackSpeed, double range, int cost, int pierce, List<Balloons> balloons, Vector2 orgin)
            : base(texture, circle, dart, x, y, width, height, damage, attackSpeed, range, cost, pierce, balloons, orgin)
        {
            this.damage = damage;
            this.attackSpeed = attackSpeed;
            this.range = range;
            this.cost = cost;
            this.circle = circle;
            this.dart = dart;
            this.pierce = pierce;
            this.balloons = balloons;
            this.orgin = orgin;
            rotation = 0;
            projectiles = new List<Projectile>();
            numberOfTacks = 8;
        }



        // --- Methods --- //

        public override void Update(GameTime gt, Rectangle windowDimensions, List<Balloons> bloons, float gameSpeed)
        {
            timeSinceLastShot += gt.ElapsedGameTime.TotalMilliseconds;

            tileSize = windowDimensions.Width / 12;

            if (BalloonInRange())
            {

                if (timeSinceLastShot > 950 / (attackSpeed * gameSpeed))
                {
                    timeSinceLastShot = 0;
                    shoot();
                }
            }

            List<Projectile> removeProjectiles = new List<Projectile>();
            foreach (Projectile p in projectiles)
            {
                if (p.Active)
                {
                    p.Update(gt, windowDimensions, gameSpeed);
                }
                else
                {
                    removeProjectiles.Add(p);
                }
            }

            foreach (Projectile p in removeProjectiles)
            {
                projectiles.Remove(p);
            }
            removeProjectiles.Clear();
        }

        private void shoot()
        {
            for (int i = 0; i < numberOfTacks; i++)
            {
                projectiles.Add(new Projectile(dart, rectangle.X, rectangle.Y,
                9, 16, 10, 1, 1, DirectionVector(i),
                (int)(tileSize * 2), balloons));
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

            foreach (Projectile p in projectiles)
            {
                p.Draw(sb);
            }
        }

        public void drawRange(SpriteBatch sb, float tileSize)
        {
            sb.Draw(circle,
                new Rectangle((int)(rectangle.X - range), (int)(rectangle.Y - range),
                (int)range * 2, (int)range * 2),
                Color.White * 0.2f);
        }

        private bool BalloonInRange()
        {
            foreach (Balloons b in balloons)
            {
                if (range + b.Rectangle.Width / 2 >=
                new Vector2(
                    Math.Abs((Rectangle.X + Rectangle.Width / 2) - (b.Rectangle.X+b.Rectangle.Width/2)),
                    Math.Abs((Rectangle.Y + Rectangle.Width / 2) - (b.Rectangle.Y+b.Rectangle.Height/2))).Length())
                {
                    return true;
                }
            }
            return false;
        }

        private Vector2 DirectionVector(int number)
        {
            double angle = number * 2 * (Math.PI / numberOfTacks);
             Vector2 result = new Vector2(
                (float)Math.Cos(angle),
                (float)Math.Sin(angle));

            result.Normalize();
            return result;
        }
    }
}
