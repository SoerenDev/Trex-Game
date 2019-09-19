using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Source {
    /* These are the gamestates */ 
    enum GameState {
        MainMenu,
        Gameplay,
        EndOfGame
    }

    public class Main : Game {

        /* Here are the declarations for the game */
        private SpriteFont font;

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        private Player player;
        private Cactus[] cactus;
        private Enemy[] enemy;
        private Scrolling[] floor;

        private int[] deltatime;
        private int[] objectCounter;

        private Button start;
        private Button restart;
        private Button stop;

        private GameState _state;

        /* Here are the moste of the definitions for the game */
        public Main() {
            this.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            /* Here you can set the number of enemies and cactuses you will have in your game */
            this.cactus = new Cactus[5];
            this.enemy = new Enemy[5];
            this.floor = new Scrolling[2];

            this.deltatime = new int[2];
            this.objectCounter = new int[2];

            this.start = new Button(300, 200, "start");
            this.restart = new Button(300, 150, "restart");
            this.stop = new Button(300, 230, "stop");

            this._state = GameState.MainMenu;
        }

        protected override void Initialize() {
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            this.start.Load(Content);
            this.restart.Load(Content);
            this.stop.Load(Content);
        }

        protected override void Update(GameTime gameTime) {
            /* With this shortcuts you can close the game */
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            switch (this._state) {
                case GameState.MainMenu:
                    this.UpdateMainMenu(gameTime);
                    break;
                case GameState.Gameplay:
                    this.UpdateGameplay(gameTime);
                    break;
                case GameState.EndOfGame:
                    this.UpdateEndOfGame(gameTime);
                    break;
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime) {
            spriteBatch.Begin();

            switch (this._state) {
                case GameState.MainMenu:
                    this.DrawMainMenu(spriteBatch);
                    break;
                case GameState.Gameplay:
                    this.DrawGameplay(spriteBatch);
                    break;
                case GameState.EndOfGame:
                    this.DrawEndOfGame(spriteBatch);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /* This method update the main menu */
        void UpdateMainMenu(GameTime gameTime) {
            IsMouseVisible = true;
            /* Here sets the program the gamestate to Gameplay when the Button is pressed */
            if (this.start.IsClicked() || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A)) {
                this.LoadGamePlay();
                this._state = GameState.Gameplay; 
            }
        }

        /* This method update all the stuff for the main game */
            void UpdateGameplay(GameTime gameTime) {

            this.player.Update(gameTime);
            /* This foreach loop calculate the new position for the cactuses */
            foreach (Cactus oneCactus in this.cactus) {
                if (oneCactus != null) {
                    oneCactus.Update(gameTime);
                }
            }
            /* This foreach loop calculate the new position for the enemies */
            foreach (Enemy oneEnemy in this.enemy) {
                if (oneEnemy != null) {
                    oneEnemy.Update(gameTime);
                }
            }
            /* The two if statements below set the position back for the floor */
            if ((this.floor[0].rectangle.X + this.floor[0].rectangle.Width) <= 0) {
                this.floor[0].rectangle.X = this.floor[1].rectangle.X + this.floor[1].rectangle.Width;
            }
            if ((this.floor[1].rectangle.X + this.floor[1].rectangle.Width) <= 0) {
                this.floor[1].rectangle.X = this.floor[0].rectangle.X + this.floor[0].rectangle.Width;
            }
            /* These two lines code calculate the position for the floor */
            this.floor[0].Update(gameTime);
            this.floor[1].Update(gameTime);
            /* Here we save the respawn time for the cactuses and enemies*/
            this.deltatime[0] += gameTime.ElapsedGameTime.Milliseconds;
            this.deltatime[1] += gameTime.ElapsedGameTime.Milliseconds;
            /* This if statements respawn new cactuses */
            if (this.deltatime[0] > 1500) {
                this.deltatime[0] = 0;
                this.cactus[objectCounter[0] % cactus.Length] = new Cactus();
                this.cactus[objectCounter[0] % cactus.Length].Load(Content);
                this.objectCounter[0]++;
            }
            /* This if statement respawn new enemies */
            if (this.deltatime[1] > 8000) {
                this.deltatime[1] = 0;

                this.enemy[objectCounter[1] % enemy.Length] = new Enemy();
                this.enemy[objectCounter[1] % enemy.Length].Load(Content);

                this.objectCounter[1]++;
            }
            /* Here the program check if the player collide with an enemy or a cactus */
            foreach (Enemy oneEnemy in this.enemy) {
                foreach (Cactus oneCactus in this.cactus) {

                    if (this.player != null && ((oneCactus != null && this.player.IsDead(oneCactus.rectangle)) 
                        || (oneEnemy != null && this.player.IsDead(oneEnemy.rectangle)))) {
                        this._state = GameState.EndOfGame;
                        this.player = null;
                        for (int i = 0; i < this.cactus.Length; i++) {
                            this.cactus[i] = null;
                        }
                        for (int i = 0; i < this.enemy.Length; i++) {
                            this.enemy[i] = null;
                        }
                        for (int i = 0; i < this.floor.Length; i++) {
                            this.floor[i] = null;
                        }
                        IsMouseVisible = true;
                    }
                }
            }
        }

        /* This method update the game scene where you can choose if you retry or close the game */
        void UpdateEndOfGame(GameTime gameTime) {
            if (this.restart.IsClicked() || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.X)) {
                this.LoadGamePlay();
                this._state = GameState.Gameplay;
            }
            if (this.stop.IsClicked()) {
                Exit();
            }
        }

        /* This method draw the main menue */
        void DrawMainMenu(SpriteBatch spriteBatch) {
            GraphicsDevice.Clear(Color.AntiqueWhite);
            this.start.Draw(spriteBatch);
        }

        /* This method draw all the stuff for the main game */
        void DrawGameplay(SpriteBatch spriteBatch) {
            GraphicsDevice.Clear(Color.AntiqueWhite);
            spriteBatch.DrawString(font, "Score: " + this.objectCounter[0] * 10 + "m", new Vector2(700, 30), Color.Black);

            /* This foreach loop draw all the cactuses */
            foreach (Cactus oneCatus in this.cactus) {
                if (oneCatus != null) {
                    oneCatus.Draw(spriteBatch);
                }
            }

            /* This foreach loop draw all the enemies */
            foreach (Enemy oneEnemy in this.enemy) {
                if (oneEnemy != null) {
                    oneEnemy.Draw(spriteBatch);
                }
            }
            /* These three lines code draw the floor and the player */
            this.floor[0].Draw(spriteBatch);
            this.floor[1].Draw(spriteBatch);
            this.player.Draw(spriteBatch);
        }

        /* This method draw the game scene where you can choose if you retry or close the game */
        void DrawEndOfGame(SpriteBatch spriteBatch) {
            GraphicsDevice.Clear(Color.AntiqueWhite);
            this.stop.Draw(spriteBatch);
            this.restart.Draw(spriteBatch);
        }

        /* This method load all the stuff for the new game */
        void LoadGamePlay() {
            this.player = new Player();
            this.floor[0] = new Scrolling(Content.Load<Texture2D>("Floor"), new Rectangle(0, 383, 800, 20));
            this.floor[1] = new Scrolling(Content.Load<Texture2D>("Floor"), new Rectangle(800, 383, 800, 20));
            IsMouseVisible = false;
            this.player.Load(Content);
            this.font = Content.Load<SpriteFont>("Score");
            this.objectCounter[0] = 0;
        }
    }
}
