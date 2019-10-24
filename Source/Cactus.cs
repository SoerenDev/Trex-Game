using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source {
    class Cactus {
        private Texture2D texture;
        public Rectangle rectangle { get; private set; }
        private Vector2 position;

        public void Load(ContentManager Content) {
            this.texture = Content.Load<Texture2D>("Cactus");
            this.position = new Vector2(800, 350);
        }

        public void Update(GameTime gameTime) {
            this.position.X -= (int)(0.4f * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            this.rectangle = new Rectangle((int)this.position.X, (int)this.position.Y, 42, 42);
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(this.texture, this.rectangle, Color.White);
        }

    }
}
