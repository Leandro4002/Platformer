using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global {

    class Layer {
        public float x, y, distanceX, distanceY;
        public Texture2D image;
        public bool isVisible = true;
        public bool isOnFront = false;

    }
    class Background {
        World world;
        List<Layer> layers;
        Color color = Color.White;
        bool isVisible = true;

        Background(World world) {
            layers = new List<Layer>();
        }

        void load(int id) {
            //Charger les bonnes images de fond
            Layer layer = new Layer();
            layer.image = world.content.textures["collines1"];
            layers.Add(layer);

            //Replace correctenent les images de fond
            foreach (Layer l in layers) {
                l.x = l.x - (-world.x * l.distanceX / 100);
                l.y = l.y - (-world.y * l.distanceY / 100);
            }
        }

        void moveX(float moveSpeed) {
            foreach (Layer l in layers) {
                l.x = l.x + (moveSpeed * l.distanceX / 100);
            }
        }

        void moveY(float moveSpeed) {
            foreach (Layer l in layers) {
                l.y = l.y + (moveSpeed * l.distanceY / 100);
            }
        }

        public void Draw(SpriteBatch spritebatch, bool onFront) {
            if (isVisible) {
                foreach (Layer l in layers) {
                    if (l.isVisible/* && l.isOnFront == onFront*/) {
                        spritebatch.Draw(l.image, new Vector2(l.x, l.y), color);
                    }
                }
            }
        }

    }
}
