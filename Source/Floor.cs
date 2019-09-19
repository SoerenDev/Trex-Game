using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source {
    class Floor {
        protected Texture2D texture;
        public Rectangle rectangle;
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(this.texture, this.rectangle, Color.White);
        }
    }

    class Scrolling : Floor {
        public Scrolling(Texture2D texture, Rectangle rectangle) {
            this.texture = texture;
            this.rectangle = rectangle;
        }
        public void Update(GameTime gameTime) {
            this.rectangle.X -= (int)(0.4f * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
        }
    }
}
