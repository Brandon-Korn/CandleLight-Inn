// Authors: Billy Fox
// This class manages code related to obstacles in the level.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Candlelight_Inn
{
    enum ObstacleType
    {
        Stationary,
        Moveable,
        Breakable
    }

    class Obstacle : GameObject
    {
        private ObstacleType type;
        private Item itemOnTop;

        // A get-only property for the type of this obstacle.
        public ObstacleType Type
        {
            get
            {
                return type;
            }
        }

        // Get only property for the item on the obsticle
        public Item ItemOnTop
        {
            get { return itemOnTop; }
            set { itemOnTop = value; }
        }

        // Other than the parameters of the base class constructor, all this
        // constructor takes in is the type of obstacle being created.
        public Obstacle(ObstacleType type, 
            Texture2D sprite, 
            Rectangle position) : 
            base (sprite, 
                position)
        {
            this.type = type;
        }

        // An overloaded constructor for when there is an item located on the obsticle
        public Obstacle(Item onTop, ObstacleType type,
            Texture2D sprite,
            Rectangle position) :
            base(sprite,
                position)
        {
            itemOnTop = onTop;
            this.type = type;
        }



        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, position, Color.White);
            if (itemOnTop != null)
            {
                itemOnTop.Draw(sb);
            }
        }

        /// <summary>
        /// Takes in an item and saves it as the item on the obsticle
        /// Updates the position to match the obsticles.
        /// </summary>
        /// <param name="toPut">The item to put on top</param>
        public void PutOnTop(Item toPut)
        {
            if (itemOnTop != null)
            {
                return;
            }

            itemOnTop = toPut;
            itemOnTop.UpdatePosition(position.X, position.Y);
        }

        public bool Push(LevelManager map, MovementDirections playerOrientation)
        {
            if (type == ObstacleType.Moveable)
            {
                // Checks the player's current orientation
                switch (playerOrientation)
                {
                    case MovementDirections.Up:
                        {
                            // Then checks to make sure the tile you're trying to move
                            // onto is not taken up by an obstacle
                            if ((map.LevelMap[map.PlayerX, map.PlayerY - 2].OnTile == null))
                            {
                                map.LevelMap[map.PlayerX, map.PlayerY - 2].OnTile = this;

                                map.LevelMap[map.PlayerX, map.PlayerY - 1].OnTile = null;

                                position = map.LevelMap[map.PlayerX, map.PlayerY - 2].Position;
                            }

                        }
                        break;

                    case MovementDirections.Down:
                        {
                            // Then checks to make sure the tile you're trying to move
                            // onto is not taken up by an obstacle
                            if ((map.LevelMap[map.PlayerX, map.PlayerY + 2].OnTile == null))
                            {
                                map.LevelMap[map.PlayerX, map.PlayerY + 2].OnTile = this;

                                map.LevelMap[map.PlayerX, map.PlayerY + 1].OnTile = null;

                                position = map.LevelMap[map.PlayerX, map.PlayerY + 2].Position;
                            }

                        }
                        break;

                    case MovementDirections.Left:
                        {
                            // Then checks to make sure the tile you're trying to move
                            // onto is not taken up by an obstacle
                            if ((map.LevelMap[map.PlayerX - 2, map.PlayerY].OnTile == null))
                            {
                                map.LevelMap[map.PlayerX - 2, map.PlayerY].OnTile = this;

                                map.LevelMap[map.PlayerX - 1, map.PlayerY].OnTile = null;

                                position = map.LevelMap[map.PlayerX - 2, map.PlayerY].Position;

                            }

                        }
                        break;

                    case MovementDirections.Right:
                        {
                            // Then checks to make sure the tile you're trying to move
                            // onto is not taken up by an obstacle
                            if ((map.LevelMap[map.PlayerX + 2, map.PlayerY].OnTile == null))
                            {
                                map.LevelMap[map.PlayerX + 2, map.PlayerY].OnTile = this;

                                map.LevelMap[map.PlayerX + 1, map.PlayerY].OnTile = null;
                                position = map.LevelMap[map.PlayerX + 2, map.PlayerY].Position;

                            }

                        }
                        break;

                }
                if (itemOnTop != null)
                {
                    itemOnTop.UpdatePosition(position.X, position.Y);
                }
                return true;
            }
            return false;
        }
    }
}
