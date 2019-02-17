using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global {
    class Background {
        class Layer {
            public float x, y, distanceX, distanceY;
            public Texture2D image;
            public bool isOnFront = false;

            public Layer(Texture2D image, float distanceX, float distanceY, bool isOnFront = false) {
                this.image = image;
                this.distanceX = distanceX;
                this.distanceY = distanceY;
                this.isOnFront = isOnFront;
            }
        }

        World world;
        List<Layer> layers;
        Color color = Color.White;
        bool isVisible = true;

        public Background(World world) {
            this.world = world;
            layers = new List<Layer>();
        }

        public void LoadContent() {
            //Charger les bonnes images de fond
            layers.Add(new Layer(world.content.textures["collines1"], 0, 0));
            layers.Add(new Layer(world.content.textures["collines2"], 0.15f, 0.1f));
            layers.Add(new Layer(world.content.textures["collines3"], 0.25f, 0.15f));
            layers.Add(new Layer(world.content.textures["collines4"], 0.3f, 0.2f));
            layers.Add(new Layer(world.content.textures["collines5"], 0.4f, 0.25f));
            layers.Add(new Layer(world.content.textures["collines6"], 1.2f, 0.35f, true));

            ReplaceLayers();
        }

        public void ReplaceLayers() {
            //Replace correctenent les images de fond
            foreach (Layer l in layers) {
                l.x = world.x * l.distanceX;
                l.y = world.y * l.distanceY;
            }
        }

        public void Draw(SpriteBatch spritebatch, bool onFront) {
            if (isVisible) {
                foreach (Layer l in layers) {
                    if (l.isOnFront == onFront) {
                        spritebatch.Draw(l.image, new Vector2(l.x, l.y), color);
                    }
                }
            }
        }

    }
}
