using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global.Meteo {
    class Rain : Meteo_abstract {
        #region drop class
        class Drop {
            public Color color;
            public float x, y, depth;
            public int wth, hgt;
            public Rain rain;

            public Drop(Rain rain) {
                this.rain = rain;
                Reset(true);
            }

            public void Reset(bool resetPosition = false) {
                //Size and depth
                wth = rain.world.rnd.Next(rain.minWth, rain.maxWth + 1);
                hgt = rain.world.rnd.Next(rain.minHgt, rain.maxHgt + 1);
                depth = (float)rain.world.rnd.NextDouble() * rain.maxDepth + rain.minDepth;

                //Position
                if (resetPosition) {
                    x = (float)(rain.world.rnd.NextDouble() * (rain.world.wdowDimensions.Width - wth) - rain.world.x);
                    y = (float)(rain.world.rnd.NextDouble() * (rain.world.wdowDimensions.Height - hgt) - rain.world.y);
                }

                //Color depending on depth
                float scalar = depth * (rain.maxDepth - rain.minDepth);
                float r, g, b;
                rain.minDepthColor.Deconstruct(out r, out g, out b);
                rain.maxDepthColor.Deconstruct(out float maxR, out float maxG, out float maxB);
                r += (maxR - r)*scalar;
                g += (maxG - g)*scalar;
                b += (maxB - b)*scalar;
                color = new Color(r/255, g/255, b/255);
            }
        }
        #endregion

        #region private attributes
        List<Drop> particules;
        int minWth, maxWth, minHgt, maxHgt;
        int numberOfParticules;
        float minDepth, maxDepth, fallSpeed;
        Color minDepthColor, maxDepthColor;
        #endregion

        #region constructor
        public Rain(World world, int numberOFParticules, Color minDepthColor, Color maxDepthColor, int minWth = 1, int maxWth = 2, int minHgt = 8, int maxHgt = 10, float minDepth = 0.7f, float maxDepth = 1.2f, float fallSpeed = 200) : base(world) {
            this.minDepthColor = minDepthColor;
            this.maxDepthColor = maxDepthColor;
            this.numberOfParticules = numberOFParticules;

            particules = new List<Drop>();

            //Optional parameter
            this.minWth = minWth;
            this.maxWth = maxWth;
            this.minHgt = minHgt;
            this.maxHgt = maxHgt;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
            this.fallSpeed = fallSpeed;

            Load();
        }
        #endregion

        #region private methods
        void Load() {
            //Set wind setting
            world.windLimit = new Vector2(100, 10);
            world.windScalar = new Vector2(10, 1);
            world.windRefreshRate = 500;

            //Add every particules
            for (int i = 0; i < numberOfParticules; i++)
                particules.Add(new Drop(this));
        }
        #endregion

        #region public methods
        public override void Update(float dt) {
            foreach (Drop drop in particules) {
                //Apply gravity
                drop.y += (fallSpeed * dt * drop.depth);

                //Apply wind
                drop.x += (world.wind.X * drop.depth * dt);
                drop.y += (world.wind.Y * drop.depth * dt);

                //Transfer the drop from one side to the other (vertical)
                if (drop.y + world.y < -drop.hgt) {
                    drop.y += world.wdowDimensions.Height + drop.hgt;
                } else if (drop.y + world.y > world.wdowDimensions.Height) {
                    drop.y -= world.wdowDimensions.Height + drop.hgt;
                    drop.Reset();
                }

                //Transfer the drop from one side to the other (horizontal)
                if (drop.x + world.x < -drop.wth) {
                    drop.x += world.wdowDimensions.Width + drop.wth;
                    drop.Reset();
                } else if (drop.x + world.x > world.wdowDimensions.Width) {
                    drop.x -= world.wdowDimensions.Width + drop.wth;
                    drop.Reset();
                }
            }

        }

        public override void Draw(SpriteBatch spriteBatch) {
            foreach (Drop drop in particules) {
                spriteBatch.Draw(world.content.textures["rect,10,10"], new Rectangle((int)(world.x + drop.x), (int)(world.y + drop.y), drop.wth, drop.hgt), drop.color);
            }
        }
        #endregion
    }
}
