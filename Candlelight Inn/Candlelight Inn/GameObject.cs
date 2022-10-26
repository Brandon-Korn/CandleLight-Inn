// Authors: Brandon Korn
// This class contains some of the basic items needed for a game object.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Candlelight_Inn
{
    /// <summary>
    /// An Abstract class for a Game Object
    /// </summary>
    abstract class GameObject
    {
        // Fields
        protected Texture2D sprite;
        protected Rectangle position;

        //Properties
        /// <summary>
        /// Allows for the getting of the Object's position rectangle
        /// </summary>
        public virtual Rectangle Position
        {
            get { return position; }
        }

        /// <summary>
        /// Allows the getting and changing of the X value of the Object
        /// </summary>
        public int X
        {
            get { return position.X; }
            
            set { position.X = value; }
        }

        /// <summary>
        /// Allows for the getting and changing of the Y value of the Object
        /// </summary>
        public int Y
        {
            get { return position.Y; }

            set { position.Y = value; }
        }

        public Texture2D Sprite
        {
            get { return sprite; }
        }

        /// <summary>
        /// Constructor using a Texture2D sprite and Rectangle Position
        /// </summary>
        /// <param name="sprite">A Texture2D Sprite</param>
        /// <param name="position">A Rectangle position</param>
        public GameObject(Texture2D sprite, Rectangle position)
        {
            this.sprite = sprite;
            this.position = position;
        }

        /// <summary>
        /// Constructor using a Texture2D sprite and the ints needed for a rectangle object
        /// </summary>
        /// <param name="sprite">A Texture2D Sprite</param>
        /// <param name="position">A Rectangle position</param>
        public GameObject(Texture2D sprite, int x, int y, int width, int height)
        {
            this.sprite = sprite;
            position = new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// An abstract Draw method
        /// </summary>
        /// <param name="sb">A Sprite Batch Object</param>
        public abstract void Draw(SpriteBatch sb); 
    }
}
