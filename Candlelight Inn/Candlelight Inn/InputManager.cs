using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Authors: Matt Ivansek, Eddie Numrich

namespace Candlelight_Inn
{
    /// <summary>
    /// Handles the movement and inputs of the player and their
    /// interactions with GameObjects
    /// </summary>
    class InputManager
    {
        private KeyboardState kbState;
        private KeyboardState previousKbState;
        private int heldFrames;

        private MouseState mState;
        private MouseState previousMState;

        public InputManager()
        {
            heldFrames = 0;
        }
        
        public void GetInput()
        {
            kbState = Keyboard.GetState();
            mState = Mouse.GetState();
        }

        public void SaveInput()
        {
            previousKbState = kbState;
            previousMState = mState;
        }

        /// <summary>
        /// CheckInput() checks input and returns an enum value
        /// relating to player movement
        /// </summary>
        /// <returns></returns>
        public MovementDirections CheckInput()
        {

            // Checks against each relevent movement key (wasd) to see if its being pressed
            // Returns the movement direction of that key
            // Held key tracks how many frames a single key has been held down
            if (SingleKeyPress(Keys.W) || SingleKeyPress(Keys.Up))
            {
                heldFrames = 0;
                return MovementDirections.Up;
            }

            else if (SingleKeyPress(Keys.A) || SingleKeyPress(Keys.Left))
            {
                heldFrames = 0;
                return MovementDirections.Left;
            }

            else if (SingleKeyPress(Keys.S) || SingleKeyPress(Keys.Down))
            {
                heldFrames = 0;
                return MovementDirections.Down;

            }

            else if (SingleKeyPress(Keys.D) || SingleKeyPress(Keys.Right))
            {
                heldFrames = 0;
                return MovementDirections.Right;
            }
            
            // If the player isn't pressing any relevent movement key (wasd),
            // It returns to hold where the player is
            else if(kbState.IsKeyUp(Keys.D) && kbState.IsKeyUp(Keys.S) &&
                    kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.W))
            {
                heldFrames = 0;
                return MovementDirections.Hold;
            }

            // If a relevent movement button (wasd) is being held down, it will either
            // return to hold its position, or if it has been held for a certain amount of frames,
            // it will tell the player to move forward where its facing
            else
            {
                heldFrames++;
                if (heldFrames >= 5)
                {
                    heldFrames = 0;
                    return MovementDirections.Move;
                }
                return MovementDirections.Hold;
            }            
        }

