using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global.Control {
    abstract class Control_Abstract {
        public enum status { normal, hovered, active };
        public Texture2D texture;
        public SpriteFont font;
        public int x, y;
        public int wth, hgt;
        public string text;

        public Control_Abstract(Texture2D texture, SpriteFont font, int x, int y, int wth, int hgt, string text) {
            this.texture = texture;
            this.font = font;
            this.x = x;
            this.y = y;
            this.wth = wth;
            this.hgt = hgt;
            this.text = text;
        }

        public virtual void Update(KeyboardState keyboardState, MouseState mouseState, float dt) {

        }

        public virtual void Draw(SpriteBatch spritebatch) {

        }
    }
}
