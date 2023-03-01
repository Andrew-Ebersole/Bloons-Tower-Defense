using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata;

namespace DevcadeGame
{
    internal class ContentManager
    {
        // --- Fields --- //

        private Texture2D grassTileSheet;
        private int tileSize;
        private GameObject[,] mapTiles;



        // --- Properties --- //





        // --- Constructor --- //

        public ContentManager(Texture2D grassTileSheet, int tileSize)
        {
            this.grassTileSheet = grassTileSheet;
            this.tileSize = tileSize;
            mapTiles = new GameObject[15, 35];
        }



        // --- Methods --- //

        private void Draw(SpriteBatch sb)
        {
            for (int x = 0; x < 15; x++)
            {
                for (int y = 0; y < 35; y++)
                {
                    mapTiles[x, y].Draw(sb);
                }
            }
        }



    }
}
