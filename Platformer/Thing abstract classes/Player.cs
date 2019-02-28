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
    class Player : Actor {
        #region public attributes
        float moveSpeed, jumpForce, jumpSlow;
        float jumpKeyDownTime, airTime;
        bool jumpKeyDown = false;
        int jumpLevel;
        float[] jumpLevels;
        List<Keys> inputs;
        #endregion

        #region constructor
        public Player(World world, Vector2 position, float width, float height, string textureString) : base(world, position, width, height, textureString) {
            airTime = 0;
            moveSpeed = 250;
            jumpForce = 700;
            jumpKeyDownTime = 0;
            jumpLevel = 1;
            jumpLevels = new float[] { 100, 200, 250, 300 };
            jumpSlow = 500;
            inputs = new List<Keys>();
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

            PressedState(Keys.S);
            PressedState(Keys.A);
            PressedState(Keys.D);

            //Direction du joueur
            if (inputs.Count() > 0) {
                switch (inputs.Last()) {
                    case Keys.S:
                        //Nada
                        break;
                    case Keys.A:
                        MoveX(-moveSpeed * dt);
                        break;
                    case Keys.D:
                        MoveX(moveSpeed * dt);
                        break;
                }
            }

            //Saut
            if (world.oldKeyboardState.IsKeyUp(Keys.W) && world.keyboardState.IsKeyDown(Keys.W)
                && _isOnGround) {
                ApplyForce(new Vector2(0, -jumpForce));
                _isOnGround = false;
            }
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

        #region private methods
        bool PressedState(Keys key) {
            if (world.oldKeyboardState.IsKeyUp(key) && world.keyboardState.IsKeyDown(key)
                && !inputs.Contains(key)) {
                inputs.Add(key);
                return true;
            } else if (world.oldKeyboardState.IsKeyDown(key) && world.keyboardState.IsKeyUp(key)
                && inputs.Contains(key)) {
                inputs.RemoveAt(inputs.FindIndex(item => item == key));
            }
            return false;
        }
        #endregion

        #region public methods
        public void TouchGround() {
            _isOnGround = true;
            //Console.WriteLine("Le joueur touche le sol");
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
