using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpriteAnimation{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D player; // this will hold the player image/sprite sheet
        Vector2 location; // This will be the location of where we draw the texture
        int frameWidth = 21; // we'll hard code the width
        int frameHeight = 17; // and the height
        int currentFrame = 0; // the current frame of the animation
        int frameCount = 0; // how many frames exist in the animation
        List<Rectangle> frames = new List<Rectangle>(); // this will contain all the individual frames of an animation
        float frameTime = 0.025f; // how long we should wait before we render another frame
        int animationDirection = 1; // this will control which way we should animate the image
        float elapsedTime = 0f; // this is the elapsed time since the image has rendered a frame


        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // this loads the player texture, which is 7 frames
            // if you look at the image, the base image of the player (not moving) is in the center of the image (or, frame 4). To the left and the right, it shows the player moving
            // the decision for this is that when they reach each respective corner, you simply play the animation to the other end and have a fluid animation
            player = Content.Load<Texture2D>("Player");
            frameCount = player.Width / frameWidth; // even though we know the animation is 7 frames, we'll use the math in case we expand on the image to be a larger or smaller number

            // iterate until we reach the frame count, addint each frame to the list of frames
            for (int i = 0; i < frameCount; i++) {
                frames.Add(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight));
            }

            // we'll set the location of where to display the player texture here, as it won't be moving around the screen. Again, using the math to display it in the middle of the screen
            // and taking into account the width of the player image
            location = new Vector2((Window.ClientBounds.Width / 2) + (frameWidth / 2), (Window.ClientBounds.Height / 2) + (frameHeight / 2));
        }

       

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // we look at the elapsed time of the game running and add it to the frame time
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (elapsedTime >= frameTime) { // we look to see if the elapsed time since the last frame was rendered is equal to or has exceeded the allowed time between frames. if it has, we need to render the next frame
                // if the animation has reached the right side, move to the left
                if (currentFrame == frames.Count - 1)
                    animationDirection = -1;
                
                // if the animation has reached the left side, move to the right
                if(currentFrame == 0)
                    animationDirection = 1;

                currentFrame += animationDirection; // play the animation in the specified direction (left, or right)
                elapsedTime = 0f; // and reset the timer so that we can figure out when to render the next frame
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(player, location, frames[currentFrame], Color.White); // simply draw the play animation in the middle of the screen
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
