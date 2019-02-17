using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Global {
    abstract class Rectangle_Entity : Entity, IRectangleCollision {
        #region protected attributes
        protected int[] _side;
        protected float _width, _height;
        #endregion

        #region public accessors
        public int[] side { get { return _side; } set { _side = value; } }
        public float width { get { return _width; } protected set { _width = value; } }
        public float height { get { return _height; } protected set { _height = value; } }

        public Vector2 centerPoint => new Vector2(pos.X + width / 2, pos.Y + height / 2);
        public float centerX => x + width/2;
        public float centerY => y + height/2;
        #endregion

        #region constructor
        public Rectangle_Entity(World world, Vector2 pos, float width, float height, string textureString) : base(world, pos, textureString) {
            this.width = width;
            this.height = height;

            side = new int[] { 1, 1, 1, 1 };
        }
        #endregion

        #region protected methods
        protected bool MovingRectCollison(IRectangleCollision rect, int side, float moveSpeed) {
            return MovingRectCollison(rect.x, rect.y, rect.width, rect.height, side, moveSpeed);
        }
        protected bool MovingRectCollison(float x, float y, float wth, float hgt, int side, float moveSpeed) {
            switch (side) {
                //haut
                case 0:
                    if (pos.X + width > x && pos.X < x + wth &&
                    pos.Y + height <= y && pos.Y + height + moveSpeed > y)
                        return true;

                    break;
                //bas
                case 1:
                    if (pos.X + width > x && pos.X < x + wth &&
                    pos.Y >= y + hgt && pos.Y + moveSpeed < y + hgt)
                        return true;

                    break;
                //gauche
                case 2:
                    if (pos.Y + height > y && pos.Y < y + hgt &&
                    pos.X + width <= x && pos.X + width + moveSpeed > x)
                        return true;
                    /*
                    if (pos.X + width <= x && pos.X + width + moveSpeed > x) {
                        if (pos.Y + height - player.offsetStairEffect >= y && pos.Y < y + hgt) {
                            return true;
                        } else if (pos.Y + height >= y && pos.Y < y + hgt) {
                            float delta = pos.Y + height - y;
                            if (!MoveY(-delta)) return true;
                        }
                    }
                    */

                    break;
                //droite
                case 3:
                    if (pos.Y + height > y && pos.Y < y + hgt &&
                    pos.X >= x + wth && pos.X + moveSpeed < x + wth)
                        return true;
                    /*
                    if (pos.X >= x + wth && pos.X + moveSpeed < x + wth) {
                        if (pos.Y + height - player.offsetStairEffect >= y && pos.Y < y + hgt) {
                            return true;
                        } else if (pos.Y + height >= y && pos.Y < y + hgt) {
                            float delta = pos.Y + height - y;
                            if (!MoveY(-delta)) return true;
                        }
                    }
                    */
                    break;
                default: throw new Exception("Unknown side to collide");
            }

            return false;
        }
        #endregion

        #region public methods
        public override void Update(float dt) {
            base.Update(dt);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, pos + world.pos, color);

            spriteBatch.DrawString(world.content.fonts["consolas"], id.ToString(), new Vector2(world.pos.X + pos.X + 5, world.pos.Y + pos.Y + 38), Color.DarkOrange);
        }

        public override bool MoveX(float moveSpeed) {
            float dx = float.MaxValue;
            float newDx;

            if (moveSpeed < 0) {
                //Check collisions with every other things in the world
                for (int i = 0; i < world.things.Count; i++) {
                    //Get iterated thing
                    dynamic iteratedThing = world.things[i];

                    //If the iterated thing is not rectangle, ignore it
                    if (!(iteratedThing is IRectangleCollision)) {
                        Console.WriteLine(iteratedThing.GetType());
                        continue;
                    }

                    //Prevents not checking himself
                    if (iteratedThing.id != id) {
                        if (MovingRectCollison(iteratedThing, 3, moveSpeed)) {
                            if (iteratedThing.side[3] == 1) {
                                //TODO on touch event

                                newDx = pos.X - (iteratedThing.pos.X + iteratedThing.width);

                                if (newDx < dx) {
                                    dx = newDx;
                                }
                            }
                        }
                    }
                }

                //Collision world borders
                if (MovingRectCollison(0, 0, 0, world.height, 3, moveSpeed)) {

                    //Get the distance between thing and world border
                    newDx = pos.X;

                    //If the thing is already stick to world border, return
                    if (newDx == 0) {
                        HorizontalCollision(0, moveSpeed);
                        return false;
                    }

                    //Handle collision between thing and world border
                    if (newDx < dx) {
                        dx = newDx;
                        world.bordersEvents[2](this, new Vector2(0, moveSpeed));
                    }
                }

                //If the movement is stopped, return
                if (HorizontalCollision(dx, moveSpeed)) return false;

            } else if (moveSpeed > 0) {
                //Check collisions with every other things in the world
                for (int i = 0; i < world.things.Count; i++) {
                    //Get iterated thing
                    dynamic iteratedThing = world.things[i];

                    if (!(iteratedThing is IRectangleCollision))
                        continue;

                    //Prevents not checking himself
                    if (iteratedThing.id != id) {
                        if (MovingRectCollison(iteratedThing, 2, moveSpeed)) {
                            if (iteratedThing.side[2] == 1) {
                                //TODO on touch event
                                newDx = iteratedThing.pos.X - (pos.X + width);

                                if (newDx < dx) {
                                    dx = newDx;
                                }
                            }
                        }
                    }
                }

                //Collision world borders
                if (MovingRectCollison(world.width, 0, 0, world.height, 2, moveSpeed)) {

                    //Get the distance between thing and world border
                    newDx = world.width - (pos.X + width);

                    //If the thing is already stick to world border, return
                    if (newDx == 0) {
                        HorizontalCollision(0, moveSpeed);
                        return false;
                    }

                    //Handle collision between thing and world border
                    if (newDx < dx) {
                        dx = newDx;
                        world.bordersEvents[3](this, new Vector2(0, moveSpeed));
                    }
                }

                //If the movement is stopped, return
                if (HorizontalCollision(dx, moveSpeed)) return false;
            }

            //Déplacement du thing si il n'y a eu aucune collisions
            x += moveSpeed;

            return true;
        }
        public override bool MoveY(float moveSpeed) {
            float dy = float.MaxValue;
            float newDy;

            if (moveSpeed < 0) {
                //Check collisions with every other things in the world
                for (int i = 0; i < world.things.Count; i++) {
                    //Get iterated thing
                    dynamic iteratedThing = world.things[i];

                    if (!(iteratedThing is IRectangleCollision))
                        continue;

                    //Prevents not checking himself
                    if (iteratedThing.id != id) {

                        if (MovingRectCollison(iteratedThing, 1, moveSpeed)) {
                            if (iteratedThing.side[1] == 1) {
                                //TODO on touch event

                                newDy = pos.Y - (iteratedThing.pos.Y + iteratedThing.height);

                                if (newDy == 0) {
                                    VerticalCollision(0, moveSpeed);
                                    return false;
                                }

                                if (newDy < dy) dy = newDy;
                            }
                        }
                    }
                }

                //Collision world borders
                if (MovingRectCollison(0, 0, world.width, 0, 1, moveSpeed)) {

                    //Get the distance between thing and world border
                    newDy = pos.Y;

                    //If the thing is already stick to world border, return
                    if (newDy == 0) {
                        VerticalCollision(0, moveSpeed);
                        return false;
                    }

                    //Handle collision between thing and world border
                    if (newDy < dy) {
                        dy = newDy;
                        world.bordersEvents[0](this, new Vector2(0, moveSpeed));
                    }
                }

                //If the movement is stopped, return
                if (VerticalCollision(dy, moveSpeed)) return false;

            } else if (moveSpeed > 0) {
                //Check collisions with every other things in the world
                for (int i = 0; i < world.things.Count; i++) {
                    //Get iterated thing
                    dynamic iteratedThing = world.things[i];

                    if (!(iteratedThing is IRectangleCollision)) {
                        continue;
                    }

                    //Prevents not checking himself
                    if (iteratedThing.id != id) {
                        if (MovingRectCollison(iteratedThing, 0, moveSpeed)) {

                            if (iteratedThing.side[0] == 1) {
                                //TODO on touch event

                                //Get the distance between things
                                newDy = iteratedThing.pos.Y - (pos.Y + height);

                                //If the thing is already stick to iterated thing
                                if (newDy == 0) {
                                    VerticalCollision(0, moveSpeed);
                                    return false;
                                }

                                if (newDy < dy) dy = newDy;
                            }
                        }
                    }
                }

                //Collision world borders
                if (MovingRectCollison(0, world.height, world.width, 0, 0, moveSpeed)) {

                    //Get the distance between thing and world border
                    newDy = world.height - (pos.Y + height);

                    //If the thing is already stick to world border, return
                    if (newDy == 0) {
                        VerticalCollision(0, moveSpeed);
                        return false;
                    }

                    //Handle collision between thing and world border
                    if (newDy < dy) {
                        dy = newDy;
                        world.bordersEvents[1](this, new Vector2(0, moveSpeed));
                    }
                }

                //If the movement is stopped, return
                if (VerticalCollision(dy, moveSpeed)) return false;

            }

            //Déplacement du thing
            y += moveSpeed;

            return true;
        }

        #endregion
    }
}
