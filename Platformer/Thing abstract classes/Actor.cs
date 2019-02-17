using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global {
    abstract class Actor : Rectangle_Entity {
        #region protected attributes
        protected int _health;
        protected bool _isOnGround;
        #endregion

        #region public accessors
        public int health {  get { return _health; } protected set { _health = value; } }
        public bool isOnGround {  get { return _isOnGround; } protected set { _isOnGround = value; } }
        #endregion

        #region constructor
        public Actor(World world, Vector2 pos, float width, float height, string textureString) : base(world, pos, width, height, textureString) {

        }
        #endregion

        #region protected methods
        #endregion

        #region public methods
        public override void Update(float dt) {
            base.Update(dt);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
        }
        public override bool MoveX(float moveSpeed) {
            return base.MoveX(moveSpeed);
        }
        public override bool MoveY(float moveSpeed) {
            return base.MoveY(moveSpeed);
        }
        #endregion
    }
}
