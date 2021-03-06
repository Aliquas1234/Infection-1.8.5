﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infection_1._8._5
{
    class Background : Sprite
    {
        public Background(Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition, Color newColor) : base(newTexture, newRectangle, newPosition, newColor)
        {
            texture = newTexture;
            rectangle = newRectangle;
            position = newPosition;
            color = newColor;
        }

        public override void Update(GameTime gameTime)
        {
            position.X += velocity.X;
            position.Y += velocity.Y;

            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
        }
    }
}


