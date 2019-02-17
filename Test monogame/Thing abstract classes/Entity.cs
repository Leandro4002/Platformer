using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Global {
    abstract class Entity : Thing {
        #region protected attributes
        protected Vector2 _force, _permanentForce, _limitForce, _dragForce; //pixel/second
        protected float _mass;
        #endregion

        #region public accessors
        public Vector2 force { get { return _force; } }
        public Vector2 permanentForce { get { return _permanentForce; } set { _permanentForce = value; } }
        public Vector2 limitForce { get { return _limitForce; } set { _limitForce = value; } }
        public Vector2 dragForce { get { return _dragForce; } }
        public float mass { get { return _mass; } private set { _mass = value; } }
        #endregion

        #region constructor
        public Entity(World world, Vector2 pos, string textureString) : base(world) {
            this.pos = pos;
            this.textureString = textureString;
        }
        #endregion

        #region update
        public override void Update(float dt) {
            //Add permanent speed to the thing speed (gravity or wind for exemple)
            ApplyForce(permanentForce * dt);

            //Slow down the cineticForce
            ApplyDrag(dragForce * dt);

            //Limit maximal force applied to the object
            _force.X = MathHelper.Clamp(force.X, -limitForce.X, limitForce.X);
            _force.Y = MathHelper.Clamp(force.Y, -limitForce.Y, limitForce.Y);

            //Apply the speed of the object
            MoveY(force.Y * dt);
            MoveX(force.X * dt);
        }
        #endregion

        #region protected methods
        //Approach force to 0
        protected void ApplyDrag(Vector2 dragForce) {
            if (dragForce.X >= Math.Abs(force.X)) {
                _force.X = 0;
            } else {
                if (force.X < 0) {
                    _force.X += dragForce.X;
                } else if (force.X > 0) {
                    _force.X -= dragForce.X;
                }
            }

            if (dragForce.Y >= Math.Abs(force.Y)) {
                _force.Y = 0;
            } else {
                if (force.Y < 0) {
                    _force.Y += dragForce.Y;
                } else if (force.Y > 0) {
                    _force.Y -= dragForce.Y;
                }
            }
        }

        protected virtual bool HorizontalCollision(float dx, float moveSpeed) {
            if (dx != float.MaxValue) {
                _force.X = 0;

                if (moveSpeed > 0) {
                    x += dx;
                } else if (moveSpeed < 0) {
                    x -= dx;
                }

                return true;
            }

            return false;
        }
        protected virtual bool VerticalCollision(float dy, float moveSpeed) {
            if (dy != float.MaxValue) {
                _force.Y = 0;

                if (moveSpeed > 0) {
                    y += dy;
                } else if (moveSpeed < 0) {
                    y -= dy;
                }

                return true;
            }

            return false;
        }
        #endregion

        #region public methods
        public void ApplyForce(Vector2 appliedForce) {
            _force += appliedForce;
        }

        public abstract bool MoveX(float moveSpeed);
        public abstract bool MoveY(float moveSpeed);
        #endregion
    }
}
