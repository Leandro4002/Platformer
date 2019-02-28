using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global {
    [Serializable]
    abstract class Bloc : Thing, IRectangleCollision {
        #region protected attributes
        protected int[] _side;
        protected float _width, _height;
        #endregion

        #region public accessor
        public int[] side { get { return _side; } protected set { _side = value; } }
        public float width { get { return _width; } protected set { _width = value; } }
        public float height { get { return _height; } protected set { _height = value; } }
        #endregion

        #region public attributes
        public static readonly Color DEBUG_COLOR = Color.DarkOrange;
        public static readonly int SIZE = 50;
        #endregion

        #region constructor
        public Bloc(World world) : base(world) {
            //Set defaults values
            width = SIZE;
            height = SIZE;
            color = Color.White;
            side = new int[] { 1, 1, 1, 1 };
            textureString = "rect,"+SIZE+","+SIZE+",3";
        }
        #endregion

        #region public methods
        public Point getCoord => new Point((int)x/SIZE, (int)y/SIZE);
        
        public void CalculatePosition(Point gridPosition) {
            pos = new Vector2(gridPosition.X * SIZE, gridPosition.Y * SIZE);
        }
        #endregion
    }
}
