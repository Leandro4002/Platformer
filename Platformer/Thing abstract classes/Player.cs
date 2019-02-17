using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global {
    class Player : Actor {

        #region private attributes
        float _moveSpeed = 150, _jumpForce = -700;
        Vector2 _directionSpeed;
        #endregion

        #region public accessors
        public float moveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
        public float jumpForce { get { return _jumpForce; } set { _jumpForce = value; } }
        public Vector2 directionSpeed { get { return _directionSpeed; } }
        #endregion

        #region constructor
        public Player(World world, Vector2 position, float width, float height, string textureString) : base(world, position, width, height, textureString) {
            
        }
        #endregion

        #region base methods override
        protected override bool VerticalCollision(float dy, float moveSpeed) {
            if (dy != float.MaxValue) {
                _force.Y = 0;

                if (moveSpeed > 0) {
                    //Place the player on the right spot
                    y += dy;

                    //The player just land on the ground
                    if (dy != 0) {
                        TouchGround();
                    }
                } else if (moveSpeed < 0) {
                    //Place the player on the right spot
                    y -= dy;
                }

                return true;
            }

            if (moveSpeed > 0) {
                //The player is falling and not touching anything
                _isOnGround = false;
            }

            return false;
        }
        #endregion

        #region update
        public override void Update(float dt) {
            base.Update(dt);

            //Saut
            if (world.oldKeyboardState.IsKeyUp(Keys.W) && world.keyboardState.IsKeyDown(Keys.W)
                /*&& _isOnGround*/) {
                ApplyForce(new Vector2(0, _jumpForce));
                _isOnGround = false;
            }

            //Gauche
            if (world.keyboardState.IsKeyDown(Keys.A))
                MoveX(-moveSpeed * dt);

            //Droite
            if (world.keyboardState.IsKeyDown(Keys.D))
                MoveX(moveSpeed * dt);
        }
        #endregion

        #region draw
        public override void Draw(SpriteBatch spriteBatch) {
            if (_isOnGround) {
                spriteBatch.Draw(texture, pos + world.pos, Color.Green);
            } else {
                spriteBatch.Draw(texture, pos + world.pos, Color.Red);
            }
        }
        #endregion

        #region public methods
        public void TouchGround() {
            _isOnGround = true;
            Console.WriteLine("Le joueur touche le sol");
        }
        public override bool MoveX(float moveSpeed) {
            bool result = base.MoveX(moveSpeed);
            world.SetCameraPosition(centerX, centerY);
            return result;
        }
        public override bool MoveY(float moveSpeed) {
            bool result = base.MoveY(moveSpeed);
            world.SetCameraPosition(centerX, centerY);
            return result;
        }
        #endregion
    }
}
