using Microsoft.Xna.Framework.Graphics;
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
    internal class Projectile : GameObject
    {
        // --- Fields --- //

        private int speed;
        private Vector2 direction;
        private int damage;
        private Balloons target;
        private float rotation;
        private int pierce;
        private List<Balloons> balloons;
        private List<Balloons> piercedBalloons;
        private int distance;
        private int maxDistance;
        



        // --- Properties --- //

        public bool Active
        {
            get
            {
                if (pierce > 0)
                {
                    return true;
                }
                return false;
            }
        }



        // --- Constructor --- //

        public Projectile(Texture2D texture, int x, int y, int width, int height, int speed,
            int damage, int pierce, Vector2 direction, int maxDistance, List<Balloons> balloons) 
            : base(texture, x, y, width, height)
        {
            this.speed = speed;
            this.damage = damage;
            this.pierce = pierce;
            this.balloons = balloons;
            this.direction = direction;
            distance = 0;
            this.maxDistance = maxDistance;
            piercedBalloons = new List<Balloons>();
            rotation = RotationAngle();
        }



        // --- Methods --- //

        public void Update(GameTime gameTime, Rectangle windowDimensions, float gameSpeed)
        {
            
            position += speed * direction * gameSpeed;
            distance += (int)(speed * gameSpeed);
            if (distance > maxDistance)
            {
                pierce = 0;
            }
            foreach (Balloons b in balloons)
            {
                if (b.Rectangle.Intersects(rectangle)
                    && Active
                    && NotPierced(b))
                {
                    pierce -= 1;
                    b.Damage(damage);
                    piercedBalloons.Add(b);
                }
            }
            base.Update(gameTime, windowDimensions);
        }

        public override void Draw(SpriteBatch sb)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, rectangle.Width, rectangle.Height);
            if (pierce > 0)
            {
                sb.Draw(texture,
                rectangle,
                null,
                Color.LightGray,
                rotation,
                new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2),
                SpriteEffects.None,
                1);
            }
        }

        public float RotationAngle()
        {
            float angle = (float)Math.Atan2(direction.Y,
                direction.X);
            float rotationAngle = angle - (float)(Math.PI / 2);

            return rotationAngle;
        }

        /// <summary>
        /// Returns true if the balloons has not been pierced by the projectile
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool NotPierced(Balloons b)
        {
            bool result = true;
            foreach (Balloons pierecdBalloon in piercedBalloons)
            {
                if (pierecdBalloon == b)
                {
                    result = false;
                }
            }
            return result;
        }

    }
}
