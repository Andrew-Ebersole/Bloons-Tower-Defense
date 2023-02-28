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
    internal class Balloons : GameObject
    {
        // --- Fields --- //

        private int health;
        private int speed;
        private List<Vector2> path;



        // --- Properties --- //





        // --- Constructor --- //

        public Balloons(Texture2D texture, int x, int y, int width, int height, int health, int speed, List<Vector2> path) : base(texture, x, y, width, height)
        {
            this.health = health;
            this.speed = speed;
            this.path = path;
        }



        // --- Methods --- //





    }
}
