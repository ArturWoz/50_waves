using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projekt
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
<<<<<<< Updated upstream

=======
        float targetX =128  ;
        float targetY;
        float zoom = 1;
        Texture2D province_desert, province_farmland, province_forest, province_jungle, province_lake, province_mountains, province_plains, province_sea, province_taiga, province_tundra,province_coast,province_hills;
        Vector2 Camera_position = Vector2.Zero;
        Vector2 scale;
        string path = "map.txt";
        string s;
        string[,] mapaS; 
        Province[,] mapa;
        terrain s2;
        int i = 0,x,y;
>>>>>>> Stashed changes
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
<<<<<<< Updated upstream

=======
            KeyboardState keystate = Keyboard.GetState();
            if(keystate.IsKeyDown(Keys.W))
            {
                Camera_position.Y = Camera_position.Y - (20/zoom);
            }
            if (keystate.IsKeyDown(Keys.S))
            {
                Camera_position.Y = Camera_position.Y + (20/zoom);
            }
            if (keystate.IsKeyDown(Keys.A))
            {
                Camera_position.X = Camera_position.X -(20/zoom);
            }
            if (keystate.IsKeyDown(Keys.D))
            {
                Camera_position.X = Camera_position.X + (20/zoom);
            }
            if (keystate.IsKeyDown(Keys.Up)&&zoom>0.5   )
            {
                Camera_position = Camera_position * 1.01f;
                zoom = zoom - (float)0.01;
            }
            if (keystate.IsKeyDown(Keys.Down)&&zoom<8)
            {
                Camera_position = Camera_position * 0.99f;
                zoom = zoom + (float)0.01;
            }
>>>>>>> Stashed changes
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
