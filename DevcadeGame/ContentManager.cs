using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata;
using System.IO;
using System.Runtime.CompilerServices;

namespace DevcadeGame
{
    internal class ContentManager
    {
        // --- Fields --- //

        private Texture2D grassTileSheet;
        private int tileSize;
        private int sourceTileSize;
        private Rectangle[,] mapTiles;



        // --- Properties --- //

         



        // --- Constructor --- //

        public ContentManager(Texture2D grassTileSheet, int tileSize)
        {
            this.grassTileSheet = grassTileSheet;
            this.tileSize = tileSize;
            this.sourceTileSize = grassTileSheet.Width / 8;
            mapTiles = new Rectangle[12, 28];

            StreamReader mapInput = new StreamReader("Content/Map.txt");
            string line = "";
            int y = 0;
            while ((line = mapInput.ReadLine()) != null)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    Rectangle sourceRectangle = new Rectangle();
                    switch (line[x])
                    {
                        case '.':
                            // Grass
                            sourceRectangle = new Rectangle(
                                sourceTileSize * 0,sourceTileSize * 0   // Location
                                , sourceTileSize, sourceTileSize);      // Size
                            break;

                        case '_':
                            // Path
                            sourceRectangle = new Rectangle(
                                sourceTileSize * 0, sourceTileSize * 4  // Location
                                , sourceTileSize, sourceTileSize);      // Size
                            break;
                        case '1':
                            // Tall Grass
                            sourceRectangle = new Rectangle(
                                sourceTileSize * 1, sourceTileSize * 3  // Location
                                , sourceTileSize, sourceTileSize);      // Size
                            break;
                        case '2':
                            // Flower 1
                            sourceRectangle = new Rectangle(
                                sourceTileSize * 4, sourceTileSize * 3  // Location
                                , sourceTileSize, sourceTileSize);      // Size
                            break;
                        case '3':
                            // Flower 2
                            sourceRectangle = new Rectangle(
                                sourceTileSize * 5, sourceTileSize * 3  // Location
                                , sourceTileSize, sourceTileSize);      // Size
                            break;
                        case '4':
                            // Flower 3
                            sourceRectangle = new Rectangle(
                                sourceTileSize * 4, sourceTileSize * 1  // Location
                                , sourceTileSize, sourceTileSize);      // Size
                            break;
                    }
                    if (x < 12 && y < 28)
                    {
                        mapTiles[x, y] = sourceRectangle;
                    }
                }
                y++;
            }
            mapInput.Close();
        }



        // --- Methods --- //

        public void Draw(SpriteBatch sb)
        {
            for (int y = 0; y < 28; y++)
            {
                for (int x = 0; x < 12; x++)
                {
                    sb.Draw(
                        grassTileSheet,             // Texture
                        new Rectangle(x*tileSize,   // X pos
                        y*tileSize,                 // Y pos
                        tileSize,                   // Width
                        tileSize),                  // Height
                        mapTiles[x, y],             // Source Rectangle
                        Color.White);               // Tint
                }
            }
        }

       

    }
}
