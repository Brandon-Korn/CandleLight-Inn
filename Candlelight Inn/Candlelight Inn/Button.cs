using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Candlelight_Inn
{
    class Button
    {
        // Fields
        private Texture2D spriteSheet;
        private Rectangle position;
        private Rectangle sprite;
        private Rectangle togglePosition;
        private Rectangle toggleSprite;
        private bool toggleButton;


        public Rectangle Position
        {
            get { return position; }
        }

        public Button(Texture2D buttonSpriteSheet, 
            Rectangle position, 
            Rectangle sprite,
            Rectangle togglePosition,
            Rectangle toggleSprite,
            bool toggle)
        {
            spriteSheet = buttonSpriteSheet;
            this.position = position;
            this.sprite = sprite;
            this.togglePosition = togglePosition;
            this.toggleSprite = toggleSprite;
            toggleButton = toggle;
        }

        /// <summary>
        /// Draws the button to the screen
        /// Draws the correct sprite of the button depending on where the mouse is
        /// </summary>
        /// <param name="sb">The SpriteBatch object for drawing</param>
        public void Draw(SpriteBatch sb)
        {
            MouseState mState = Mouse.GetState();

            Point mousePoint = new Point(mState.X, mState.Y);

            if (toggleButton)
            {
                sb.Draw(spriteSheet,
                        position,
                        sprite,
                        Color.White);
            }
            // Checks if the moues is within the rectangle of the 
            // button, and displays the hovered sprite is it is
            else if (position.Contains(mousePoint))
            {
                sb.Draw(spriteSheet, 
                        togglePosition,
                        toggleSprite,
                        Color.White);
            }
            else
            {
                sb.Draw(spriteSheet, 
                    position,
                    sprite,
                    Color.White);
            }
        }

        public void DrawNewLocation(SpriteBatch sb, Rectangle newPosition)
        {
            {
                sb.Draw(spriteSheet,
                    newPosition,
                    sprite,
                    Color.White);
            }
        }
    }
}

