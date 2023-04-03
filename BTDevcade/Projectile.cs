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
            int damage, Balloons b, int pierce, List<Balloons> balloons, int maxDistance) 
            : base(texture, x, y, width, height)
        {
            this.speed = speed;
            this.damage = damage;
            target = b;
            rotation = RotationAngle();
            this.pierce = pierce;
            this.balloons = balloons;
            direction = DirectionVector();
            distance = 0;
            this.maxDistance = maxDistance;
        }



        // --- Methods --- //

        public override void Update(GameTime gameTime, Rectangle windowDimensions)
        {
            
            position += speed * direction;
            distance += speed;
            if (distance > maxDistance)
            {
                pierce = 0;
            }
            foreach (Balloons b in balloons)
            {
                if (b.Rectangle.Intersects(rectangle)
                    && Active)
                {
                    pierce -= 1;
                    b.Damage(damage);
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
                Color.White,
                rotation,
                new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2),
                SpriteEffects.None,
                1);
            }
        }

        /// <summary>
        /// Returns the direction between
        /// </summary>
        /// <returns></returns>
        private Vector2 DirectionVector()
        {
            Vector2 result = new Vector2(target.Rectangle.X - rectangle.X,
                target.Rectangle.Y - rectangle.Y);

            result.Normalize();
            return result;
        }

        public float RotationAngle()
        {
            float angle = (float)Math.Atan2((target.Rectangle.Y+target.Rectangle.Height/2) - Rectangle.Y,
                (target.Rectangle.X+target.Rectangle.Width/2) - Rectangle.X);
            float rotationAngle = angle - (float)(Math.PI / 2);

            return rotationAngle;
        }

    }
}
