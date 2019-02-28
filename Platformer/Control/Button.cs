using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global.Control {
    class Button : Control_Abstract {
        
        public status state;
        public Action OnClick;

        public Button(Texture2D texture, SpriteFont font, int x, int y, int wth, int hgt, string text, Action OnClick) : base (texture, font, x, y, wth, hgt, text) {
            this.OnClick = OnClick;
        }

        public override void Update(KeyboardState keyboardState, MouseState mouseState, float dt) {
            if (mouseState.X > x && mouseState.X < x + wth &&
                mouseState.Y > y && mouseState.Y < y + hgt) {
                if (mouseState.LeftButton == ButtonState.Pressed) {
                    if (state == status.hovered) {
                        OnClick();
                        state = status.active;
                    }
                } else {
                    state = status.hovered;
                }
                Mouse.SetCursor(MouseCursor.Hand);
            } else {
                state = status.normal;
            }
        }

        public override void Draw(SpriteBatch spritebatch) {
            switch (state) {
                case status.normal:
                    spritebatch.Draw(texture, new Vector2(x, y), Color.LightGray);
                    break;
                case status.hovered:
                    spritebatch.Draw(texture, new Vector2(x, y), Color.DarkGray);
                    break;
                case status.active:
                    spritebatch.Draw(texture, new Vector2(x, y), Color.Gray);
                    break;
            }

            spritebatch.DrawString(font, text, new Vector2(x+wth/2 - font.MeasureString(text).X/2, y+hgt/ 2 - font.MeasureString(text).Y/2), Color.Black);
        }
    }
}
