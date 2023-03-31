using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DevcadeGame
{
    internal class GameObject
    {
        // --- Fields --- //

        protected Texture2D texture;
        protected Rectangle rectangle;
        protected Vector2 position;



        // --- Properties --- //

        /// <summary>
        /// returns x position of the game object
        /// </summary>
        public Vector2 Position { get { return position; } set {position = value; } }

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
            position = new Vector2(x, y);
            rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

        }



        // --- Methods --- //

        public virtual void Update(GameTime gameTime, Rectangle windowDimensions)
        {
        }

        public virtual void Draw(SpriteBatch sb)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, rectangle.Width, rectangle.Height);
            sb.Draw(
                texture,        // Image
                rectangle,      // Rectangle
                Color.White);   // Tint
        }

    }
}
