using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Global.Meteo {
    [Serializable]
    abstract class Meteo_abstract {
        #region protected attributes
        protected World _world;
        #endregion

        #region public accessors
        public World world { get { return _world; } private set { _world = value; } }
        #endregion

        #region constructor
        public Meteo_abstract(World world) {
            this.world = world;
        }
        #endregion

        #region private methods

        #endregion

        #region public methods
        public virtual void Update(float dt) {

        }

        public virtual void Draw(SpriteBatch spriteBatch) {

        }
        #endregion
    }
}
