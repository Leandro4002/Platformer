using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Global.Meteo
{
    class Confetti_rain : Meteo_abstract
    {
        #region drop class
        class Drop
        {
            public Color color;
            public Vector3 pos;
            public int wth, hgt;

            //All randomized drop
            public Drop(int minWth, int maxWth, int minHgt, int maxHgt, float maxDepth, Rectangle wdowDim, Random rnd, Color color)
            {
                wth = rnd.Next(minWth, maxWth + 1);
                hgt = rnd.Next(minHgt, maxHgt + 1);

                int x = rnd.Next(wdowDim.Width - wth);
                int y = rnd.Next(wdowDim.Height - hgt);
                int z = rnd.Next(100, (int)(maxDepth * 100)) / 100;//Width and height multiplicator

                pos = new Vector3(x, y, z);

                //Generate color depending on the z axis
                this.color = color;
                this.color.R = (byte)(this.color.R * z);
                this.color.G = (byte)(this.color.G * z);
                this.color.B = (byte)(this.color.B * z);
            }

            //All variable defined
            public Drop(Vector3 pos, int wth, int hgt, Color color)
            {
                this.pos = pos;
                this.wth = wth;
                this.hgt = hgt;
                this.color = color;
            }
        }
        #endregion

        #region private attributes
        List<Drop> particules;
        int minWth, maxWth, minHgt, maxHgt;
        int numberOfParticules;
        float maxDepth, fallSpeed;
        Color particuleColor;
        #endregion

        #region constructor
        public Confetti_rain(World world, int numberOFParticules, Color color, int minWth = 1, int maxWth = 2, int minHgt = 8, int maxHgt = 10, float maxDepth = 5f, float fallSpeed = 500) : base(world)
        {

            this.particuleColor = color;
            this.numberOfParticules = numberOFParticules;

            particules = new List<Drop>();

            //Optional parameter
            this.minWth = minWth;
            this.maxWth = maxWth;
            this.minHgt = minHgt;
            this.maxHgt = maxHgt;
            this.maxDepth = maxDepth;
            this.fallSpeed = fallSpeed;

            Load();
        }
        #endregion

        #region private methods
        void Load()
        {
            Random rnd = new Random();
            for (int i = 0; i < numberOfParticules; i++)
                particules.Add(new Drop(minWth, maxWth, minHgt, maxHgt, maxDepth, world.wdowDimensions, rnd, particuleColor));
        }
        #endregion

        #region public methods
        public override void Update(float dt)
        {
            foreach (Drop drop in particules)
            {
                //Apply gravity
                drop.pos.Y += fallSpeed * dt * (1 / drop.pos.Z);

                //Apply wind
                drop.pos.X += world.wind.X * dt;
                drop.pos.Y += world.wind.Y * dt;

                //Replace the drop in the top of the window
                if (drop.pos.Y > world.wdowDimensions.Height)
                    drop.pos.Y = -drop.hgt;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Drop drop in particules)
                spriteBatch.Draw(world.content.textures["rect,10,10"], new Rectangle((int)drop.pos.X, (int)drop.pos.Y, drop.wth, drop.hgt), drop.color);
        }
        #endregion
    }
}
