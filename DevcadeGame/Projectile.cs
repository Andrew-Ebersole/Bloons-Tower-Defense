using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace DevcadeGame
{
    internal class Projectile : GameObject
    {
        // --- Fields --- //

        private int speed;
        private Vector2 direction;
        private int damage;



        // --- Properties --- //





        // --- Constructor --- //

        public Projectile(Texture2D texture, int x, int y, int width, int height, int speed, Vector2 direction, int damage) : base(texture, x, y, width, height)
        {
            this.speed = speed;
            this.direction = direction;
            this.damage = damage;
        }



        // --- Methods --- //





    }
}
