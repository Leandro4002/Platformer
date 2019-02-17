using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Global {
    abstract class Thing {
        #region protected attributes
        protected World _world;
        protected Vector2 _pos;
        protected Texture2D _texture;
        protected Color _color;
        protected bool _isDrawn = true;
        protected int _id;
        protected string _textureString;
        #endregion

        #region public accessors
        public World world { get { return _world; } set { _world = value; } }
        public Vector2 pos { get { return _pos; } protected set { _pos = value; } }
        public Texture2D texture { get { return _texture; } protected set { _texture = value; } }
        public Color color { get { return _color; } set { _color = value; } }
        public bool isDrawn { get { return _isDrawn; } set { _isDrawn = value; } }
        public int id { get { return _id; } protected set { _id = value; } }
        public string textureString { get { return _textureString; } protected set { _textureString = value; } }

        public float x { get { return _pos.X; } protected set { _pos.X = value; } }
        public float y { get { return _pos.Y; } protected set { _pos.Y = value; } }
        #endregion

        #region constructors
        public Thing(World world) {
            this.world = world;
            id = this.world.GenerateUniqueId();
        }
        #endregion

        #region public methods
        public virtual void LoadContent() {
            texture = world.content.textures[textureString];
        }
        public virtual void Update(float dt) {

        }
        public virtual void Draw(SpriteBatch spriteBatch) {
            
        }
        #endregion
    }
}