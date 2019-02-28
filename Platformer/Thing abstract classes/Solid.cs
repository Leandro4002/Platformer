using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global {
    abstract class Solid : Bloc {
        #region constructor
        protected Solid(World world) : base(world) {
            
        }
        #endregion

        #region public methods
        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, world.pos + pos, color);
            if (world.displayDebug) {
                spriteBatch.Draw(world.content.textures["rect,50,50,3"], world.pos + pos, DEBUG_COLOR);
                spriteBatch.DrawString(world.content.fonts["consolas"], x/SIZE + "\n" + y/SIZE, world.pos + pos + new Vector2(2, 2), DEBUG_COLOR);
            }
        }
        #endregion
    }
}
