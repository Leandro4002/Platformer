using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global.Blocs {
    class Dirt : Solid {
        #region constructor
        public Dirt(World world) : base(world) {
            textureString = "dirt";
        }
        #endregion
    }
}
