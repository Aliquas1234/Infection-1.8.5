using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infection_1._8._5
{
    class Character : Sprite
    {
        Background _background;
        public int encounterRate;
        public bool bossFight = false;

        public Character(Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition, Color newColor, Background newBackground) : base(newTexture, newRectangle, newPosition, newColor)
        {
            texture = newTexture;
            rectangle = newRectangle;
            position = newPosition;
            _background = newBackground;
            color = newColor;

            //speed of Character's movement
            velocity.X = 3;
            velocity.Y = 3;
        }

        public void Update(GameTime gameTime, Map map, Sprite boss)
        {
            Random rnd = new Random();

            //Updating rectangle coords
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;

            #region Movement Input

            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
            {
                position.Y -= velocity.Y;
                encounterRate = rnd.Next(1, 500);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
            {
                position.X -= velocity.X;
                encounterRate = rnd.Next(1, 500);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
            {
                position.Y += velocity.Y;
                encounterRate = rnd.Next(1, 500);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
            {
                position.X += velocity.X;
                encounterRate = rnd.Next(1, 500);
            }

            #endregion

            #region Screen Parameters
            if (position.Y <= 60)
                position.Y = 60;

            else if (position.Y >= 425 - rectangle.Height)
                position.Y = 425 - rectangle.Height;
            #endregion

            #region collision

            if (rectangle.Intersects(boss.rectangle))
                bossFight = true;

            #endregion
        }
    }
}


