// Authors: Matt Ivansek, Brandon Korn, Billy Fox
// This class manages the code for items.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Candlelight_Inn
{
    // This enum is for the different items that the player can pick up while
    // playing the game.
    enum ItemType 
    { 
        Candle,
        Key,
        Match
    }

    class Item : GameObject
    {
        // Fields
        private ItemType typeOfItem;
        private bool isHeld;
        private Rectangle heldPosition;

        //Bool for match
        private bool used;

        private const int tileSize = 64;

        /// <summary>
        /// Get only property for the item type
        /// </summary>
        public ItemType ItemType
        {
            get { return typeOfItem; }
        }


        public bool IsHeld
        {
            get { return isHeld; }
            set { isHeld = value; }
        }

        /// <summary>
        /// Get-Set Property for if a match has been used
        /// True if so, false if not
        /// </summary>
        public bool IsUsed
        {
            get { return used; }
            set { used = value; }
        }

        public override Rectangle Position
        {
            get
            {
                if (isHeld)
                {
                    return heldPosition;
                }
                else
                {
                    return position;
                }
            }
        }
        /// <summary>
        /// Constructor that calls the parent
        /// </summary>
        /// <param name="sprite">Texture 2D sprite</param>
        /// <param name="position">Rectangle position</param>
        /// <param name="type">The ItemType type of item</param>
        public Item(Texture2D sprite, Rectangle position, ItemType type) : 
              base (sprite, position)
        {
            typeOfItem = type;
            isHeld = false;
            heldPosition = new Rectangle(100, 100, tileSize * 2 / 3, tileSize * 2 / 3); // Placeholder for when we figure out
                                            // where the held item box is on the screen

            if (type == ItemType.Match)
            {
                used = false;
            }
        }

        /// <summary>
        /// Draws the item to the screen, either on a tile or in the held item location
        /// </summary>
        /// <param name="sb">Sprite Batch object</param>
        public override void Draw(SpriteBatch sb)
        {
            if (isHeld)
            {
                sb.Draw(sprite, heldPosition, Color.White);
            }
            else
            {
                sb.Draw(sprite, position, Color.White);
            }
        }

        // Updates the position of the item to a new rectangle
        public void UpdatePosition(int x, int y)
        {
            if (isHeld)
            {
                heldPosition.X = x;
                heldPosition.Y = y;
            }
            else
            {
                position.X = x;
                position.Y = y;
            }
        }
    }
}
