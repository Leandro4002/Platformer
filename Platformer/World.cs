using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Global {
    class World {
        #region private attributes
        float _x, _y, _width, _height;
        Bloc[,] _blocs;
        Player _player;
        List<Thing> _things;
        Action<Thing, Vector2>[] _bordersEvents;
        Rectangle _windowDimensions;
        Color _color;
        ContentOrganizer _content;
        string _name;
        KeyboardState _keyboardState, _oldKeyboardState;
        Vector2 _wind;
        List<Meteo.Meteo_abstract> _meteo;
        Background background;
        #endregion

        #region public attributes
        public bool displayDebug;
        public readonly Color DEBUG_COLOR;
        public Texture2D debugGrid;
        public Texture2D debugRectangleBackground;
        public Vector2 windScalar, windLimit, windForce;
        public int windRefreshRate; //Delay beetween wind refresh in miliseconds
        public float windTimer;
        public Random rnd;
        #endregion

        #region public accessors
        public float x { get { return _x; } set { _x = value; } }
        public float y { get { return _y; } set { _y = value; } }
        public float width { get { return _width; } private set { _width = value; } }
        public float height { get { return _height; } private set { _height = value; } }
        public Bloc[,] blocs { get { return _blocs; } private set { _blocs = value; } }
        public Player player { get { return _player; } private set { _player = value; } }
        public List<Thing> things { get { return _things; } private set { _things = value; } }
        public Action<Thing, Vector2>[] bordersEvents { get { return _bordersEvents; } private set { _bordersEvents = value; } }
        public Rectangle wdowDimensions { get { return _windowDimensions; } private set { _windowDimensions = value; } }
        public Color color { get { return _color; } private set { _color = value; } }
        public ContentOrganizer content { get { return _content; } private set { _content = value; } }
        public string name { get { return _name; } private set { _name = value; } }
        public KeyboardState keyboardState { get { return _keyboardState; } private set { _keyboardState = value; } }
        public KeyboardState oldKeyboardState { get { return _oldKeyboardState; } private set { _oldKeyboardState = value; } }
        public Vector2 wind { get { return _wind; } private set { _wind = value; } }
        public List<Meteo.Meteo_abstract> meteo{ get { return _meteo; } private set { _meteo = value; } }

        public Vector2 pos => new Vector2(x, y);
        #endregion

        #region constructor
        public World(Rectangle wdowDimensions, int cols, int rows, Random rnd, string name = "Default") {
            //Set constructor values
            this.wdowDimensions = wdowDimensions;
            width = cols * Bloc.SIZE;
            height = rows * Bloc.SIZE;
            this.name = name;
            this.rnd = rnd;
            
            //Set default values
            x = 0;
            y = 0;
            things = new List<Thing>();
            meteo = new List<Meteo.Meteo_abstract>();
            color = Color.White;
            blocs = new Bloc[cols, rows];
            DEBUG_COLOR = Color.DarkGreen;
            windLimit = new Vector2(100, 10);
            windScalar = new Vector2(5, 1);
            bordersEvents = new Action<Thing, Vector2>[4];

            //Define default world border events
            void Upper(Thing thing, Vector2 moveForce) {
                //Do something when a thing cross the upper border
            }
            bordersEvents[0] = Upper;

            void Bottom(Thing thing, Vector2 moveForce) {
                //Do something when a thing cross the bottom border
            }
            bordersEvents[1] = Bottom;

            void Left(Thing thing, Vector2 moveForce) {
                //Do something when a thing cross the left border
            }
            bordersEvents[2] = Left;

            void Right(Thing thing, Vector2 moveForce) {
                //Do something when a thing cross the right border
            }
            bordersEvents[3] = Right;

            Load();
        }
        #endregion

        #region loadContent
        public void LoadContent(ContentOrganizer content) {
            this.content = content;

            //Load image(s) background
            background.LoadContent();

            //Load content of things
            foreach (var thing in things)
                thing.LoadContent();
        }
        #endregion

        #region load
        public void Load() {
            displayDebug = true;

            background = new Background(this);

            //meteo.Add(new Meteo.Rain(this, 500, Color.Red, Color.Blue));

            CreateThings();

            //Set the camera to be in the bottom left corner
            SetCameraPosition(0, height);
        }
        #endregion

        #region update
        public void Update(float dt, KeyboardState keyboardState) {
            this.keyboardState = keyboardState;

            //Update all things in world
            foreach (var thing in things)
                thing.Update(dt);

            //Update wind if necessary
            windTimer += 1000*dt;
            if (windTimer > windRefreshRate && windRefreshRate != 0)
            {
                windTimer = 0;
                UpdateWind();
            }
            
            //Update all meteo
            foreach (var meteo in meteo)
                meteo.Update(dt);

            oldKeyboardState = keyboardState;
        }
        #endregion

        #region draw
        public void Draw(SpriteBatch spriteBatch) {
            //Draw room image behind
            background.Draw(spriteBatch, false);

            //Draw all things in worlds
            foreach (var thing in things)
                thing.Draw(spriteBatch);

            //Draw room image front
            background.Draw(spriteBatch, true);

            //Draw all meteo
            foreach (var meteo in meteo)
                meteo.Draw(spriteBatch);

            if (displayDebug) {
                //Draw world border
                spriteBatch.Draw(content.textures["worldBounds"], pos, DEBUG_COLOR);

                //Calculate grid position
                float dumpX = ((x / Bloc.SIZE) - (float)Math.Floor(x / Bloc.SIZE)) * Bloc.SIZE - Bloc.SIZE;
                float dumpY = ((y / Bloc.SIZE) - (float)Math.Floor(y / Bloc.SIZE)) * Bloc.SIZE - Bloc.SIZE;

                //Doesn't get out of the world
                dumpX = MathHelper.Clamp(dumpX, x, x + width - debugGrid.Width);
                dumpY = MathHelper.Clamp(dumpY, y, y + height - debugGrid.Height);

                //Draw debug grid
                spriteBatch.Draw(debugGrid, new Vector2(dumpX, dumpY), DEBUG_COLOR);

                //Draw information about this world
                spriteBatch.Draw(debugRectangleBackground, Vector2.Zero, Color.White * 0.7f);
                string[] debugText = {
                    "Name : " + name,
                    "Pos : " + pos,
                    "Width : " + width + "; Height : " + height,
                    "Number of things : " + things.Count(),
                    string.Format("wind X:{0:000} Y:{1:000}", wind.X, wind.Y)
                };
                int textSpacing = 20;
                for (int i = 0; i < debugText.Length; i++) {
                    spriteBatch.DrawString(content.fonts["arial"], debugText[i], new Vector2(2, textSpacing * i), DEBUG_COLOR);
                }
            }
        }
        #endregion

        #region private methods
        void UpdateWind() {
            //Generate 2 noise value
            Vector2 windNoise = new Vector2();
            windNoise.X = (float)Perlin.perlin(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble());
            windNoise.Y = (float)Perlin.perlin(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble());

            //Define wheter the wind increment or decrement
            switch(rnd.Next(0, 4)) {
                case 1:
                    windNoise.X = -windNoise.X;
                    break;
                case 2:
                    windNoise.Y = -windNoise.Y;
                    break;
                case 3:
                    windNoise.X = -windNoise.X;
                    windNoise.Y = -windNoise.Y;
                    break;
            }

            //Update wind
            _wind += windNoise * windScalar;

            //Limit the max speed of the wind
            _wind.X = MathHelper.Clamp(_wind.X, -windLimit.X, windLimit.X);
            _wind.Y = MathHelper.Clamp(_wind.Y, -windLimit.Y, windLimit.Y);
        }

        void CreateThings() {
            /*
            Point[] blocPositions = {
                new Point(2, 21),
                new Point(4, 21),
                new Point(3, 22),
                new Point(3, 20),
            };

            foreach(Point position in blocPositions) {
                AddBloc(new Blocs.Dirt(this), position);
            }
            */
            
            Player player = new Player(this, new Vector2(x, height - 56), 44, 56, "player");

            player.permanentForce = new Vector2(0, 1000);
            player.limitForce = new Vector2(1000, 1000);

            SetPlayer(player);
        }
        #endregion

        #region public methods
        public int GenerateUniqueId() {
            int val = 0;
            bool isUniqueIdFound = false;

            while (!isUniqueIdFound) {
                isUniqueIdFound = true;
                foreach (Thing thing in things) {

                    if (val == thing.id) {
                        val++;
                        isUniqueIdFound = false;
                    }
                }
            }

            return val;
        }

        public void SetPlayer(Player player) {
            this.player = player;
            AddThing(player);
            player.TouchGround();
        }

        public void AddThing(Thing thing) {
            things.Add(thing);
            Console.WriteLine("Nouveau " + thing.GetType());
        }

        public void AddBloc(Bloc bloc, Point gridPosition) {
            //Test if the asked position is in the world's range
            if (gridPosition.X >= blocs.GetLength(0) || gridPosition.Y >= blocs.GetLength(1))
                throw new Exception("The bloc to add does not fall in world's array of bloc range. World array of blocs [" + blocs.GetLength(0) + "][" + blocs.GetLength(1) + "]");

            //Calculate the bloc position (from coordinate to pixels)
            bloc.CalculatePosition(gridPosition);

            //Add the bloc in the grid position
            blocs[gridPosition.X, gridPosition.Y] = bloc;

            //Add the bloc in the world's things
            AddThing(bloc);
        }

        public void AddThings(List<Thing> things) {
            foreach (Thing thing in things) {
                AddThing(thing);
            }
        }

        public void SetCameraPosition(float newCameraX, float newCameraY) {
            if (newCameraX < wdowDimensions.Width / 2) {
                x = 0;
            } else if (newCameraX > width - wdowDimensions.Width / 2) {
                x = -width + wdowDimensions.Width;
            } else {
                x = -newCameraX + wdowDimensions.Width / 2;
            }

            if (newCameraY < wdowDimensions.Height / 2) {
                y = 0;
            } else if (newCameraY > height - wdowDimensions.Height / 2) {
                y = -height + wdowDimensions.Height;
            } else {
                y = -newCameraY + wdowDimensions.Height / 2;
            }

            background.ReplaceLayers();
        }
        #endregion
    }
}