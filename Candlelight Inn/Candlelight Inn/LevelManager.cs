// Authors: Billy Fox, Brandon Korn, Eddie Numrich, Jay Miller
// This class will load in levels to be played.
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Candlelight_Inn
{
    /// <summary>
    /// A Level manager to handle drawing and manipulating the current level
    /// </summary>
    class LevelManager
    {
        // Fields
        private Tile[,] levelMap;
        private string[,] encodedLevel;
        private StreamReader levelGenerator;
        private string currLine;
        private string[] splitLine;
        private Obstacle obstacleWithItem;
        private Texture2D itemTexture;
        private Texture2D obstacleTexture;
        private int trueCandleStage = 3;
        private int timer = 0;
        private int playerX;
        private int playerY;
        private int exitX;
        private int exitY;
        private Tile exitReference;
        private bool isLocked;
        private List<Item> levelItems = new List<Item>();
        private int movementCounter = 0;
        private MovementDirections toMoveDirection;
        private List<Obstacle> moveableObjectsList = new List<Obstacle>();

        private const int tileSize = 64;



        // A get-only property for the level's map.
        public Tile[,] LevelMap
        {
            get
            {
                return levelMap;
            }
        }

        // Get only properties for the indexes of the tiles that the player
        // is currently on.
        public int PlayerX
        {
            get { return playerX; }
        }
        public int PlayerY
        {
            get { return playerY; }
        }

        /// <summary>
        /// Gets a specific tile from the level map
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public Tile this[int X, int Y]
        {
            get { return levelMap[X, Y]; }
            set { levelMap[X, Y] = value; }
        }

        // A get-and-set property for whether the exit is locked or not.
        public bool IsLocked
        {
            get
            {
                return isLocked;
            }
            set
            {
                isLocked = value;
            }
        }

        public int WalkCycleCounter
        {
            get { return movementCounter; }
        }

        public int CandleStage
        {
            get { return trueCandleStage; }
            set { trueCandleStage = value; }
        }

        public int Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        /// <summary>
        /// Creates the Level Manager and initalizes the tile map
        /// </summary>
        /// <param name="tileSprite">Takes a Texture2D sprite for the tile
        /// </param>
        public LevelManager()
        {
        }

        public void LoadLevel(string filename,
                        Texture2D tileSprite,
                        Texture2D obstacleSprite,
                        Texture2D candleSprite,
                        Texture2D keySprite,
                        Texture2D exitSprite,
                        Texture2D moveableSprite,
                        Texture2D matchSprite,
                        Texture2D bookcaseSprt,
                        Player player)
        {
            // Reset code
            timer = 0;
            player.HeldItem = null;

            obstacleTexture = obstacleSprite;
            itemTexture = candleSprite;

            // The try-catch block ensures the game won't crash if the level's
            // file happens to be missing.
            try
            {
                levelGenerator = new StreamReader("../../../Content/" + filename);

                // The first line contains the dimensions of the map, along
                // with the number of obstacles in the level. It is pulled in,
                // then split.
                currLine = levelGenerator.ReadLine();
                splitLine = currLine.Split('|');
                // The first number in the array is the row count, while the
                // second is the column count. This will be used to initialize
                // both the actual level map and the encoded version. The
                // latter will be used to draw the map later.
                levelMap = new Tile[int.Parse(splitLine[0]),
                    int.Parse(splitLine[1])];
                encodedLevel = new string[int.Parse(splitLine[0]),
                    int.Parse(splitLine[1])];

                // The next line is blank, so it will be skipped.
                currLine = levelGenerator.ReadLine();

                // We know the dimensions of the array now, and we can use them
                // with a nested for loop to read in the rest of the grid.
                for (int i = 0; i < encodedLevel.GetLength(1); i++)
                {
                    // First things first, the next line is pulled and
                    // processed.
                    currLine = levelGenerator.ReadLine();
                    splitLine = currLine.Split('|');

                    // splitLine now contains the current row being constructed
                    // in this iteration, so the nested loop will take whatever
                    // is located at index j in splitLine, then plug it into
                    // the corresponding index of encodedLevel.
                    for (int j = 0; j < encodedLevel.GetLength(0); j++)
                    {
                        encodedLevel[j, i] = splitLine[j];
                    }
                }

                int playerXIndex = 0;
                int playerYIndex = 0;
                while(encodedLevel[playerXIndex, playerYIndex] != "p")
                {
                    if(playerYIndex == encodedLevel.GetLength(1) - 1)
                    {
                        playerYIndex = 0;
                        playerXIndex++;
                    }
                    else
                    {
                        playerYIndex++;
                    }
                }
                levelMap[playerXIndex, playerYIndex] = new Tile(334, 334, tileSize, tileSize, tileSprite);
                levelMap[playerXIndex, playerYIndex].OnTile = player;
                playerX = playerXIndex;
                playerY = playerYIndex;

                // This nested loop sets up the real level map, filling it with
                // tile objects.
                for (int i = 0; i < encodedLevel.GetLength(0); i++)
                {
                    for (int j = 0; j < encodedLevel.GetLength(1); j++)
                    {
                        if((i != playerXIndex) || (j != playerYIndex))
                        {
                            levelMap[i, j] = new Tile(334 + (tileSize * (i - PlayerX)),
                                                      334 + (tileSize * (j - PlayerY)),
                                                      tileSize,
                                                      tileSize,
                                                      tileSprite);
                        }

                        switch (encodedLevel[i, j])
                        {
                            // "o" in the encoded form of the level signifies an
                            // obstacle's presence. In this event, an obstacle will
                            // be created on top that tile.
                            case "o":
                                levelMap[i, j].OnTile = new Obstacle(ObstacleType.Stationary,
                                                                obstacleSprite,
                                                                new Rectangle(levelMap[i, j].X,
                                                                              levelMap[i, j].Y,
                                                                              tileSize,
                                                                              tileSize));
                                break;

                            // With a "c", it signifies that the candle item will be placed
                            // on an obsticle at that tile.
                            case "c":
                                Item candle = new Item(candleSprite,
                                                    new Rectangle(levelMap[i, j].X,
                                                                  levelMap[i, j].Y,
                                                                  tileSize,
                                                                  tileSize),
                                                    ItemType.Candle);

                                levelMap[i, j].OnTile = new Obstacle(ObstacleType.Stationary,
                                                                obstacleSprite,
                                                                new Rectangle(levelMap[i, j].X,
                                                                              levelMap[i, j].Y,
                                                                              tileSize,
                                                                              tileSize));
                                obstacleWithItem = (Obstacle)levelMap[i, j].OnTile;
                                obstacleWithItem.PutOnTop(candle);
                                levelItems.Add(candle);
                                break;

                            // "k" is the starting position of the key.
                            case "k":
                                Item key = new Item(keySprite,
                                                    new Rectangle(levelMap[i, j].X,
                                                                  levelMap[i, j].Y,
                                                                  tileSize,
                                                                  tileSize),
                                                    ItemType.Key);
                                levelMap[i, j].OnTile = new Obstacle(ObstacleType.Stationary,
                                                                obstacleSprite,
                                                                new Rectangle(levelMap[i, j].X,
                                                                              levelMap[i, j].Y,
                                                                              tileSize,
                                                                              tileSize));
                                obstacleWithItem = (Obstacle)levelMap[i, j].OnTile;
                                obstacleWithItem.PutOnTop(key);
                                levelItems.Add(key);
                                break;

                            // "e" is where the exit will be located.
                            case "e":
                                levelMap[i, j].OnTile = new Obstacle(ObstacleType.Stationary,
                                                                exitSprite,
                                                                new Rectangle(levelMap[i, j].X,
                                                                              levelMap[i, j].Y,
                                                                              tileSize,
                                                                              tileSize));
                                exitReference = levelMap[i, j];
                                exitX = i;
                                exitY = j;
                                isLocked = true;
                                break;

                            // "m" is the location of a moveable block.
                            case "m":
                                Obstacle moveable = new Obstacle(ObstacleType.Moveable,
                                                                     moveableSprite,
                                                                     new Rectangle(levelMap[i, j].X,
                                                                                   levelMap[i, j].Y,
                                                                                   tileSize,
                                                                                   tileSize));
                                levelMap[i, j].OnTile = moveable;
                                moveableObjectsList.Add(moveable);
                                break;

                            // "a" is the location of a mAtch.
                            case "a":
                                Item match = new Item(matchSprite,
                                                    new Rectangle(levelMap[i, j].X,
                                                                  levelMap[i, j].Y,
                                                                  tileSize,
                                                                  tileSize),
                                                    ItemType.Match);
                                levelMap[i, j].OnTile = new Obstacle(ObstacleType.Stationary,
                                                                obstacleSprite,
                                                                new Rectangle(levelMap[i, j].X,
                                                                              levelMap[i, j].Y,
                                                                              tileSize,
                                                                              tileSize));
                                obstacleWithItem = (Obstacle)levelMap[i, j].OnTile;
                                obstacleWithItem.PutOnTop(match);
                                levelItems.Add(match);
                                break;

                            // a variant of the standard obstacle to add more visual flair
                            // "b" bookcase
                            case "b":
                                levelMap[i, j].OnTile = new Obstacle(ObstacleType.Stationary,
                                                                bookcaseSprt,
                                                                new Rectangle(levelMap[i, j].X,
                                                                              levelMap[i, j].Y,
                                                                              tileSize,
                                                                              tileSize));
                                break;
                        }
                    }
                }
            }

            catch (FileNotFoundException levelUndetected)
            {
                Console.WriteLine(levelUndetected.Message);
            }

            finally
            {
                if (levelGenerator != null)
                {
                    levelGenerator.Close();
                }
            }
        }

        /// <summary>
        /// Draws the tiles in each place in the tile map
        /// </summary>
        /// <param name="sb">The Sprite Batch Object</param>
        public void DrawMap(SpriteBatch sb)
        {
            // The tiles are drawn first.
            for (int x = 0; x < levelMap.GetLength(0); x++)
            {
                for (int y = 0; y < levelMap.GetLength(1); y++)
                {
                    levelMap[x, y].Draw(sb);

                    // If something is on the tile, it will be drawn next.
                    if (levelMap[x, y].OnTile != null)
                    {
                        // The test sprite I'm using right now is going to
                        // appear when the door is locked, and vanish when it
                        // unlocks.
                        if (levelMap[x, y].OnTile == exitReference &&
                            isLocked == true)
                        {
                            levelMap[x, y].OnTile.Draw(sb);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Moves the player based on where the player is facing
        /// </summary>
        /// <param name="playerCharacter">The instance of the player character</param>
        public bool MovePlayer(Player playerCharacter)
        {
            // Checks the player's current orientation
            switch (playerCharacter.CurrentDirection)
            {
                case MovementDirections.Up:
                    {
                        if (levelMap[playerX, playerY - 1].OnTile is Obstacle)
                        {
                            Obstacle moveableCheck = (Obstacle)levelMap[playerX, playerY - 1].OnTile;
                            if (moveableCheck.Type == ObstacleType.Moveable && playerCharacter.HeldItem == null)
                            {
                                moveableCheck.Push(this, playerCharacter.CurrentDirection);
                            }
                        }
                            // Then checks to make sure the tile you're trying to move
                            // onto is not taken up by an obstacle
                        if (levelMap[playerX, playerY - 1].OnTile == null)
                        {
                            // Removes the player from the tile its on, changes the player's 
                            // tile position, then puts the player on the correct tile
                            levelMap[playerX, playerY].OnTile = null;
                            playerY--;

                            if (playerCharacter.HeldItem != null)
                            {
                                playerCharacter.HeldItem.Y--;
                            }                            

                            levelMap[playerX, playerY].OnTile = playerCharacter;

                            toMoveDirection = MovementDirections.Up;
                            return true;
                        }
                    }
                    break;

                case MovementDirections.Down:
                    {
                        if (levelMap[playerX, playerY + 1].OnTile is Obstacle)
                        {
                            Obstacle moveableCheck = (Obstacle)levelMap[playerX, playerY + 1].OnTile;
                            if (moveableCheck.Type == ObstacleType.Moveable && playerCharacter.HeldItem == null)
                            {
                                moveableCheck.Push(this, playerCharacter.CurrentDirection);
                            }
                        }

                        if ((levelMap[playerX, playerY + 1].OnTile == null))
                        {
                            levelMap[playerX, playerY].OnTile = null;
                            playerY++;

                            if (playerCharacter.HeldItem != null)
                            {
                                playerCharacter.HeldItem.Y++;
                            }

                            levelMap[playerX, playerY].OnTile = playerCharacter;

                            toMoveDirection = MovementDirections.Down;
                            return true;
                        }

                    }
                    break;

                case MovementDirections.Left:
                    {
                        if (levelMap[playerX - 1, playerY].OnTile is Obstacle)
                        {
                            Obstacle moveableCheck = (Obstacle)levelMap[playerX - 1, playerY].OnTile;
                            if (moveableCheck.Type == ObstacleType.Moveable && playerCharacter.HeldItem == null)
                            {
                                moveableCheck.Push(this, playerCharacter.CurrentDirection);
                            }
                        }

                        if ((levelMap[playerX - 1, playerY].OnTile == null))
                        {
                            levelMap[playerX, playerY].OnTile = null;
                            playerX--;

                            if (playerCharacter.HeldItem != null)
                            {
                                playerCharacter.HeldItem.X--;
                            }

                            levelMap[playerX, playerY].OnTile = playerCharacter;

                            toMoveDirection = MovementDirections.Left;
                            return true;
                        }
                    }
                    break;

                case MovementDirections.Right:
                    {
                        if (levelMap[playerX + 1, playerY].OnTile is Obstacle)
                        {
                            Obstacle moveableCheck = (Obstacle)levelMap[playerX + 1, playerY].OnTile;
                            if (moveableCheck.Type == ObstacleType.Moveable && playerCharacter.HeldItem == null)
                            {
                                moveableCheck.Push(this, playerCharacter.CurrentDirection);
                            }
                        }

                        if ((levelMap[playerX + 1, playerY].OnTile == null))
                        {
                            levelMap[playerX, playerY].OnTile = null;
                            playerX++;

                            if (playerCharacter.HeldItem != null)
                            {
                                playerCharacter.HeldItem.X++;
                            }

                            levelMap[playerX, playerY].OnTile = playerCharacter;

                            toMoveDirection = MovementDirections.Right;
                            return true;
                        }
                    }
                    break;
            }

            return false;
        }

        public bool UpdateMapPositions(MovementDirections currentDirection)
        {
            int speed = tileSize / 16;


            if (movementCounter == 16)
            {
                movementCounter = 0;
                return false;
            }

            switch (toMoveDirection)
            {
                case MovementDirections.Up:
                    for (int i = 0; i < encodedLevel.GetLength(0); i++)
                    {
                        for (int j = 0; j < encodedLevel.GetLength(1); j++)
                        {
                            levelMap[i, j].Y += speed;
                        }
                    }

                    foreach (Item levelItem in levelItems)
                    {
                        levelItem.Y += speed;
                    }
                    movementCounter++;
                    
                    return true;

                case MovementDirections.Down:
                    for (int i = 0; i < encodedLevel.GetLength(0); i++)
                    {
                        for (int j = 0; j < encodedLevel.GetLength(1); j++)
                        {
                            levelMap[i, j].Y -= speed;
                        }
                    }

                    foreach (Item levelItem in levelItems)
                    {
                        levelItem.Y -= speed;
                    }
                    movementCounter++;
                    
                    return true;
                    

                case MovementDirections.Left:
                    for (int i = 0; i < encodedLevel.GetLength(0); i++)
                    {
                        for (int j = 0; j < encodedLevel.GetLength(1); j++)
                        {
                            levelMap[i, j].X += speed;
                        }
                    }

                    foreach (Item levelItem in levelItems)
                    {
                        levelItem.X += speed;
                    }
                    movementCounter++;
                    
                    return true;

                case MovementDirections.Right:
                    for (int i = 0; i < encodedLevel.GetLength(0); i++)
                    {
                        for (int j = 0; j < encodedLevel.GetLength(1); j++)
                        {
                            levelMap[i, j].X -= speed;
                        }
                    }

                    foreach (Item levelItem in levelItems)
                    {
                        levelItem.X -= speed;
                    }
                    movementCounter++;
                    
                    return true;

            }

            return false;
        }

        // This method checks if the exit door has been unlocked.
        /// <summary>
        /// Checks to see if the player has set the key on top of
        /// the exit door. If so, return false for the door no longer
        /// being locked
        /// </summary>
        /// <param name="player">Reference to player character</param>
        /// <param name="iManager">Reference to the input manager</param>
        /// <returns>False if door is no longer locked; True if locked</returns>
        public bool UnlockExit(Player player, InputManager iManager)
        {
            // If the player has the key, and is one tile away from the
            // exit, the door will be unlocked.
            Obstacle exitObstacle = (Obstacle)exitReference.OnTile;

            if (exitObstacle.ItemOnTop != null &&
                exitObstacle.ItemOnTop.ItemType == ItemType.Key)
            {
                isLocked = false;
                exitObstacle.ItemOnTop = null;
            }

            return isLocked;
        }

        // This method controls the light around the player.
        public void UpdateLight(Player player, SpriteBatch sb, GameState currentGameState, bool godMode)
        {
            Item candle = null;
            Item item = null;
            Item match = null;
            Tile tileToDraw = null;
            int playerX = 0;
            int playerY = 0;
            int candleX = 0;
            int candleY = 0;
            int itemX = 0;
            int itemY = 0;
            int matchX = 0;
            int matchY = 0;

            //Finds the coordinates for each item in the stage
            foreach (Item levelItem in levelItems)
            {
                if (levelItem.ItemType is ItemType.Candle)
                {
                    candle = levelItem;
                    candleX = levelItem.X;
                    candleY = levelItem.Y;
                }

                else if (levelItem.ItemType is ItemType.Key)
                {
                    item = levelItem;
                    itemX = levelItem.X;
                    itemY = levelItem.Y;
                }

                else if (levelItem.ItemType is ItemType.Match)
                {
                    match = levelItem;
                    matchX = levelItem.X;
                    matchY = levelItem.Y;
                }
            }

            //Find tile player is on and candle, key, match
            for (int i = 0; i < levelMap.GetLength(0); i++)
            {
                for (int j = 0; j < levelMap.GetLength(1); j++)
                {
                    //Finds if player has a held item
                    if (levelMap[i, j].OnTile == player)
                    {
                        playerX = i;
                        playerY = j;

                        if (player.HeldItem != null &&
                            player.HeldItem.ItemType == ItemType.Candle)
                        {
                            candle = player.HeldItem;
                            candleX = playerX;
                            candleY = playerY;
                        }

                        else if (player.HeldItem != null &&
                                 player.HeldItem.ItemType == ItemType.Key)
                        {
                            item = player.HeldItem;
                            itemX = playerX;
                            itemY = playerY;
                        }

                        else if (player.HeldItem != null &&
                                 player.HeldItem.ItemType == ItemType.Match)
                        {
                            match = player.HeldItem;
                            matchX = playerX;
                            matchY = playerY;
                        }
                    }

                    //Checks item if it's on an obstacle on a tile
                    else if (levelMap[i, j].OnTile is Obstacle)
                    {
                        Obstacle obstacle = (Obstacle)levelMap[i, j].OnTile;
                        Item tileItem = obstacle.ItemOnTop;

                        if (tileItem != null && tileItem.ItemType == ItemType.Candle)
                        {
                            candleX = i;
                            candleY = j;
                            candle = tileItem;
                        }

                        else if (tileItem != null && tileItem.ItemType == ItemType.Key)
                        {
                            itemX = i;
                            itemY = j;
                            item = tileItem;
                        }

                        else if (tileItem != null && tileItem.ItemType == ItemType.Match)
                        {
                            matchX = i;
                            matchY = j;
                            match = tileItem;
                        }
                    }

                    //Checks item if it's just on a tile
                    else if (levelMap[i, j].OnTile is Item)
                    {
                        Item tileItem = (Item)levelMap[i, j].OnTile;

                        if (tileItem != null && tileItem.ItemType == ItemType.Candle)
                        {
                            candleX = i;
                            candleY = j;
                            candle = tileItem;
                        }

                        else if (tileItem != null && tileItem.ItemType == ItemType.Key)
                        {
                            itemX = i;
                            itemY = j;
                            item = tileItem;
                        }

                        else if (tileItem != null && tileItem.ItemType == ItemType.Key)
                        {
                            matchX = i;
                            matchY = j;
                            match = tileItem;
                        }
                    }
                }
            }

            //Lights tiles immediately surrounding player
            for (int i = playerX - 1; i <= playerX + 1; i++)
            {
                for (int j = playerY - 1; j <= playerY + 1; j++)
                {
                    if (i < 0 || i > levelMap.GetLength(0))
                    {
                        continue;
                    }
                    if (j < 0 || j > levelMap.GetLength(1))
                    {
                        continue;
                    }

                    tileToDraw = levelMap[i, j];

                    if (tileToDraw.OnTile is Obstacle)
                    {
                        sb.Draw(tileToDraw.Sprite, tileToDraw.Position, Color.White);
                        sb.Draw(tileToDraw.OnTile.Sprite, tileToDraw.Position, Color.White);
                    }

                    else
                    {
                        sb.Draw(tileToDraw.Sprite, tileToDraw.Position, Color.White);
                    }

                }
            }

            //Draws Light surrounding candle and item sprite
            if (candle != null)
            {
                //Draws less rings around player per decrease in candle stage
                for (int i = candleX - trueCandleStage; i <= candleX + trueCandleStage; i++)
                {
                    for (int j = candleY - trueCandleStage; j <= candleY + trueCandleStage; j++)
                    {
                        if (i < 0 || i >= levelMap.GetLength(0))
                        {
                            continue;
                        }
                        if (j < 0 || j >= levelMap.GetLength(1))
                        {
                            continue;
                        }

                        tileToDraw = levelMap[i, j];

                        if (tileToDraw.OnTile is Obstacle)
                        {
                            sb.Draw(tileToDraw.Sprite, tileToDraw.Position, Color.White);
                            sb.Draw(tileToDraw.OnTile.Sprite, tileToDraw.Position, Color.White);
                        }

                        else
                        {
                            sb.Draw(tileToDraw.Sprite, tileToDraw.Position, Color.White);
                        }
                    }
                }

                tileToDraw = levelMap[candleX, candleY];

                if (tileToDraw.OnTile is Obstacle)
                {
                    if (trueCandleStage == 0 &&
                       (candleX >= playerX - 1 && candleX <= playerX + 1 &&
                        candleY >= playerY - 1 && candleY <= playerY + 1))
                    {
                        sb.Draw(tileToDraw.OnTile.Sprite, tileToDraw.Position, Color.White);
                    }

                    else if (trueCandleStage == 0)
                    {
                        sb.Draw(tileToDraw.OnTile.Sprite, tileToDraw.Position, Color.Black);
                    }
                }

                //Making sure tile candle is on also darkens
                else
                {
                    if (trueCandleStage == 0 &&
                       (candleX >= playerX - 1 && candleX <= playerX + 1 &&
                        candleY >= playerY - 1 && candleY <= playerY + 1))
                    {
                        sb.Draw(tileToDraw.Sprite, tileToDraw.Position, Color.White);
                    }

                    else if (trueCandleStage == 0)
                    {
                        sb.Draw(tileToDraw.Sprite, tileToDraw.Position, Color.Black);
                    }
                }

                //Draw Candle
                if (trueCandleStage == 0 && candle.IsHeld == false &&
                   (candleX < playerX - 1 || candleX > playerX + 1 ||
                    candleY < playerY - 1 || candleY > playerY + 1))
                {
                    sb.Draw(candle.Sprite, candle.Position, Color.Black);
                }

                else if ((candleX < playerX - 1 || candleX > playerX + 1 ||
                          candleY < playerY - 1 || candleY > playerY + 1))
                {
                    sb.Draw(candle.Sprite, candle.Position, Color.White);
                }

                else if (candle.IsHeld == false)
                {
                    sb.Draw(candle.Sprite, candle.Position, Color.White);
                }

                //Code to Draw Key goes here

                if (itemX >= candleX - trueCandleStage && itemX <= candleX + trueCandleStage &&
                    itemY >= candleY - trueCandleStage && itemY <= candleY + trueCandleStage)
                {
                    sb.Draw(item.Sprite, item.Position, Color.White);
                }

                else if (itemX >= playerX - 1 && itemX <= playerX + 1 &&
                         itemY >= playerY - 1 && itemY <= playerY + 1)
                {
                    sb.Draw(item.Sprite, item.Position, Color.White);
                }

                //else
                //{
                //    //sb.Draw(item.Sprite, item.Position, Color.Black);
                //}

                //Draw Match
                if (match != null)
                {
                    if (matchX >= candleX - trueCandleStage && matchX <= candleX + trueCandleStage &&
                        matchY >= candleY - trueCandleStage && matchY <= candleY + trueCandleStage)
                    {
                        sb.Draw(match.Sprite, match.Position, Color.White);
                    }

                    else if (matchX >= playerX - 1 && matchX <= playerX + 1 &&
                             matchY >= playerY - 1 && matchY <= playerY + 1)
                    {
                        sb.Draw(match.Sprite, match.Position, Color.White);
                    }
                }

                /*
                if (candle.IsHeld && 
                   (itemX < playerX - candleStage || itemX > playerX + candleStage ||
                    itemY < playerY - candleStage || itemY > playerY + candleStage))
                {
                    sb.Draw(item.Sprite, item.Position, Color.Black);
                }

                else if (itemX < playerX - 1 || itemX > playerX + 1 ||
                         itemY < playerY - 1 || itemY > playerY + 1)
                {
                    sb.Draw(item.Sprite, item.Position, Color.Black);
                }

                else if (item.IsHeld)
                {
                    sb.Draw(item.Sprite, item.Position, Color.White);
                }
                */
            }

            if (godMode)
            {
                trueCandleStage = 200;
            }

            else if (currentGameState == GameState.GamePlay)
            { 
                 if (timer >= 5500)
                 {
                    trueCandleStage = 0;
                 }
                else
                {
                    if (timer < 2000)
                    {
                        trueCandleStage = 3;
                    }

                    if (timer >= 2000 && timer <= 4000)
                    {
                        trueCandleStage = 2;
                    }

                    if (timer > 4000 && timer < 6000)
                    {
                        trueCandleStage = 1;
                    }

                }

                timer++;
            }
        }
    }
}
