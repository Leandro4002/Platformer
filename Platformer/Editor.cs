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
        List<Control.Button> controls;
        World world;
        ContentOrganizer content;
        IFormatter formatter;

        public Editor(World world, ContentOrganizer content) {
            this.world = world;
            this.content = content;

            formatter = new BinaryFormatter();

            //Add buttons
            controls = new List<Control.Button>();
            controls.Add(new Control.Button(content, 100, 100, 200, 80, "Bouton de TEST", delegate () { Save(Environment.CurrentDirectory + @"\TEST.bin"); }));
        }

        public void Update(MouseState mouseState) {
            foreach(Control.Button button in controls) {
                button.Update(mouseState);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Control.Button button in controls) {
                button.Draw(spriteBatch);
            }
        }

        public void Save(string path) {
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, world);
            stream.Close();
        }

        public World Load(string path) {
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return (World)formatter.Deserialize(stream);
        }
    }
}
