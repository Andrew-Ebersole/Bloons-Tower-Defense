﻿using System;
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


        // --- Properties --- //




        // --- Constructor --- //

        public Monkey(Texture2D texture, Texture2D circle, Texture2D dart, int x, int y, int width, int height,
            int damage, double attackSpeed, double range, int cost, int pierce, List<Balloons> balloons, Vector2 orgin) 
            : base(texture, x, y, width, height)
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
            rotation = 0f * (float)Math.PI;
            projectiles = new List<Projectile>();
        }



        // --- Methods --- //

        public virtual void Update(GameTime gt, Rectangle windowDimensions, List<Balloons> bloons, float gameSpeed)
        {
            timeSinceLastShot += gt.ElapsedGameTime.TotalMilliseconds;

            tileSize = windowDimensions.Width / 12;

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
                rotation = RotationAngle(target);

                if (timeSinceLastShot > 950 / (attackSpeed * gameSpeed)
                    && BalloonInRange(target))
                {
                    timeSinceLastShot = 0;
                    shoot(target);
                }
            }

            List<Projectile> removeProjectiles = new List<Projectile>();
            foreach(Projectile p in projectiles)
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

        private void shoot(Balloons b)
        {
            projectiles.Add(new Projectile(dart, rectangle.X, rectangle.Y,
                9, 16, 10, 1, pierce, DirectionVector(b), 
                (int)(tileSize * 6), balloons));
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

            foreach(Projectile p in projectiles)
            {
                p.Draw(sb);
            }
        }

        public void drawRange(SpriteBatch sb)
        {
            sb.Draw(circle,
                new Rectangle((int)(rectangle.X-range), (int)(rectangle.Y-range),
                (int)range*2,(int)range*2),
                Color.White * 0.2f);
        }

        private bool BalloonInRange(Balloons b)
        {
            if (range + b.Rectangle.Width / 2 >=
                new Vector2(
                    Math.Abs((Rectangle.X+Rectangle.Width/2) - (b.Rectangle.X)), 
                    Math.Abs((Rectangle.Y+Rectangle.Width/2) - (b.Rectangle.Y))).Length())
            {
                return true;
            }
            return false;
        }
        
        protected float RotationAngle(GameObject g)
        {
            float angle = (float)Math.Atan2(g.Rectangle.Y - Rectangle.Y, g.Rectangle.X - Rectangle.X);
            float rotationAngle = angle - (float)(Math.PI / 2);

            return rotationAngle;
        }

        /// <summary>
        /// Returns the direction between
        /// </summary>
        /// <returns></returns>
        private Vector2 DirectionVector(Balloons target)
        {
            Vector2 result = new Vector2(target.Rectangle.X - rectangle.X,
                target.Rectangle.Y - rectangle.Y);

            result.Normalize();
            return result;
        }
    }
}
