using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global.Control {
    class Button {
        public enum status { normal, hovered, pressed };

        public ContentOrganizer content;
        public int x, y;
        public int wth, hgt;
        public status state;
        public string text;
        public Action OnClick;

        public Button(ContentOrganizer content, int x, int y, int wth, int hgt, string text, Action OnClick) {
            this.content = content;
            this.x = x;
            this.y = y;
            this.wth = wth;
            this.hgt = hgt;
            this.text = text;
            this.OnClick = OnClick;
        }

        public void Update(MouseState mouseState) {
            if (mouseState.X > x && mouseState.X < x + wth &&
                mouseState.Y > y && mouseState.Y < y + hgt) {
                if (mouseState.LeftButton == ButtonState.Pressed) {
                    if (state != status.pressed) {
                        OnClick();
                        state = status.pressed;
                    }
                } else {
                    state = status.hovered;
                }
            } else {
                state = status.normal;
            }
        }

        public void Draw(SpriteBatch spritebatch) {
            switch (state) {
                case status.normal:
                    spritebatch.Draw(content.textures["rect,"+wth+","+hgt], new Vector2(x, y), Color.LightGray);
                    break;
                case status.hovered:
                    spritebatch.Draw(content.textures["rect," + wth + "," + hgt], new Vector2(x, y), Color.DarkGray);
                    break;
                case status.pressed:
                    spritebatch.Draw(content.textures["rect," + wth + "," + hgt], new Vector2(x, y), Color.Gray);
                    break;
            }

            spritebatch.DrawString(content.fonts["consolas"], text, new Vector2(x+wth/2 - content.fonts["consolas"].MeasureString(text).X/2, y+hgt/ 2 - content.fonts["consolas"].MeasureString(text).Y/2), Color.Black);
        }
    }
}
