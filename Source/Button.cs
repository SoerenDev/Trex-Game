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
    class Button {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle rectangle;

        private String name;

        public Button(int x, int y, string name) {
            this.position = new Vector2(x, y);
            this.rectangle = new Rectangle((int)this.position.X, (int)this.position.Y, 200, 70);
            this.name = name;
        }
        
        public bool IsClicked() {
            MouseState mouseState = Mouse.GetState();

            if (Collision() &&
                mouseState.LeftButton == ButtonState.Pressed) {
                return true;
            }
            return false;
        }

        public void Load(ContentManager Content) {
            this.texture = Content.Load<Texture2D>(this.name);
        }

        public void Draw(SpriteBatch spriteBatch) {
            MouseState mouseState = Mouse.GetState();
            if (this.Collision()) {
                spriteBatch.Draw(this.texture, this.rectangle, Color.GreenYellow);
            } else {
                spriteBatch.Draw(this.texture, this.rectangle, Color.White);
            }
        }
        /* This method return true when the mouse is over the button */
        private bool Collision() {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.X > this.rectangle.Left &&
                mouseState.X < this.rectangle.Right &&
                mouseState.Y > this.rectangle.Top &&
                mouseState.Y < this.rectangle.Bottom) {
                return true;
            }
            return false;
        }
    }
}
