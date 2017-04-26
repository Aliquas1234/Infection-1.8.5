using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infection_1._8._5
{
    class UserInterface : Background
    {
        public int cursorPosition = 1;
        ContentManager Content;
        public Sprite cursor;
        int UInumber;
        bool wWaiting;
        bool sWaiting;
        bool wWaitingC;
        bool sWaitingC;

        public UserInterface(Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition, Color newColor, ContentManager newContent, int newUInumber) : base(newTexture, newRectangle, newPosition, newColor)
        {
            texture = newTexture;
            rectangle = newRectangle;
            position = newPosition;
            color = newColor;
            Content = newContent;
            cursor = new Sprite(Content.Load<Texture2D>("menuCursor"), new Rectangle(0, 0, 15, 20), new Vector2(0, 0), Color.White);
            cursorPosition = 1;
            UInumber = newUInumber;
        }

        public override void Update(GameTime gameTime)
        {
            cursor.rectangle.X = (int)cursor.position.X;
            cursor.rectangle.Y = (int)cursor.position.Y;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                wWaiting = true;

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                sWaiting = true;

            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                wWaitingC = true;

            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                sWaitingC = true;

            if (Keyboard.GetState().IsKeyUp(Keys.W) && wWaiting)
            {
                cursorPosition--;
                wWaiting = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.S) && sWaiting)
            {
                cursorPosition++;
                sWaiting = false;
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Released && wWaitingC)
            {
                cursorPosition--;
                wWaitingC = false;
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Released && sWaitingC)
            {
                cursorPosition--;
                sWaitingC = false;
            }

            switch (UInumber)
            {
                //Pause Menu
                case 1:

                    if (cursorPosition < 1)
                        cursorPosition = 4;

                    if (cursorPosition > 4)
                        cursorPosition = 1;

                    switch (cursorPosition)
                    {
                        case 1:
                            cursor.position = new Vector2(328, 83);
                            break;

                        case 2:
                            cursor.position = new Vector2(322, 183);
                            break;

                        case 3:
                            cursor.position = new Vector2(300, 283);
                            break;

                        case 4:
                            cursor.position = new Vector2(328, 383);
                            break;
                    }
                    break;

                //Battle UI
                case 2:

                    if (cursorPosition < 1)
                        cursorPosition = 3;

                    if (cursorPosition > 3)
                        cursorPosition = 1;

                    switch (cursorPosition)
                    {
                        case 1:
                            cursor.position = new Vector2(680, 70);
                            break;

                        case 2:
                            cursor.position = new Vector2(680, 180);
                            break;

                        case 3:
                            cursor.position = new Vector2(680, 290);
                            break;
                    }

                    break;

                //Hero attacking
                case 3:

                    if (cursorPosition < 1)
                        cursorPosition = 4;

                    if (cursorPosition > 4)
                        cursorPosition = 1;

                    switch (cursorPosition)
                    {
                        case 1:
                            cursor.position = new Vector2(680, 70);
                            break;

                        case 2:
                            cursor.position = new Vector2(680, 140);
                            break;

                        case 3:
                            cursor.position = new Vector2(680, 220);
                            break;

                        case 4:
                            cursor.position = new Vector2(680, 290);
                            break;
                    }

                    break;
            }
        }
    }
}

