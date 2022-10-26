using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Candlelight_Inn
{
    /// <summary>
    /// Delegate for volume changes
    /// </summary>
    public delegate void Volume();

    class MenuManager
    {
        // Buttons
        private Button titleCard;
        private Button unpauseButton;
        private Button pauseHomeButton;
        private Button startButton;
        private Button optionsButton;
        private Button exitButton;
        private Button creditsButton;
        private Button backOptionsButton;
        private Button backLevelButton;
        private Button godModeOnButton;
        private Button godModeOffButton;
        private Button volumeDown;
        private Button volumeUp;
        private List<Button> levelSelectButtons;

        //Making two events
        public event Volume VolumeUp;

        public event Volume VolumeDown;

        // Create the Menu Manager by giving it all buttons through a button sprite sheet
        // with 1 column of unhovered button sprites and a second column of the hovered sprites
        // Create each instance of the buttons with the correct position and what row that specific
        // button sprite is on in the sprite sheet
        public MenuManager(Texture2D buttonSheet, 
            Texture2D levelButtonSheet, 
            Texture2D volumeButtonSheet)
        {
            // Pause menu sprites.
            unpauseButton = new Button(buttonSheet,
                new Rectangle(200, 200, 300, 100),
                new Rectangle(6555, 3480, 1885, 540),
                new Rectangle(200, 200, 300, 100),
                new Rectangle(1475, 2005, 2045, 485),
                false);
            pauseHomeButton = new Button(buttonSheet,
                new Rectangle(200, 350, 300, 100),
                new Rectangle(1940, 6525, 1115, 445),
                new Rectangle(200, 350, 300, 100),
                new Rectangle(1775, 3580, 1450, 335),
                false);

            // Title screen button sprites.
            titleCard = new Button(buttonSheet,
                new Rectangle(100, 55, 500, 106),
                new Rectangle(0, 221, 5000, 1055),
                new Rectangle(100, 55, 500, 106),
                new Rectangle(0, 221, 5000, 1055),
                false);
            startButton = new Button(buttonSheet,
                new Rectangle(256, 206, 189, 54),
                new Rectangle(6555, 3480, 1885, 540),
                new Rectangle(248, 206, 205, 49),
                new Rectangle(1475, 2005, 2045, 485),
                false);
            optionsButton = new Button(buttonSheet,
                new Rectangle(267, 260, 166, 51),
                new Rectangle(1670, 4995, 1660, 505),
                new Rectangle(250, 260, 200, 50),
                new Rectangle(6500, 2000, 1995, 500),
                false);
            creditsButton = new Button(buttonSheet,
                new Rectangle(274, 315, 153, 55),
                new Rectangle(6735, 4975, 1525, 545),
                new Rectangle(251, 315, 199, 49),
                new Rectangle(6505, 510, 1985, 485),
                false);
            exitButton = new Button(buttonSheet,
                new Rectangle(295, 375, 111, 45),
                new Rectangle(1940, 6525, 1115, 445),
                new Rectangle(278, 375, 145, 34),
                new Rectangle(1775, 3580, 1450, 335),
                false);

            // Other buttons.
            backOptionsButton = new Button(buttonSheet,
                new Rectangle(272, 600, 157, 48),
                new Rectangle(6718, 8993, 1563, 472),
                new Rectangle(251, 600, 199, 48),
                new Rectangle(6506, 7382, 1987, 472),
                false);
            backLevelButton = new Button(buttonSheet,
                new Rectangle(272, 470, 157, 48),
                new Rectangle(6718, 8993, 1563, 472),
                new Rectangle(251, 470, 199, 48),
                new Rectangle(6506, 7382, 1987, 472),
                false);
            godModeOnButton = new Button(buttonSheet,
                new Rectangle(250, 371, 200, 48),
                new Rectangle(6503, 6354, 1992, 479),
                new Rectangle(273, 371, 154, 41),
                new Rectangle(6733, 8263, 1532, 402),
                true);
            godModeOffButton = new Button(buttonSheet,
                new Rectangle(250, 371, 200, 48),
                new Rectangle(0, 9479, 1987, 521),
                new Rectangle(273, 371, 154, 41),
                new Rectangle(0, 9779, 1987, 521),
                true);
            volumeDown = new Button(volumeButtonSheet,
                new Rectangle(230, 500, 32, 65),
                new Rectangle(146, 213, 157, 323),
                new Rectangle(0, 630, 32, 65),
                new Rectangle(146, 213, 157, 323),
                true);
            volumeUp = new Button(volumeButtonSheet,
                new Rectangle(436, 500, 32, 65),
                new Rectangle(450, 216, 157, 323),
                new Rectangle(36, 630, 32, 65),
                new Rectangle(450, 216, 157, 323),
                true);

            levelSelectButtons = new List<Button>();
            levelSelectButtons.Add(new Button(levelButtonSheet,
                                            new Rectangle(80, 150, 100, 100),
                                            new Rectangle(0, 0, 70, 56),
                                            new Rectangle(80, 150, 100, 100),
                                            new Rectangle(70, 0, 70, 56),
                                            false));
            levelSelectButtons.Add(new Button(levelButtonSheet,
                                            new Rectangle(220, 150, 100, 100),
                                            new Rectangle(0, 56 * 1, 70, 56), 
                                            new Rectangle(220, 150, 100, 100),
                                            new Rectangle(70, 56 * 1, 70, 56),
                                            false));
            levelSelectButtons.Add(new Button(levelButtonSheet,
                                            new Rectangle(360, 150, 100, 100),
                                            new Rectangle(0, 56 * 2, 70, 56),
                                            new Rectangle(360, 150, 100, 100),
                                            new Rectangle(70, 56 * 2, 70, 56),
                                            false));
            levelSelectButtons.Add(new Button(levelButtonSheet,
                                            new Rectangle(500, 150, 100, 100),
                                            new Rectangle(0, 56 * 3, 70, 56),
                                            new Rectangle(500, 150, 100, 100),
                                            new Rectangle(70, 56 * 3, 70, 56),
                                            false));
            levelSelectButtons.Add(new Button(levelButtonSheet,
                                            new Rectangle(80, 300, 100, 100),
                                            new Rectangle(0, 56 * 4, 70, 56),
                                            new Rectangle(80, 300, 100, 100),
                                            new Rectangle(70, 56 * 4, 70, 56),
                                            false));
            levelSelectButtons.Add(new Button(levelButtonSheet,
                                            new Rectangle(220, 300, 100, 100),
                                            new Rectangle(0, 56 * 5, 70, 56),
                                            new Rectangle(220, 300, 100, 100),
                                            new Rectangle(70, 56 * 5, 70, 56),
                                            false));
            levelSelectButtons.Add(new Button(levelButtonSheet,
                                            new Rectangle(360, 300, 100, 100),
                                            new Rectangle(0, 56 * 6, 70, 56),
                                            new Rectangle(360, 300, 100, 100),
                                            new Rectangle(70, 56 * 6, 70, 56),
                                            false));
            levelSelectButtons.Add(new Button(levelButtonSheet,
                                            new Rectangle(500, 300, 100, 100),
                                            new Rectangle(0, 56 * 7, 70, 56),
                                            new Rectangle(500, 300, 100, 100),
                                            new Rectangle(70, 56 * 7, 70, 56),
                                            false));
            /*levelSelectButtons.Add(new Button(levelButtonSheet,
                                            new Rectangle(400, 300, 100, 100),
                                            new Rectangle(0, 56 * 8, 70, 56),
                                            new Rectangle(400, 300, 100, 100),
                                            new Rectangle(70, 56 * 8, 70, 56),
                                            false));*/
            levelSelectButtons.Add(new Button(levelButtonSheet,
                                            new Rectangle(510, 300, 100, 100),
                                            new Rectangle(0, 56 * 9, 70, 56),
                                            new Rectangle(510, 300, 100, 100),
                                            new Rectangle(70, 56 * 9, 70, 56),
                                            false));
        }
         
        public void Display(SpriteBatch sb, GameState currentState, 
                            bool godMode, bool[] levelsCleared)
        {
            switch (currentState)
            {
                case GameState.TitleScreen:
                    titleCard.Draw(sb);
                    startButton.Draw(sb);
                    optionsButton.Draw(sb);
                    creditsButton.Draw(sb);
                    exitButton.Draw(sb);
                    break;

                case GameState.GamePlay:
                    break;

                case GameState.Credits:
                    backLevelButton.Draw(sb);
                    break;

                case GameState.Paused:
                    unpauseButton.Draw(sb);
                    pauseHomeButton.Draw(sb);
                    break;

                case GameState.Settings:
                    backOptionsButton.Draw(sb);
                    if (godMode)
                    {
                        godModeOnButton.Draw(sb);
                    }
                    else
                    {
                        godModeOffButton.Draw(sb);
                    }
                    volumeDown.Draw(sb);
                    volumeUp.Draw(sb);
                    break;

                case GameState.LevelSelect:
                    backLevelButton.Draw(sb);
                    for (int i = 0; i < levelSelectButtons.Count - 1; i++)
                    {
                        if (levelsCleared[i] == true || godMode)
                        {
                            levelSelectButtons[i].Draw(sb);
                        }
                        else
                        {
                            levelSelectButtons[levelSelectButtons.Count - 1].DrawNewLocation(sb, 
                                                            levelSelectButtons[i].Position);
                        }
                    }
                    break;

                case GameState.GameOver:
                    backLevelButton.Draw(sb);
                    break;
            }
        }

        public GameState UpdateButtons(GameState currentState, bool godMode,
                                        InputManager inputManager, out bool futureGodMode)
        {
            switch (currentState)
            {
                case GameState.TitleScreen:
                    futureGodMode = godMode;
                    if (inputManager.SingleButtonPress(startButton))
                    {
                        return GameState.LevelSelect;
                    }
                    if (inputManager.SingleButtonPress(optionsButton))
                    {
                        return GameState.Settings;
                    }
                    if (inputManager.SingleButtonPress(exitButton))
                    {
                        return GameState.Quit;
                    }
                    if (inputManager.SingleButtonPress(creditsButton))
                    {
                        return GameState.Credits;
                    }
                    break;

                case GameState.Credits:
                    futureGodMode = godMode;
                    if (inputManager.SingleButtonPress(backLevelButton))
                    {
                        return GameState.TitleScreen;
                    }
                    break;

                case GameState.GamePlay:
                    futureGodMode = godMode;
                    break;

                case GameState.Paused:
                    futureGodMode = godMode;
                    if (inputManager.SingleButtonPress(unpauseButton))
                    {
                        return GameState.GamePlay;
                    }
                    if (inputManager.SingleButtonPress(pauseHomeButton))
                    {
                        return GameState.TitleScreen;
                    }
                    break;

                case GameState.Settings:
                    futureGodMode = godMode;
                    if (inputManager.SingleButtonPress(backOptionsButton))
                    {
                        return GameState.TitleScreen;
                    }
                    if (inputManager.SingleButtonPress(godModeOffButton) && !godMode)
                    {
                        futureGodMode = true;
                        return GameState.Settings;
                    }
                    if (inputManager.SingleButtonPress(godModeOnButton) && godMode)
                    {
                        futureGodMode = false;
                        return GameState.Settings;
                    }
                    
                    
                    //If the Up rocker is pressed, raise volume
                    if(inputManager.SingleButtonPress(volumeUp) &&
                       volumeUp != null)
                    {
                        VolumeUp();
                        return GameState.Settings;
                    }

                    //If down rocker is pressed, lower volume
                    if(inputManager.SingleButtonPress(volumeDown) &&
                       VolumeDown != null)
                    {
                        VolumeDown();
                        return GameState.Settings;
                    }

                    break;

                case GameState.LevelSelect:
                    futureGodMode = godMode;
                    if (inputManager.SingleButtonPress(backLevelButton))
                    {
                        return GameState.TitleScreen;
                    }
                    break;

                case GameState.GameOver:
                    futureGodMode = godMode;
                    if (inputManager.SingleButtonPress(backLevelButton))
                    {
                        return GameState.TitleScreen;
                    }
                    break;


            }
            futureGodMode = godMode;
            return currentState;
        }

        public Levels PickLevel(InputManager inputManager, bool[] levelsReached, bool godMode)
        {
            for (int i = 0; i < levelSelectButtons.Count - 1; i++)
            {
                if (inputManager.SingleButtonPress(levelSelectButtons[i]) &&
                    (levelsReached[i] == true || godMode))
                {
                    return (Levels)(i + 1);
                }
            }
            return (Levels)0;
        }
    }
}
