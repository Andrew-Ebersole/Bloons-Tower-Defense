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
    internal class Monkey : GameObject
    {
        // --- Fields --- //

        private int damage;
        private int attackSpeed;
        private int range;
        private int cost;
        private Vector2 direction;


        // --- Properties --- //





        // --- Constructor --- //

        public Monkey(Texture2D texture, int x, int y, int width, int height, int damage, int attackSpeed, int range, int cost) : base(texture, x, y, width, height)
        {
            this.damage = damage;
            this.attackSpeed = attackSpeed;
            this.range = range;
            this.cost = cost;
        }



        // --- Methods --- //





    }
}
