// Authors: Edward Numrich, Billy Fox
// Started 2/16/2022
// This class manages code related to the player avatar.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Candlelight_Inn
{
    /// <summary>
    /// A player class that can move from tile-to-tile with WASD
    /// inputs and hold an Item
    /// </summary>
    /// 

    //This enum controls the directions the player may move in.  
    public enum MovementDirections
    {
        Up,
        Down,
        Left,
        Right,
        Hold,
        Move
    };

    class Player : GameObject
    {
        //Fields
        private Item heldItem;
        private MovementDirections playerDirection;
        private bool isMoving;
        private List<Rectangle> spriteSheetPositions;

        //Properties
        /// <summary>
        /// Allows the retreival of the player's held item
        /// </summary>
        public Item HeldItem
        {
            get { return heldItem; }
            set { heldItem = value; }
        }

        /// <summary>
        /// Allows for the retrival of the current movement of the player
        /// </summary>
        public MovementDirections CurrentDirection
        {
            get { return playerDirection; }
        }

        //Parameterized Constructors
        /// <summary>
        /// Creates a new player object with no held item using the
        /// components for a recatangle
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Player(Texture2D texture,
                      int x, int y, int width, int height) :     //Rectangle
                base (texture, x, y, width, height)              //Inhertiance
        {
            heldItem = null;
            spriteSheetPositions = new List<Rectangle>();
            // Front facing Sprites
            spriteSheetPositions.Add(new Rectangle(160, 0, 160, 160)); // 0
            spriteSheetPositions.Add(new Rectangle(320, 0, 160, 160)); // 1
            spriteSheetPositions.Add(new Rectangle(0, 160, 160, 160)); // 2
            // Left facing Sprites
            spriteSheetPositions.Add(new Rectangle(160, 160, 160, 160)); // 3
            spriteSheetPositions.Add(new Rectangle(0, 320, 160, 160)); // 4
            spriteSheetPositions.Add(new Rectangle(160, 320, 160, 160)); // 5
            // Right facing Sprites
            spriteSheetPositions.Add(new Rectangle(320, 160, 160, 160)); // 6
            spriteSheetPositions.Add(new Rectangle(320, 320, 160, 160)); // 7
            spriteSheetPositions.Add(new Rectangle(0, 480, 160, 160)); // 8
            // Back facing Sprites
            spriteSheetPositions.Add(new Rectangle(0, 0, 160, 160)); // 9
            spriteSheetPositions.Add(new Rectangle(160, 480, 160, 160)); // 10
            spriteSheetPositions.Add(new Rectangle(320, 480, 160, 160)); // 11
        }

        /// <summary>
        /// Creates a new player object with no held item using a rectangle
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        public Player(Texture2D texture, Rectangle position) :   //Rectangle
                base(texture, position)                          //Inhertiance
        {
            heldItem = null;
        }


        // This method controls the player's movement.
        public void PlayerMovement(MovementDirections input)
        {
            // The player's current direction will only be updated if something
            // other than None is being returned.
            if (input != MovementDirections.Hold &&
                input != MovementDirections.Move)
            {
                playerDirection = input;
            }
        }

        /// <summary>
        /// Draws the Player to the game window
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            int spriteSheetIndex = -1;
            switch (playerDirection)
            {
                case MovementDirections.Up:
                    spriteSheetIndex = 9;
                    break;

                case MovementDirections.Left:
                    spriteSheetIndex = 6;
                    break;

                case MovementDirections.Right:
                    spriteSheetIndex = 3;
                    break;

                case MovementDirections.Down:
                    spriteSheetIndex = 0;
                    break;
            }

            sb.Draw(sprite,         //Texture or Sprite
                  position,         //Position Rectangle
                  spriteSheetPositions[spriteSheetIndex],
               Color.White);        //Color Tint
        }

        /// <summary>
        /// Removes the current held item and replaces it with
        /// a new item
        /// </summary>
        /// <param name="tileItem"></param>
        /// <returns></returns>
        public Item ChangeItems(Item tileItem, Rectangle position,
                                LevelManager lManager)
        {
            Item itemToRemove = this.heldItem;

            //Swap current item with a new item
            if (heldItem != null && tileItem != null)
            {
                if (heldItem.ItemType == ItemType.Candle &&
                    tileItem.ItemType == ItemType.Match &&
                    tileItem.IsUsed == false)
                {
                    lManager.CandleStage = 3;
                    lManager.Timer = 0;
                    tileItem.IsUsed = true;
                }

                //Don't change the held item
                return tileItem;
            }

            else
            {
                heldItem = tileItem;

                if (heldItem != null)
                {
                    heldItem.IsHeld = true;
                }

                else if (itemToRemove != null)
                {
                    itemToRemove.IsHeld = false;
                    itemToRemove.X = position.X;
                    itemToRemove.Y = position.Y;
                }

                //Return the old item
                return itemToRemove;
            }
        }

        /// <summary>
        /// Updates the positon of the player character to be on
        /// the tile it is on
        /// </summary>
        /// <param name="map">The tile manager with the level map</param>
        public bool UpdatePosition(LevelManager map, bool isPushing)
        {
            Rectangle tilePostion = map.LevelMap[map.PlayerX, map.PlayerY].Position;

            int speed = 2;
            if (isPushing)
            {
                speed /= 2;
            }

            if (position.X > tilePostion.X)
            {
                position.X -= speed;
                return true;
            }
            else if (position.X < tilePostion.X)
            {
                position.X += speed;
                return true;
            }
            if (position.Y > tilePostion.Y)
            {
                position.Y -= speed;
                return true;
            }
            else if (position.Y < tilePostion.Y)
            {
                position.Y += speed;
                return true;
            }
            return false;
        }

        public void DrawAnimated(SpriteBatch sb, int walkCycle)
        {
            int spriteSheetIndex = -1;
            switch (playerDirection)
            {
                case MovementDirections.Up:
                    spriteSheetIndex = 9;
                    break;

                case MovementDirections.Left:
                    spriteSheetIndex = 6;
                    break;

                case MovementDirections.Right:
                    spriteSheetIndex = 3;
                    break;

                case MovementDirections.Down:
                    spriteSheetIndex = 0;
                    break;
            }

            if (walkCycle > 0)
            {
                if (((walkCycle - 1) / 8) % 2 == 0)
                {
                    spriteSheetIndex += 1;
                }
                if (((walkCycle - 1) / 8) % 2 == 1)
                {
                    spriteSheetIndex += 2;
                }
            }

            sb.Draw(sprite,           //Texture or Sprite
                    position,         //Position Rectangle
                    spriteSheetPositions[spriteSheetIndex],
                    Color.White);     //Color Tint
        }
    }
}
