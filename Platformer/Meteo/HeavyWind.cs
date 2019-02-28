using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global.Meteo {
    [Serializable]
    class HeavyWind : Meteo_abstract {
        #region drop class
        class Drop {
            public Color color;
            public float x, y, depth;
            public int wth, hgt;
            public HeavyWind wind;

            public Drop(HeavyWind wind) {
                this.wind = wind;
                Reset(true);
            }

            public void Reset(bool resetPosition = false) {
                //Size and depth
                wth = wind.world.rnd.Next(wind.minWth, wind.maxWth + 1);
                hgt = wind.world.rnd.Next(wind.minHgt, wind.maxHgt + 1);
                depth = (float)wind.world.rnd.NextDouble() * wind.maxDepth + wind.minDepth;

                //Position
                if (resetPosition) {
                    x = (float)(wind.world.rnd.NextDouble() * (wind.world.wdowDimensions.Width - wth) - wind.world.x);
                    y = (float)(wind.world.rnd.NextDouble() * (wind.world.wdowDimensions.Height - hgt) - wind.world.y);
                }

                //Color depending on depth
                float scalar = depth * (wind.maxDepth - wind.minDepth);
                float r, g, b;
                wind.minDepthColor.Deconstruct(out r, out g, out b);
                wind.maxDepthColor.Deconstruct(out float maxR, out float maxG, out float maxB);
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
        float minDepth, maxDepth;
        Color minDepthColor, maxDepthColor;
        Vector2 windDefaultSpeed, moveEntityFactor;
        #endregion

        #region constructor
        public HeavyWind(World world, int numberOFParticules, Color minDepthColor, Color maxDepthColor, Vector2 windDefaultSpeed, Vector2 moveEntityFactor,
            int minWth = 7, int maxWth = 10, int minHgt = 2, int maxHgt = 4, float minDepth = 0.8f, float maxDepth = 1.1f) : base(world) {
            this.minDepthColor = minDepthColor;
            this.maxDepthColor = maxDepthColor;
            this.numberOfParticules = numberOFParticules;
            this.windDefaultSpeed = windDefaultSpeed;
            this.moveEntityFactor = moveEntityFactor;

            particules = new List<Drop>();

            //Optional parameter
            this.minWth = minWth;
            this.maxWth = maxWth;
            this.minHgt = minHgt;
            this.maxHgt = maxHgt;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;

            Load();
        }
        #endregion

        #region private methods
        void Load() {
            //Set wind setting
            world.windLimit = new Vector2(50, 20);
            world.windScalar = new Vector2(3, 20);
            world.windRefreshRate = 200;

            //Add every particules
            for (int i = 0; i < numberOfParticules; i++)
                particules.Add(new Drop(this));
        }
        #endregion

        #region public methods
        public override void Update(float dt) {
            Vector2 dropWindSpeed = (world.wind + windDefaultSpeed) * dt;
            foreach (Drop drop in particules) {

                //Apply wind
                drop.x += dropWindSpeed.X * drop.depth;
                drop.y += dropWindSpeed.Y * drop.depth;

                //Transfer the drop from one side to the other (vertical)
                if (drop.y + world.y < -drop.hgt) {
                    drop.y += world.wdowDimensions.Height + drop.hgt;
                    drop.Reset();
                } else if (drop.y + world.y > world.wdowDimensions.Height) {
                    drop.y -= world.wdowDimensions.Height + drop.hgt;
                    drop.Reset();
                }

                //Transfer the drop from one side to the other (horizontal)
                if (drop.x + world.x < -drop.wth) {
                    drop.x += world.wdowDimensions.Width + drop.wth;
                    if (world.wind.X < 0) drop.Reset();
                } else if (drop.x + world.x > world.wdowDimensions.Width) {
                    drop.x -= world.wdowDimensions.Width + drop.wth;
                    if (world.wind.X > 0) drop.Reset();
                }
            }

            Vector2 entityWindSpeed = (world.wind + windDefaultSpeed) * moveEntityFactor * dt;
            for (int i = 0; i < world.things.Count; i++) {
                dynamic iteratedThing = world.things[i];

                if (iteratedThing is Entity) {
                    iteratedThing.Move(entityWindSpeed);
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
