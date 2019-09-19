using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source {
    class Enemy {

        private Texture2D[] texture;
        public Rectangle rectangle { get; private set; }
        private Vector2 position;

        private int deltatime = 0;

        private enum EnemyState {
            FlyOne,
            FlyTwo
        }

        private EnemyState enemyState;

        public Enemy() {
            this.texture = new Texture2D[2];
            this.position = new Vector2(800, 350);
            this.enemyState = EnemyState.FlyOne;
        }
        public void Load(ContentManager Content) {
            this.texture[0] = Content.Load<Texture2D>("FlyOne");
            this.texture[1] = Content.Load<Texture2D>("FlyTwo");
        }

        public void Update(GameTime gameTime) {
            this.position.X -= 0.6f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            this.rectangle = new Rectangle((int)this.position.X, (int)this.position.Y, 42, 42);

            this.deltatime += gameTime.ElapsedGameTime.Milliseconds;

            if (this.deltatime > 100) {
                if (this.enemyState == EnemyState.FlyOne) {
                    this.enemyState = EnemyState.FlyTwo;
                } else {
                    this.enemyState = EnemyState.FlyOne;
                }
                this.deltatime = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(this.texture[(int)enemyState], this.rectangle, Color.White);
        }
    }
}
