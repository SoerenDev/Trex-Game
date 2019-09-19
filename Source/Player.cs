using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source {
    class Player {

        private enum PlayerState {
            Idle,
            WalkOne,
            WalkTwo
        }

        private Texture2D[] texture;

        private Vector2 position;
        private Vector2 velocity;
        private Rectangle rectangle;

        private bool isOnFloor;
        private bool state;

        private int deltatime;
        private int extraJumps;
        private int jumps = 2; /* Here you can set the number how often the player can jump */

        private PlayerState playerState;

        public Player() {
            this.position = new Vector2(100, 350);
            this.rectangle = new Rectangle((int)position.X, (int)position.Y, 42, 42);
            this.isOnFloor = false;
            this.extraJumps = this.jumps;
            this.texture = new Texture2D[3];
            this.playerState = PlayerState.WalkOne;
            this.state = true;
        }

        public void Update(GameTime gameTime) {

            this.position += (this.velocity * gameTime.ElapsedGameTime.Milliseconds)/10;
            this.deltatime += gameTime.ElapsedGameTime.Milliseconds;

            /* This is for the walk animation */
            if (this.deltatime > 100) {
                if (this.playerState == PlayerState.WalkOne) {
                    this.playerState = PlayerState.WalkTwo;
                } else {
                    this.playerState = PlayerState.WalkOne;
                }
                this.deltatime = 0;
            }

            /* This is for the jump animation */
            if (!this.isOnFloor) {
                this.playerState = PlayerState.Idle;
            }

            Input();
        }

        public void Load(ContentManager Content) {
            this.texture[0] = Content.Load<Texture2D>("Idle");
            this.texture[1] = Content.Load<Texture2D>("WalkOne");
            this.texture[2] = Content.Load<Texture2D>("WalkTwo");
        }

        /* This methods check if two hitboxes collide */ 
        public bool IsDead(Rectangle rectangle) {
            if (this.rectangle.Right > rectangle.Left &&
                this.rectangle.Left < rectangle.Right &&
                this.rectangle.Top < rectangle.Bottom &&
                this.rectangle.Bottom > rectangle.Top) {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture[(int)this.playerState], this.rectangle, Color.White);
        }

        private void Input() {
            /* These if statements are for the jumps */
            if (this.state && (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))) {

                if ((Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
                    && this.isOnFloor || (this.isOnFloor == false && this.extraJumps > 0)) {
                    this.position.Y -= 10f;
                    this.velocity.Y = -7;
                    this.isOnFloor = false;
                    this.extraJumps--;
                }
                this.state = false;
            }
            /* This if statement sets the state to true when the button is up. We need this for the first if statement in this method */
            if ((Keyboard.GetState().IsKeyUp(Keys.Space) && GamePad.GetState(PlayerIndex.One).IsButtonUp(Buttons.A)) || this.isOnFloor) {
                this.state = true;
            }

            /* This sets the new position of the player. It is important that the position is set before the last of statement of this method. */
            this.rectangle.X = (int)position.X;
            this.rectangle.Y = (int)position.Y;

            /* This if-else statement regulate the fall speed */
            if (this.isOnFloor == false && (Keyboard.GetState().IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown))) {
                this.velocity.Y += 0.9f;
            } else if (isOnFloor == false && !(Keyboard.GetState().IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown))) {
                this.velocity.Y += 0.3f;
            }
            /* When the player is on the ground the the Y velocity speed is set to zero */
            if (this.isOnFloor == true) {
                this.velocity.Y = 0f;
            }
            /* This if statement set the ground */
            if (this.rectangle.Y > 350) {
                this.velocity.Y = 0f;
                this.rectangle.Y = 350;
                this.isOnFloor = true;
                this.extraJumps = jumps;
            }
        }
    }
}
