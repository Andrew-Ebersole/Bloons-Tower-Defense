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
    internal class GameObject
    {
        // --- Fields --- //

        protected Texture2D texture;
        protected Rectangle rectangle;



        // --- Properties --- //

        /// <summary>
        /// returns x position of the game object
        /// </summary>
        public int X { get { return rectangle.X; } set { rectangle.X = value; } }

        /// <summary>
        /// returns the y position of the game object
        /// </summary>
        public int Y { get { return rectangle.Y; } set { rectangle.Y = value; } }

        /// <summary>
        /// returns the rectangle the game object resides in
        /// </summary>
        public Rectangle Rectangle { get { return rectangle; } }



        // --- Constructor --- //

        /// <summary>
        /// Creates a new game object with the given dimensions and image
        /// </summary>
        /// <param name="texture"> sprite of the game object </param>
        /// <param name="x"> starting x position </param>
        /// <param name="y"> starting y position </param>
        /// <param name="width"> width in pixels </param>
        /// <param name="height"> height in pixels </param>
        public GameObject(Texture2D texture, int x, int y, int width, int height)
        {
            this.texture = texture;
            rectangle = new Rectangle(x, y, width, height);
        }



        // --- Methods --- //

        public virtual void Update(GameTime gameTime, int windowWidth)
        {
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(
                texture,        // Image
                rectangle,      // Rectangle
                Color.White);   // Tint
        }

    }
}
