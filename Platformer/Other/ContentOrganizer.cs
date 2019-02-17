using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Global
{
    public class TexturesOrganizer
    {
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        GraphicsDevice graphicsDevice;

        public TexturesOrganizer(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public Texture2D this[string index] {
            get {
                //https://stackoverflow.com/questions/14462820/check-if-indexing-operator-exists
                var dict = textures as Dictionary<string, Texture2D>;
                
                Texture2D result;
                dict.TryGetValue(index, out result);

                //If the string of the index is formated correctly, generate a 2D texture box
                //rect,wth,hgt,(optional thick)
                //Example of format :
                //rect,50,50,1 //Generate a 50x50 pixels rectangle with 1 pixel thick border
                //rect,50,50 //Generate a 50x50 pixels rectangle that is filled
                if (result == null) {
                    string[] contents = index.Split(',');

                    switch (contents[0]) {
                        case "rect":
                            int[] values = new int[3];

                            values[0] = int.Parse(contents[1]);
                            values[1] = int.Parse(contents[2]);
                            values[2] = (contents.Length > 3) ? int.Parse(contents[3]) : 0 ;

                            if (values[2] == 0) {//Create rectangle with border
                                Add(index, Tools.GenerateFilledRectangleTexture2D(graphicsDevice, values[0], values[1], Color.White));
                            } else {//Create filled rectangle
                                Add(index, Tools.GenerateRectangleTexture2D(graphicsDevice, values[0], values[1], values[2], Color.White));
                            }
                            
                            return textures[index];
                        case "circle":
                            break;
                    }
                }
                
                return result;
            }

            set {
                textures[index] = value;
            }
        }

        public void Add(string textureName, Texture2D texture)
        {
            textures.Add(textureName, texture);
        }
    }

    public class FontsOrganizer
    {
        private Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
        GraphicsDevice graphicsDevice;

        public FontsOrganizer(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public SpriteFont this[string index] {
            get {
                return fonts[index];
            }

            set {
                fonts[index] = value;
            }
        }

        public void Add(string fontName, SpriteFont font)
        {
            fonts.Add(fontName, font);
        }
    }

    public class ContentOrganizer
    {
        GraphicsDevice graphicsDevice;
        public TexturesOrganizer textures;
        public FontsOrganizer fonts;

        public ContentOrganizer(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            textures = new TexturesOrganizer(graphicsDevice);
            fonts = new FontsOrganizer(graphicsDevice);
        }
    }
}