        // This method manages inputs related to picking up items.
        /// <summary>
        /// Handles the picking-up and placing of items on the map
        /// </summary>
        /// <param name="player"></param>
        /// <param name="map"></param>
        public void Hands(Player player, LevelManager map)
        {
            // For each direction, when space is pressed, swap the item held
            // with the item on the tile in front of the player (in the way the
            // player is facing). Due to the nature of tiles with and without obstacles
            // being different, there are checks to see if the tile has an obstacle. If
            // there is an item on an obstacle tile, it swaps using ItemOnTop property.
            // if the tile has no obstacle, it will just use the OnTile property.
            
            if (player.CurrentDirection == MovementDirections.Up && 
                map.PlayerY > 0 &&
                SingleKeyPress(Keys.Space))
            {
                if (map[map.PlayerX, map.PlayerY - 1].OnTile is Obstacle)
                {
                    Obstacle obstacleTile = (Obstacle)map[map.PlayerX, map.PlayerY - 1].OnTile;

                    if (obstacleTile.ItemOnTop == null)
                    {
                        obstacleTile.ItemOnTop = player.ChangeItems(
                            null,
                            map[map.PlayerX, map.PlayerY - 1].Position,
                            map);
                    }

                    else
                    {
                        obstacleTile.ItemOnTop = player.ChangeItems(
                            obstacleTile.ItemOnTop,
                            map[map.PlayerX, map.PlayerY - 1].Position,
                            map);
                    }
                }

                else
                {
                    Tile floorTile = map[map.PlayerX, map.PlayerY - 1];

                    if (floorTile.OnTile == null)
                    {
                        floorTile.OnTile = player.ChangeItems(
                            null,
                            map[map.PlayerX, map.PlayerY - 1].Position,
                            map);
                    }

                    else
                    {
                        floorTile.OnTile = player.ChangeItems(
                            (Item)floorTile.OnTile,
                            map[map.PlayerX, map.PlayerY - 1].Position,
                            map);
                    }
                }
            }
            else if (player.CurrentDirection == MovementDirections.Down &&
                map.PlayerY < map.LevelMap.GetLength(1) &&
                SingleKeyPress(Keys.Space))
            {

                if (map[map.PlayerX, map.PlayerY + 1].OnTile is Obstacle)
                {
                    Obstacle obstacleTile = (Obstacle)map[map.PlayerX, map.PlayerY + 1].OnTile;

                    if (obstacleTile.ItemOnTop == null)
                    {
                        obstacleTile.ItemOnTop = player.ChangeItems(
                            null,
                            map[map.PlayerX, map.PlayerY + 1].Position,
                            map);
                    }

                    else
                    {
                        obstacleTile.ItemOnTop = player.ChangeItems(
                            obstacleTile.ItemOnTop,
                            map[map.PlayerX, map.PlayerY + 1].Position,
                            map);
                    }
                }

                else
                {
                    Tile floorTile = map[map.PlayerX, map.PlayerY + 1];

                    if (floorTile.OnTile == null)
                    {
                        floorTile.OnTile = player.ChangeItems(
                            null,
                            map[map.PlayerX, map.PlayerY + 1].Position,
                            map);
                    }

                    else
                    {
                        floorTile.OnTile = player.ChangeItems(
                            (Item)floorTile.OnTile,
                            map[map.PlayerX, map.PlayerY + 1].Position,
                            map);
                    }
                }
            }
            else if (player.CurrentDirection == MovementDirections.Left &&
                map.PlayerX > 0 &&
                SingleKeyPress(Keys.Space))
            {
                if (map[map.PlayerX - 1, map.PlayerY].OnTile is Obstacle)
                {
                    Obstacle obstacleTile = (Obstacle)map[map.PlayerX - 1, map.PlayerY].OnTile; 

                    if (obstacleTile.ItemOnTop == null)
                    {
                        obstacleTile.ItemOnTop = player.ChangeItems(
                            null,
                            map[map.PlayerX - 1, map.PlayerY].Position,
                            map);
                    }

                    else
                    {
                        obstacleTile.ItemOnTop = player.ChangeItems(
                            obstacleTile.ItemOnTop,
                            map[map.PlayerX - 1, map.PlayerY].Position,
                            map);
                    }
                }

                else
                {
                    Tile floorTile = map[map.PlayerX - 1, map.PlayerY];

                    if (floorTile.OnTile == null)
                    {
                        floorTile.OnTile = player.ChangeItems(
                            null,
                            map[map.PlayerX - 1, map.PlayerY].Position,
                            map);
                    }

                    else
                    {
                        floorTile.OnTile = player.ChangeItems(
                            (Item)floorTile.OnTile,
                            map[map.PlayerX - 1, map.PlayerY].Position,
                            map);
                    }
                }
            }
            else if (player.CurrentDirection == MovementDirections.Right &&
                map.PlayerY < map.LevelMap.GetLength(0) &&
                SingleKeyPress(Keys.Space))
            {
                if (map[map.PlayerX + 1, map.PlayerY].OnTile is Obstacle)
                {
                    Obstacle obstacleTile = (Obstacle)map[map.PlayerX + 1, map.PlayerY].OnTile;

                    if (obstacleTile.ItemOnTop == null)
                    {
                        obstacleTile.ItemOnTop = player.ChangeItems(
                            null,
                            map[map.PlayerX + 1, map.PlayerY].Position,
                            map);
                    }

                    else
                    {
                        obstacleTile.ItemOnTop = player.ChangeItems(
                            obstacleTile.ItemOnTop,
                            map[map.PlayerX + 1, map.PlayerY].Position,
                            map);
                    }
                }

                else
                {
                    Tile floorTile = map[map.PlayerX + 1, map.PlayerY];

                    if (floorTile.OnTile == null)
                    {
                        floorTile.OnTile = player.ChangeItems(
                            null,
                            map[map.PlayerX + 1, map.PlayerY].Position,
                            map);
                    }

                    else
                    {
                        floorTile.OnTile = player.ChangeItems(
                            (Item)floorTile.OnTile,
                            map[map.PlayerX + 1, map.PlayerY].Position,
                            map);
                    }
                }
            }

            //Updates the position of the held item
            if (player.HeldItem != null)
            {
                player.HeldItem.UpdatePosition(player.X - 15, player.Y - 15);
            }
        }

        public GameState ChangeMenu(GameState currentState)
        {

            if ((SingleKeyPress(Keys.Q) || SingleKeyPress(Keys.Escape)) && 
                currentState == GameState.GamePlay)
            {
                return GameState.Paused;
            }

            else if ((SingleKeyPress(Keys.Q) || SingleKeyPress(Keys.Escape)) &&
                currentState == GameState.Paused)
            {
                return GameState.GamePlay;
            }

            if (SingleKeyPress(Keys.R) && 
               (currentState == GameState.GamePlay || currentState == GameState.Paused))
            {
                return GameState.StartGame;
            }

            return currentState;
        }

        /// <summary>
        /// Returns if this is the first frame that a given key is being pressed
        /// </summary>
        /// <param name="key">The Key from the keyboard</param>
        /// <returns>Returns true if this is the first frame its being pressed</returns>
        public bool SingleKeyPress(Keys key)
        {
            if(kbState.IsKeyDown(key) && previousKbState.IsKeyUp(key))
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public bool SingleButtonPress(Button button)
        {
            Point mousePoint = new Point(mState.X, mState.Y);

            if (mState.LeftButton == ButtonState.Pressed &&
                previousMState.LeftButton == ButtonState.Released &&
                button.Position.Contains(mousePoint))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Point GetMousePos()
        {
            return mState.Position;
        }
    }
}
