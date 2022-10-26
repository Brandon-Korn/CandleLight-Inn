// Authors: Billy Fox , Brandon Korn, Edward Numrich
// This class will manage code related to individual tiles in a level.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Candlelight_Inn
{
    /// <summary>
    /// A Tile class that can hold other GameObject
    /// child classes
    /// </summary>
    class Tile : GameObject
    {
        // Fields
        private GameObject onTile;
        private Texture2D texture;


        // Properties
        public GameObject OnTile
        {
            get
            {
                return onTile;
            }
            set
            {
                // Checks to make sure you're not putting a tile on a tile
                if (!(value is Tile))
                {
                    onTile = value;
                }
            }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        /// <summary>
        /// Gets the length of one side of the tile. (Tiles are squares)
        /// </summary>
        public int TileLength
        {
            get { return texture.Width; }
        }

        // Constructors
        public Tile(int x, int y, int width, int height,
                    Texture2D texture) :
                base(texture, x, y, width, height)
        {
            onTile = null;
            this.texture = texture;
        }

        public Tile(Texture2D texture, Rectangle position) :
                base(texture, position)
        {
            onTile = null;
            this.texture = texture;
        }


        // Methods
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, position, Color.White);
            if (onTile != null && !(onTile is Player))
            {
                onTile.Draw(sb);
            }
        }
    }
}
