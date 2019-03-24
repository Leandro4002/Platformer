using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Global {
    public static class Tools
    {
        //Maps a number from one range to another
        public static float Map(this float value, float from1, float to1, float from2, float to2) => (value - from1) / (to1 - from1) * (to2 - from2) + from2;

        //Get hypotenuse
        public static double Pyth(float A, float B) => Math.Sqrt(Math.Pow(A, 2) + Math.Pow(B, 2));

        //Maybe temp
        public static float[] GeneratePerlinNoiseArray(int arraySize, float[] seed, int octaves, float bias)
        {
            float[] output = new float[arraySize];

            for (int x = 0; x < arraySize; x++)
            {
                float noise = 0.0f;
                float scale = 1.0f;
                float scaleAcc = 0.0f;

                for (int o = 0; o < octaves; o++)
                {
                    int pitch = arraySize >> o;
                    int sample1 = (x / pitch) * pitch;
                    int sample2 = (sample1 + pitch) % arraySize;

                    int blend = (x - sample1) / pitch;
                    float sample = (1.0f - blend) * seed[sample1] + blend * seed[sample2];
                    noise += sample * scale;
                    scaleAcc += scale;
                    scale /= bias;
                }

                //Scale to seed range
                output[x] = noise / scaleAcc;
            }

            return output;
        }

        public static void ShiftArray<T>(this T[] array, int steps) {
            while(steps != 0) {
                if (steps > 0) {
                    for (int j = 0; j < array.Length - 1; j++) {
                        array[j] = array[j + 1];
                    }
                    steps--;
                } else if (steps < 0) {
                    for (int j = array.Length; j < 1; j--) {
                        array[j] = array[j - 1];
                    }
                    steps++;
                }
            }
        }

        public static bool RectCollision(float x, float y, float wth, float hgt, float x2, float y2, float wth2, float hgt2)
        {
            if ((x + wth > x2 && x < x2 + wth2) && (y + hgt > y2 && y < y2 + hgt2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Texture2D GenerateFilledRectangleTexture2D(GraphicsDevice graphicsDevice, int width, int height, Color color) {

            Texture2D texture = new Texture2D(graphicsDevice, width, height);

            int numberOfPixels = width * height;

            //Create an array of the size of the image (1 entry = 1 pixel)
            Color[] data = new Color[numberOfPixels];

            //Fill the texture
            for (int i = 0; i < numberOfPixels; i++)
                data[i] = color;

            //Put the data in the texture
            texture.SetData(data);

            return texture;
        }

        //Dirty but functioning
        //Need to add border of image
        public static Texture2D GenerateGridTexture2D(GraphicsDevice graphicsDevice, int cols, int rows, int size, Color color) {

            Texture2D texture = new Texture2D(graphicsDevice, cols * size, rows * size);

            int numberOfPixels = cols * size * rows * size;

            //Create an array of the size of the image (1 entry = 1 pixel)
            Color[] data = new Color[numberOfPixels];

            //Fill horizontal lines
            for (int i = size * cols * size; i < numberOfPixels; i += size * cols * size) {
                //Draw a line to the right
                for (int j = 0; j < cols * size; j++) {
                    data[i + j] = color;
                }

                if (i != 0) {
                    i -= size * cols;
                    //Draw a line to the right
                    for (int j = 0; j < cols * size; j++) {
                        data[i + j] = color;
                    }
                    i += size * cols;
                }
            }

            //Fill vertical lines
            for (int i = size; i < cols * size; i += size) {

                //Draw a line to the bottom
                for (int j = 0; j < numberOfPixels; j += cols * size) {
                    data[i + j] = color;
                }
                
                if (i != 0) {
                    i--;
                    //Draw a line to the bottom
                    for (int j = 0; j < numberOfPixels; j += cols * size) {
                        data[i + j] = color;
                    }
                    i++;
                }
            }

            //Put the data in the texture
            texture.SetData(data);

            return texture;
        }

        public static Texture2D GenerateRectangleTexture2D(GraphicsDevice graphicsDevice, int width, int height, int thick, Color color) {
            if (thick < 1 || thick > Math.Min(width, height) / 2)
                throw new Exception("Rectangle thickness can't be less than 1 or beeing smaller or equal than half of the width divided by height of the texture");

            Texture2D texture = new Texture2D(graphicsDevice, width, height);

            //Create an array of the size of the image (1 entry = 1 pixel)
            Color[] data = new Color[width * height];

            //Create upper line
            for (int i = 0; i < width * thick; ++i)
                data[i] = color;

            //Create down line
            for (int i = (height - thick) * width; i < data.Length; ++i)
                data[i] = color;

            //Create left line
            for (int j = 0; j < thick; j++) {
                for (int i = j; i < data.Length; i += width)
                    data[i] = color;
            }

            //Create right line
            for (int j = 1; j < thick + 1; j++) {
                for (int i = width - j; i < data.Length; i += width)
                    data[i] = color;
            }

            //Put the data in the texture
            //Si il y a l'exception "invalid argument" :
            //https://stackoverflow.com/questions/32002182/bitmap-creation-problems-invalid-parameter-c-sharp
            texture.SetData(data);

            return texture;
        }
    }
}
