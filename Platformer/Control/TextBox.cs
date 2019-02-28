using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global.Control {
    //Only work well with unicode font !
    //If not, the cursor might not be in the correct place
    class TextBox : Control_Abstract {
        int _cursorPosition, _textPosition;
        public KeyboardState oldKeyboardState;
        public string textShown;
        public int thick, numOfVisibleCharacter;
        public float characterWidth, writableWidth;
        public float cursorBlinkDelay, cursorBlinkDelayHandler; //ms
        public float cursorMoveDelay, cursorMoveDelayHandler; //ms
        public float cursorFirstMoveDelay, cursorFirstMoveDelayHandler; //ms
        public bool cursorVisible, firstCursorMove;
        public bool cursorFirstMoveDelayCompleted, cursorMoveDelayCompleted, movingKeyPressed, movingKeyPressedOld;
        public status state;

        public int cursorPosition {
            get { return _cursorPosition; }
            set {
                cursorBlinkDelayHandler = 0;
                cursorVisible = true;
                if (value > _cursorPosition) {
                    if (value > textPosition + numOfVisibleCharacter) {
                        textPosition += value - _cursorPosition;
                    }
                } else if (value < _cursorPosition) {
                    if (value < textPosition) {
                        textPosition -= _cursorPosition - value;
                    }
                }
                _cursorPosition = value < 0 ? 0 : (value > text.Length ? text.Length : value);
            }
        }
        public int textPosition {
            get { return _textPosition; }
            set {
                if (text.Length <= numOfVisibleCharacter) return;
                _textPosition = value < 0 ? 0 : (value > text.Length - numOfVisibleCharacter ? text.Length - numOfVisibleCharacter : value);
                textShown = text.Substring(_textPosition, numOfVisibleCharacter);
            }
        }

        public TextBox(Texture2D texture, SpriteFont font, int x, int y, int wth, int hgt, string text, int thick = 4) : base(texture, font, x, y, wth, hgt, text) {
            this.thick = thick;
            this.cursorBlinkDelay = 600;
            this.cursorMoveDelay = 40;
            this.cursorFirstMoveDelay = 400;
            this.characterWidth = font.MeasureString("X").X;
            this.writableWidth = (wth - thick * 2);
            this.numOfVisibleCharacter = (int)(writableWidth / characterWidth) + 1;
            this.firstCursorMove = true;
            this.cursorFirstMoveDelayCompleted = true;
            this.cursorMoveDelayCompleted = true;

            float textWidth = font.MeasureString(text).X;

            if (textWidth > writableWidth) {
                textPosition = 0;
            } else {
                textShown = text;
            }
        }

        public override void Update(KeyboardState keyboardState, MouseState mouseState, float dt) {
            //Timer
            float dtMilliseconds = dt * 1000;
            cursorBlinkDelayHandler += dtMilliseconds;
            cursorMoveDelayHandler += dtMilliseconds;
            cursorFirstMoveDelayHandler += dtMilliseconds;
            
            //Mouse
            if (mouseState.X > x && mouseState.X < x + wth &&
                mouseState.Y > y && mouseState.Y < y + hgt) {
                if (mouseState.LeftButton == ButtonState.Pressed) {
                    if (state == status.hovered) {
                        cursorBlinkDelayHandler = 0;
                        cursorVisible = true;
                        //cursorPosition = mouseState.X - x;
                        state = status.active;
                    }
                } else if (state != status.active) {
                    state = status.hovered;
                }
                Mouse.SetCursor(MouseCursor.IBeam);
            } else {
                if (mouseState.LeftButton == ButtonState.Pressed || state != status.active) {
                    state = status.normal;
                }
            }

            //Cursor blink
            if (cursorBlinkDelayHandler >= cursorBlinkDelay) {
                cursorBlinkDelayHandler = 0;
                cursorVisible = !cursorVisible;
            }

            //Keyboard
            if (state == status.active) {
                foreach (Keys k in keyboardState.GetPressedKeys()) {
                    Console.WriteLine(k);
                }

                bool oldKeyLeft = oldKeyboardState.IsKeyDown(Keys.Left);
                bool oldKeyRight = oldKeyboardState.IsKeyDown(Keys.Right);
                bool keyLeft = keyboardState.IsKeyDown(Keys.Left);
                bool keyRight = keyboardState.IsKeyDown(Keys.Right);

                movingKeyPressed = keyLeft | keyRight;

                if (keyLeft & keyRight) {
                    firstCursorMove = true;
                    cursorFirstMoveDelayCompleted = true;
                    return;
                }

                if (movingKeyPressed && !movingKeyPressedOld || !movingKeyPressed ||
                    keyLeft & oldKeyRight ||
                    keyRight & oldKeyLeft) {
                    firstCursorMove = true;
                    cursorFirstMoveDelayCompleted = true;
                }

                if (movingKeyPressed) { 
                    if (firstCursorMove) {
                        if (cursorFirstMoveDelayCompleted) {
                            cursorFirstMoveDelayHandler = 0;
                            cursorFirstMoveDelayCompleted = false;
                            if (keyLeft) {
                                cursorPosition--;
                            } else if (keyRight) {
                                cursorPosition++;
                            }
                        } else {
                            if (cursorFirstMoveDelayHandler >= cursorFirstMoveDelay) {
                                firstCursorMove = false;
                                cursorFirstMoveDelayCompleted = true;
                                cursorMoveDelayHandler = 0;
                            }
                        }
                    } else {
                        if (cursorMoveDelayHandler >= cursorMoveDelay) {
                            cursorMoveDelayHandler = 0;
                            if (keyLeft) {
                                cursorPosition--;
                            } else if (keyRight) {
                                cursorPosition++;
                            }
                        }
                    }
                }
            }

            oldKeyboardState = keyboardState;
            movingKeyPressedOld = movingKeyPressed;
        }

        public override void Draw(SpriteBatch spritebatch) {

            //Border
            switch (state) {
                case status.normal:
                    spritebatch.Draw(texture, new Rectangle(x, y, wth, hgt), Color.LightGray);
                    break;
                case status.hovered:
                    spritebatch.Draw(texture, new Rectangle(x, y, wth, hgt), Color.DarkGray);
                    break;
                case status.active:
                    spritebatch.Draw(texture, new Rectangle(x, y, wth, hgt), Color.Gray);
                    break;
            }
            
            spritebatch.Draw(texture, new Rectangle(x+thick, y+thick, wth-thick*2, hgt-thick*2), Color.White);
            spritebatch.DrawString(font, textShown, new Vector2(x + thick, y + hgt/2 - font.MeasureString(text).Y/2 + thick), Color.Black);
            if (cursorVisible && state == status.active) {
                spritebatch.Draw(texture, new Rectangle(x+thick+(int)font.MeasureString(text.Substring(0,(cursorPosition-textPosition))).X, y+thick, 2, hgt-thick*2), Color.Black);
            }
        }
    }
}
