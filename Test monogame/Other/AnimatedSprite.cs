using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Global
{
    public class AnimatedSprite
    {
        Texture2D texture;

        int frameX, frameY = 0;
        int frameWidth, frameHeight = 0;

        int currentFrame, totalFrames = 0;
        int fps = 0;


        float timer = 0;
        int cols, rows = 0;

        int centerMethod = 0;

        bool isVisible = true;
        bool isLooping = true;
        bool isPlaying = true;
        bool isInvertedX = false;
        bool isInvertedY = false;

        Color color = Color.White;

        int rotation = 0;
        int sx = 1;
        int sy = 1;
        int ox = 0;
        int oy = 0;
        int kx = 0;
        int ky = 0;

        public AnimatedSprite(Texture2D texture, int totalFrames, int frameWth, int frameHgt, int cols, int rows, int fps = 10)
        {
            this.texture = texture;
            this.totalFrames = totalFrames;
            this.frameWidth = frameWth;
            this.frameHeight = frameHgt;
            this.cols = cols;
            this.rows = rows;
            this.fps = fps;
        }

        public void Update(float dt)
        {
            if (isPlaying)
            {
                timer -= dt;

                if (timer <= 0)
                {
                    //Recalculate time between every frame change
                    timer = 1 / fps;

                    currentFrame++;

                    //If this is the last frame
                    if (currentFrame >= totalFrames)
                    {
                        if (isLooping)
                        {//Set the current frame to 0 (reset the animation)
                            currentFrame = 0;
                        }
                        else
                        {//Pause the animation and set the currentFrame to the total of frames
                            currentFrame--;
                            isPlaying = false;
                        }
                    }

                    calculateCurrentFramePosition();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            if (isVisible)
            {
                Rectangle sourceRectangle = new Rectangle(frameX, frameY, frameWidth, frameHeight);
                Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, frameWidth, frameWidth);

                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
            }
        }

        private void calculateCurrentFramePosition()
        {
            //Find actual line
            int row = currentFrame / cols;

            //Detect if the currentFrame is at the last column
            if (currentFrame % cols == 0 && row > 0)
            {
                row--;
            }

            //Find actual column
            int col = (currentFrame - 1) % cols;

            //Find the location of the current frame in the spritesheet
            frameX = 1 + col * 2 + col * frameWidth;
            frameY = 1 + row * 2 + row * frameHeight;
        }
    }
}
