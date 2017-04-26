using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infection_1._8._5
{
    class Sprite
    {
        public Texture2D texture;
        public Rectangle rectangle;
        public Vector2 position;
        public Vector2 velocity;
        public Color color;

        public Sprite(Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition, Color newColor)
        {
            texture = newTexture;
            rectangle = newRectangle;
            position = newPosition;
            color = newColor;
        }

        public virtual void Update(GameTime gameTime)
        {
            position.X += velocity.X;
            position.Y += velocity.Y;

            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }
    }
}


