using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Projekt
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        float targetX =128; //province size
        float targetY;
        float zoom = 1;
        Texture2D province_desert, province_farmland, province_forest, province_jungle, province_lake, province_mountains, province_plains, province_sea, province_taiga, province_tundra,province_coast,province_hills;
        Vector2 Camera_position = Vector2.Zero;
        Vector2 Mouse_position;
        Vector2 Scale;
        Vector2 Highlighted_province;
        Vector2 Prev_highlighted_province;
        string path = "map.txt";
        string s; //string used for loading map files 
        Province[,] mapa;
        terrain s2; //string s converted to type terrain
        SpriteFont font;
        int i = 0, x, y, scroll = 0; //Yprov will be always remembered :(
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected Vector2 ProvinceIDToMapCoordinate(int id)
        {
            Vector2 output;
            output.Y = id / this.y;
            output.X = id % this.y;
            return output;
        }
        protected Vector2 MouseToMapCoordinate(Vector2 Mouse_position) // return X and Y coordinates of the province that mouse points at
        {
            Vector2 Output;
            Vector2 Province_size = new Vector2(this.targetX / this.zoom, this.targetY / this.zoom);
            Vector2 Camera_offset = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            Vector2 Temp = Mouse_position - this.Camera_position/zoom - Camera_offset;
            Output.X = (int) (Temp.X / Province_size.X);
            Output.Y = (int) (Temp.Y / Province_size.Y);
            return Output;
        }

        protected Texture2D TerrainToTexture (terrain SS)
        {
            if (SS == terrain.desert) return province_desert;
            else if (SS == terrain.sea) return province_sea;
            else if (SS == terrain.coast) return province_coast;
            else if (SS == terrain.lake) return province_lake;
            else if (SS == terrain.hills) return province_hills;
            else if (SS == terrain.taiga) return province_taiga;
            else if (SS == terrain.tundra) return province_tundra;
            else if (SS == terrain.plains) return province_plains;
            else if (SS == terrain.farmland) return province_farmland;
            else if (SS == terrain.forest) return province_forest;
            else if (SS == terrain.mountains) return province_mountains;
            else if (SS == terrain.jungle) return province_jungle;
            else return province_sea;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width; // window size to be the display size
            _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            StreamReader sr = File.OpenText(path);
            x = int.Parse(sr.ReadLine()); // map dimensions
            y = int.Parse(sr.ReadLine());
            mapa = new Province[x, y];

            for (int k = 0; k < y; k++) // loading map file
            {
                for (int k2 = 0; k2 < x; k2++)
                {
                    s = sr.ReadLine();
                    s2 = (terrain)Enum.Parse(typeof(terrain), s, true);
                    Province load = new Province(i, "placeholder", 0, s2, false, 0, 0);
                    mapa[k2, k] = load;
                    i++;
                }
            }

            sr.Close();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice); //loading graphics
            font = Content.Load<SpriteFont>("defaultFont");
            province_desert = Content.Load<Texture2D>("desert1");
            province_farmland = Content.Load<Texture2D>("farmland1");
            province_forest = Content.Load<Texture2D>("forest1");
            province_jungle = Content.Load<Texture2D>("jungle1");
            province_lake = Content.Load<Texture2D>("lake1");
            province_mountains = Content.Load<Texture2D>("mountains1");
            province_plains = Content.Load<Texture2D>("plains1");
            province_sea = Content.Load<Texture2D>("sea1");
            province_taiga = Content.Load<Texture2D>("taiga1");
            province_tundra = Content.Load<Texture2D>("tundra1");
            province_coast = Content.Load<Texture2D>("coast1");
            province_hills = Content.Load<Texture2D>("hills1");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState keystate = Keyboard.GetState();
            MouseState mousestate = Mouse.GetState();

            if(keystate.IsKeyDown(Keys.S) | mousestate.Y >= _graphics.PreferredBackBufferHeight-15) //camera movement
            {
                Camera_position.Y = Camera_position.Y - (30*zoom);
            }
            if (keystate.IsKeyDown(Keys.W) | mousestate.Y <= 25)
            {
                Camera_position.Y = Camera_position.Y + (30*zoom);
            }
            if (keystate.IsKeyDown(Keys.D) | mousestate.X >= _graphics.PreferredBackBufferWidth - 15)
            {
                Camera_position.X = Camera_position.X -(30*zoom);
            }
            if (keystate.IsKeyDown(Keys.A) | mousestate.X <= 25)
            {
                Camera_position.X = Camera_position.X + (30*zoom);
            }
            if (keystate.IsKeyDown(Keys.Up) &&zoom>0.5) //zoom
            {
                zoom = zoom - (float)0.025;
            }
            if (keystate.IsKeyDown(Keys.Down) && zoom<8)
            {
                zoom = zoom + (float)0.025;
            }

            if (mousestate.ScrollWheelValue > scroll && zoom > 0.5)
            {
                zoom = zoom - (float)0.25;
            }
            if (mousestate.ScrollWheelValue < scroll && zoom < 8)
            {
                zoom = zoom + (float)0.25;
            }
            if (zoom < 0.5) zoom = 0.5f; //zoom cap
            if (zoom > 8) zoom = 8;

            
               
            // TODO: Add your update logic here
            Scale = new Vector2(targetX / zoom / (float)province_desert.Width, targetX / zoom / (float)province_desert.Height); // adjusting scrolling to different camera positions
            targetY = targetX;
            scroll = mousestate.ScrollWheelValue;

            int Xpos, Ypos; //reading mouse position
            Xpos = Mouse.GetState().Position.X;
            Ypos = Mouse.GetState().Position.Y;
            Mouse_position = new Vector2(Xpos, Ypos);
            Vector2 Test = MouseToMapCoordinate(Mouse_position);
            if ((int)Test.X > -1 && (int)Test.Y > -1 && (int)Test.X < x && (int)Test.Y < y) // x and y are map dimensions
            {
                Highlighted_province = MouseToMapCoordinate(Mouse_position);
            }
            if (mousestate.LeftButton == ButtonState.Pressed)
            {
                mapa[(int)Prev_highlighted_province.X, (int)Prev_highlighted_province.Y].SetClicked(false);
                mapa[(int)Highlighted_province.X, (int)Highlighted_province.Y].SetClicked(true);
                Prev_highlighted_province = Highlighted_province;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            for (int k = 0; k < y; k++) //drawing provinces
                {

                for (int k2 = 0; k2 < x; k2++)
                {
                    terrain SS = mapa[k2, k].GetTerrain();
                    Vector2 Province_offset = new Vector2(targetX / zoom * k, targetY / zoom * k2);
                    Vector2 Camera_offset = new Vector2(_graphics.PreferredBackBufferWidth/2, _graphics.PreferredBackBufferHeight / 2);
                    _spriteBatch.Draw(TerrainToTexture(SS), position: ((Camera_position) / zoom + Camera_offset) + Province_offset, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
                    if (mapa[k, k2].GetClicked()) _spriteBatch.Draw(province_jungle, position: ((Camera_position) / zoom + Camera_offset) + Province_offset, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
                }
            }

            //show camera position and province id
            
            _spriteBatch.DrawString(font,Camera_position.ToString()+"\n"+Mouse_position.ToString() + '\n' + mapa[(int)Highlighted_province.X,(int)Highlighted_province.Y].GetID() +'\n'+ mapa[(int)Highlighted_province.X, (int)Highlighted_province.Y].GetClicked(), Vector2.Zero,Color.OrangeRed);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
