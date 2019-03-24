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
    class Background {
        class Layer {
            public struct Options {
                public bool isOnFront, overflowX, overflowY;
            }
            public Vector2 pos, distance, offset;
            public Texture2D image;
            public Options options;

            public Layer(Texture2D image, Vector2 distance, Vector2? offset = null, Options? options = null) {
                this.image = image;
                this.distance = distance;
                this.offset = offset ?? Vector2.Zero;
                this.options = options ?? new Options();
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
            layers.Add(new Layer(world.content.textures["collines1"], new Vector2(0, 0), new Vector2(0, world.y + world.height - world.content.textures["collines1"].Height)));
            layers.Add(new Layer(world.content.textures["collines2"], new Vector2(-0.1f, -0.05f), new Vector2(0, world.y + world.height - world.content.textures["collines2"].Height - world.y * 0.05f)));
            layers.Add(new Layer(world.content.textures["collines3"], new Vector2(-0.2f, -0.1f), new Vector2(0, world.y + world.height - world.content.textures["collines3"].Height - world.y * 0.1f)));
            layers.Add(new Layer(world.content.textures["collines4"], new Vector2(-0.3f, -0.2f), new Vector2(0, world.y + world.height - world.content.textures["collines4"].Height - world.y * 0.2f)));
            layers.Add(new Layer(world.content.textures["collines5"], new Vector2(-0.4f, -0.3f), new Vector2(0, world.y + world.height - world.content.textures["collines5"].Height - world.y * 0.3f)));
            layers.Add(new Layer(world.content.textures["collines6"], new Vector2(-1.6f, -0.8f), new Vector2(0, world.y + world.height - world.content.textures["collines6"].Height - world.y * 0.8f), new Layer.Options { isOnFront = true, overflowX = true }));

            //Replace correctenent les images de fond
            ReplaceLayers();
        }

        public void ReplaceLayers() {
            foreach (Layer l in layers) {
                l.pos.X = l.offset.X + (-world.x * l.distance.X);
                l.pos.Y = l.offset.Y + (-world.y * l.distance.Y);
            }
        }

        public void Draw(SpriteBatch spritebatch, bool onFront) {
            if (isVisible) {
                foreach (Layer l in layers) {
                    if (l.options.isOnFront == onFront) {
                        if (l.options.overflowX && l.options.overflowY) {
                            for (float i = l.pos.X + ((l.pos.X > 0) ? -l.image.Width : 0) + l.image.Width * ((int)-l.pos.X / l.image.Width); i < world.wdowDimensions.Width; i += l.image.Width) {
                                for (float j = l.pos.Y + ((l.pos.Y > 0) ? -l.image.Height : 0) + l.image.Height * ((int)-l.pos.Y / l.image.Height); j < world.wdowDimensions.Height; j += l.image.Height) {
                                    spritebatch.Draw(l.image, new Vector2(i, j), color);
                                }
                            }
                        } else if (l.options.overflowX) {
                            for (float i = l.pos.X + ((l.pos.X > 0) ? -l.image.Width : 0) + l.image.Width * ((int)-l.pos.X / l.image.Width); i < world.wdowDimensions.Width; i += l.image.Width) {
                                spritebatch.Draw(l.image, new Vector2(i, l.pos.Y), color);
                            }
                        } else if (l.options.overflowY) {
                            for (float i = l.pos.Y + ((l.pos.Y > 0) ? -l.image.Height : 0) + l.image.Height * ((int)-l.pos.Y / l.image.Height); i < world.wdowDimensions.Height; i += l.image.Height) {
                                spritebatch.Draw(l.image, new Vector2(l.pos.X, i), color);
                            }
                        } else {
                            spritebatch.Draw(l.image, l.pos, color);
                        }
                    }
                }
            }
        }
    }
}
