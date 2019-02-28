using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global {
    class Editor {
        [Serializable]
        public struct SaveData {
            public float x, y, width, height;
            public string name;
            public Bloc[,] blocs;
            public List<Thing> things;
            public Action<Thing, Vector2>[] bordersEvents;
            public Color color;
            public List<Meteo.Meteo_abstract> meteo;
            public Background background;
            public Random rnd;
            public Vector2 wind, windScalar, windLimit, windForce;
            public int windRefreshRate; //Delay beetween wind refresh in miliseconds
            public float windTimer;
        }

        List<Control.Control_Abstract> controls;
        World world;
        ContentOrganizer content;
        IFormatter formatter;

        public Editor(World world) {
            this.world = world;

            formatter = new BinaryFormatter();
        }

        public void LoadContent(ContentOrganizer content) {
            this.content = content;

            //Add buttons
            controls = new List<Control.Control_Abstract>();
            Texture2D rectTexture = content.textures["rect,200,80"];
            SpriteFont font = content.fonts["consolas"];
            //controls.Add(new Control.Button(rectTexture, font, 100, 100, 200, 80, "Sauvegarder", delegate () { Save(Environment.CurrentDirectory + @"\TEST.bin"); }));
            //controls.Add(new Control.Button(rectTexture, font, 100, 200, 200, 80, "Charger", delegate () { Load(Environment.CurrentDirectory + @"\TEST.bin"); }));
            //controls.Add(new Control.TextBox(rectTexture, font, 100, 300, 206, 30, "textbox with a very long text that cannot be displayed"));
            //controls.Add(new Control.TextBox(rectTexture, font, 100, 400, 206, 30, "dsfsdfqs"));
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState, float dt) {
            Mouse.SetCursor(MouseCursor.Arrow);
            foreach (Control.Control_Abstract button in controls) {
                button.Update(keyboardState, mouseState, dt);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Control.Control_Abstract button in controls) {
                button.Draw(spriteBatch);
            }
        }

        public void Save(string path) {
            //TODO Check si le chemin est valide
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, world.ToSaveData());
            stream.Close();
        }

        public World Load(string path) {
            //TODO Check si le chemin est valide
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return (World)formatter.Deserialize(stream);
        }
    }
}
