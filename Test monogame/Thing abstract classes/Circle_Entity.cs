using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Global {
    abstract class Circle_Entity : Entity {
        #region protected attributes

        #endregion

        #region public accessors
        public float radius { get; protected set; }
        #endregion

        #region constructor
        public Circle_Entity(World world, Vector2 pos, float radius, string textureString) : base(world, pos, textureString) {
            this.world = world;
            this.pos = pos;
            this.radius = radius;
            this.textureString = textureString;
        }
        #endregion
    }
}
