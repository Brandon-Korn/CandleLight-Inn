// Ace Software- Edward Numrich, Matt Ivansek, Jay Miller, Brandon Korn, Billy
// Fox
// Started February 16, 2022
// Candlelight Inn- A puzzle game where you navigate an abandoned hotel, with
// only one candle that is ever burning away for light.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;




namespace Candlelight_Inn
{

    // Determines if the current state of the game is in the menu, game, or game over menu.
    enum GameState { Paused, GamePlay, StartGame, GameOver, TitleScreen, Quit, Settings, LevelSelect, Credits }
    enum Levels { Level1 = 1, Level2, Level3, Level4, Level5, Level6, Level7, Level8};

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Fields
        private Player playerCharacter;
        private LevelManager levelManager;
        private InputManager inputManager;
        private MenuManager menuManager;
        private Song gameplayMusic;
        private GameState currentGameState;
        private Levels currLevel;
        private bool playerMoving;
        private bool playerPushingObstacle;
        private bool exitLocked;
        private string levelFile;
        private const int tileSize = 64;
        private bool godMode;
        private bool[] levelsCleared;


        // Sprites
        private Texture2D testTile;
        private Texture2D obstacleSprite;
        private Texture2D candleSprite;
        private Texture2D keySprite;
        private Texture2D exitSprite;
        private Texture2D playerSprite;
        private SpriteFont arial30;
        private SpriteFont arial12;
        private Texture2D buttonSpriteSheet;
        private Texture2D moveableObstacle;
        private Texture2D candleGoldSprite;
        private Texture2D candleOrangeSprite;
        private Texture2D playerSpritesheet;
        private Texture2D rect;
        private Texture2D doorOpen;
        private Texture2D doorClose;
        private Texture2D LevelButtonSprites;
        private Texture2D volumeButtonSprites;
        private Texture2D bookcaseSprt;
        private Texture2D matchSprite;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            inputManager = new InputManager();
            levelManager = new LevelManager();

            _graphics.PreferredBackBufferWidth = 700;
            _graphics.PreferredBackBufferHeight = 700;
            // middle tile X - 334
            // middle tile y - 334
            _graphics.ApplyChanges();


            currentGameState = GameState.TitleScreen;
            playerMoving = false;
            playerPushingObstacle = false;
            godMode = false;
            levelsCleared = new bool[9]
                        { true, false, false, false, false, false, false, false, false};

            levelFile = "myLevel1.txt";


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            testTile = Content.Load<Texture2D>("Use this carpet");
            playerSprite = Content.Load<Texture2D>("spritesheet of player");
            obstacleSprite = Content.Load<Texture2D>("Crate-1.png");
            keySprite = Content.Load<Texture2D>("key silver");
            candleSprite = Content.Load<Texture2D>("Candle flame blue-1.png (1)");
            arial30 = Content.Load<SpriteFont>("Arial30");
            arial12 = Content.Load<SpriteFont>("Arial12");
            moveableObstacle = Content.Load<Texture2D>("Chair");
            candleGoldSprite = Content.Load<Texture2D>("Candle flame Gold");
            candleOrangeSprite = Content.Load<Texture2D>("Candle flame Orange");
            playerSpritesheet = Content.Load<Texture2D>("spritesheet of player");
            buttonSpriteSheet = Content.Load<Texture2D>("Candlelight Menu updated");
            doorOpen = Content.Load<Texture2D>("open door");
            doorClose = Content.Load<Texture2D>("close door");
            LevelButtonSprites = Content.Load<Texture2D>("LevelSelectButtons");
            volumeButtonSprites = Content.Load<Texture2D>("volume Buttons");
            bookcaseSprt = Content.Load<Texture2D>("bookcase");
            matchSprite = Content.Load<Texture2D>("matchstick");

            gameplayMusic = Content.Load<Song>("The Caretaker-" +
                "All You Are Going to Want to do is Get Back There");
            menuManager = new MenuManager(buttonSpriteSheet, LevelButtonSprites, volumeButtonSprites);

            playerCharacter = new Player(playerSprite,
                334,
                334,
                tileSize,
                tileSize);
            MediaPlayer.Volume = 0.6f;
            MediaPlayer.Play(gameplayMusic);
            MediaPlayer.IsRepeating = true;

            rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });

            //Subscribe methods for volume to events
            menuManager.VolumeUp += this.VolumeRockerUp;
            menuManager.VolumeDown += this.VolumeRockerDown;
        }

        protected override void Update(GameTime gameTime)
        {
            if (currentGameState == GameState.Quit)
            {
                this.Exit();
            }

            inputManager.GetInput();

            switch (currentGameState)
            {
                // While the game is playing
                case GameState.GamePlay:

                    // Saves the current directional key press for this frame

                    MovementDirections movementKey = inputManager.CheckInput();

                    // If the player is already facing that direction, move the player that direction
                    // Only checks to move if the player is not currently moving to make the movement more
                    // intuitive for the player
                    if ((movementKey == playerCharacter.CurrentDirection ||
                        movementKey == MovementDirections.Move) &&
                        !playerMoving)
                    {
                        playerMoving = levelManager.MovePlayer(playerCharacter);
                    }

                    // Update the positon of the player
                    if (playerMoving)
                    {
                        playerMoving = levelManager.UpdateMapPositions(playerCharacter.CurrentDirection);
                    }

                    // Update how the player is facing

                    playerCharacter.PlayerMovement(movementKey);


                    //Update the player's held item
                    inputManager.Hands(playerCharacter, levelManager);


                    if (!levelManager.UnlockExit(playerCharacter,
                        inputManager))
                    {
                        levelsCleared[(int)currLevel] = true;
                        if (currLevel == Levels.Level8)
                        {
                            currentGameState = GameState.GameOver;
                            break;
                        }

                        Nextlevel(currLevel + 1);
                    }

                    break;

                case GameState.TitleScreen:
                    // The level is always reset to Level 1 when you are at the
                    // title screen.
                    //currLevel = Levels.Level1;
                    break;

                // While the game is paused
                case GameState.Paused:
                    break;

                case GameState.LevelSelect:
                    Levels returned = menuManager.PickLevel(inputManager, levelsCleared, godMode);
                    if (returned != 0 && (int)returned < 9)
                    {
                        Nextlevel(returned);
                    }
                    break;

                case GameState.StartGame:









                    levelManager.LoadLevel(levelFile, testTile, obstacleSprite,
                        candleSprite, keySprite, doorClose, moveableObstacle,
                        matchSprite, bookcaseSprt, playerCharacter);

                    //while (playerCharacter.Position != 
                    //    levelManager.LevelMap[levelManager.PlayerX, levelManager.PlayerY].Position)
                    //{
                    //    playerCharacter.UpdatePosition(levelManager, playerPushingObstacle);
                    //}
                    currentGameState = GameState.GamePlay;
                    break;
            }

            currentGameState = inputManager.ChangeMenu(currentGameState);
            currentGameState = menuManager.UpdateButtons(currentGameState, godMode, inputManager, out godMode);



            inputManager.SaveInput();



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            switch (currentGameState)
            {
                case GameState.TitleScreen:

                    menuManager.Display(_spriteBatch, currentGameState, godMode, levelsCleared);
                    
                    break;

                case GameState.Credits:
                    menuManager.Display(_spriteBatch, currentGameState, godMode, levelsCleared);

                    _spriteBatch.DrawString(arial12,
                                            "Ace Software",
                                            new Vector2(120, 100),
                                            Color.White);
                    _spriteBatch.DrawString(arial30,
                                            "Programmers: Brandon Korn, \nEddie Numrich, Jay Miller, \nBilly Fox, Matt Ivansek\n\nArt: Jay Miller",
                                            new Vector2(75, 250),
                                            Color.White);
                    //_spriteBatch.DrawString(arial30,
                    //                        "",
                    //                        new Vector2(290, 275),
                     //                       Color.White);
                    break;
                case GameState.GamePlay:
                    //levelManager.DrawMap(_spriteBatch);

                    levelManager.UpdateLight(playerCharacter,
                                             _spriteBatch,
                                            currentGameState, godMode);

                    playerCharacter.DrawAnimated(_spriteBatch, levelManager.WalkCycleCounter);

                    menuManager.Display(_spriteBatch, currentGameState, godMode, levelsCleared);

                    // TEST PRINT TO KNOW THE ORIENTATION
                    //_spriteBatch.DrawString(testSpriteFont,
                    //    playerCharacter.CurrentDirection.ToString(),
                    //    new Vector2(350, 650), Color.White);

                    //_spriteBatch.DrawString(testSpriteFont, 
                    //    $"{levelManager.IsLocked}", new Vector2(100, 50), 
                    //    Color.White);

                    if (playerCharacter.HeldItem != null)
                    {
                        playerCharacter.HeldItem.Draw(_spriteBatch);
                    }

                    break;
                case GameState.LevelSelect:
                    menuManager.Display(_spriteBatch, currentGameState, godMode, levelsCleared);
                    _spriteBatch.DrawString(arial30, "CHOOSE A LEVEL", new Vector2(175, 55), Color.White);
                    break;

                case GameState.Paused:
                    menuManager.Display(_spriteBatch, currentGameState, godMode, levelsCleared);
                    break;

                case GameState.Settings:
                    _spriteBatch.Draw(rect, new Rectangle(50, 50, 600, 305), Color.Beige);
                    _spriteBatch.DrawString(arial30, "CONTROLS:", new Vector2(240, 65), Color.Black);
                    _spriteBatch.DrawString(arial30, "W-A-S-D to move", new Vector2(100, 140), Color.Black);
                    _spriteBatch.DrawString(arial30, "SPACE to interact with items", new Vector2(100, 215), Color.Black);
                    _spriteBatch.DrawString(arial30, "Q - Pause         R - Reset level", new Vector2(100, 290), Color.Black);

                    _spriteBatch.DrawString(arial30, "VOLUME:", new Vector2(260, 440), Color.White);
                    _spriteBatch.DrawString(arial30, (Math.Round(MediaPlayer.Volume, 2) * 100).ToString() + "%", 
                                                new Vector2(305, 510), Color.MediumVioletRed);

                    menuManager.Display(_spriteBatch, currentGameState, godMode, levelsCleared);
                    break;

                case GameState.GameOver:
                    _spriteBatch.DrawString(arial30, "YOU WIN!!\nCongradulations!:", new Vector2(180, 240), Color.White);
                    menuManager.Display(_spriteBatch, currentGameState, godMode, levelsCleared);
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void Nextlevel(Levels desiredLevel)
        {
            levelManager.IsLocked = true;
            currLevel = desiredLevel;
            levelFile = "myLevel" + (int)currLevel + ".txt";
            currentGameState = GameState.StartGame;
        }

        private void VolumeRockerUp()
        {
            
            MediaPlayer.Volume += 0.2f;
        }

        private void VolumeRockerDown()
        {
            MediaPlayer.Volume -= 0.2f;
        }
    }
}
