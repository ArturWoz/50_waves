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
        float targetX =128;
        float targetY;
        float zoom = 1;
        Texture2D province_desert, province_farmland, province_forest, province_jungle, province_lake, province_mountains, province_plains, province_sea, province_taiga, province_tundra,province_coast,province_hills;
        Vector2 Camera_position = Vector2.Zero;
        Vector2 Mouse_position;
        Vector2 scale;
        string path = "map.txt";
        string s;
        string[,] mapaS; 
        Province[,] mapa;
        terrain s2;
        SpriteFont font;
        int i = 0, x, y, scroll = 0,Xprov,Yprov;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            StreamReader sr = File.OpenText(path);
            x = int.Parse(sr.ReadLine());
            y = int.Parse(sr.ReadLine());
            mapa = new Province[x, y];
            mapaS = new string[x, y];

            for (int k = 0; k < y; k++)
            {
                for (int k2 = 0; k2 < x; k2++)
                {
                    s = sr.ReadLine();
                    mapaS[k2, k] = s;
                    s2 = (terrain)Enum.Parse(typeof(terrain), s, true);
                    Province load = new Province(i, "placeholder", 0, s2, false, 0, 0);
                    mapa[k2, k] = load;
                }
            }

            sr.Close();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
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

            if(keystate.IsKeyDown(Keys.S) | mousestate.Y >= _graphics.PreferredBackBufferHeight-15)
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
            if (keystate.IsKeyDown(Keys.Up) &&zoom>0.5)
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
            if (zoom < 0.5) zoom = 0.5f;
            if (zoom > 8) zoom = 8;
               
            // TODO: Add your update logic here
            scale = new Vector2(targetX / zoom / (float)province_desert.Width, targetX / zoom / (float)province_desert.Height);
            targetY = targetX;
            scroll = mousestate.ScrollWheelValue;
            int Xpos, Ypos;
            Xpos = Mouse.GetState().Position.X;
            Ypos = Mouse.GetState().Position.Y;
            Mouse_position = new Vector2(Xpos, Ypos);

            for (int k = 0; k < y; k++)
            {

                for (int k2 = 0; k2 < x; k2++)
                {
                    Vector2 size = new Vector2(targetX / zoom , targetY / zoom );
                    Vector2 province_offset = new Vector2(targetX / zoom * k, targetY / zoom * k2);
                    Vector2 camera_offset = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
                    Vector2 UL= ((Camera_position) / zoom + camera_offset) + province_offset;
                    Vector2 DR =UL+size;
                    if(UL.X<=Xpos && Xpos<=DR.X && DR.Y>=Ypos && Ypos>=UL.Y)
                    {
                        Xprov = k;
                        Yprov = k2;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            for (int k = 0; k < y; k++)
                {

                for (int k2 = 0; k2 < x; k2++)
                {
                    string SS = mapaS[k2, k];
                    Vector2 province_offset = new Vector2(targetX / zoom * k, targetY / zoom * k2);
                    Vector2 camera_offset = new Vector2(_graphics.PreferredBackBufferWidth/2, _graphics.PreferredBackBufferHeight / 2);
                    if (SS == "desert") _spriteBatch.Draw(province_desert, position: ((Camera_position)/zoom+camera_offset) + province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (SS == "sea") _spriteBatch.Draw(province_sea, position: ((Camera_position) / zoom + camera_offset) + province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (SS == "coast") _spriteBatch.Draw(province_coast, position: ((Camera_position) / zoom + camera_offset) + province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (SS == "lake") _spriteBatch.Draw(province_lake, position: ((Camera_position) / zoom + camera_offset) + province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (SS == "hills") _spriteBatch.Draw(province_hills, position: ((Camera_position) / zoom + camera_offset) + province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (SS == "taiga") _spriteBatch.Draw(province_taiga, position: ((Camera_position) / zoom + camera_offset) + province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (SS == "tundra") _spriteBatch.Draw(province_tundra, position: ((Camera_position) / zoom + camera_offset) + province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (SS == "plains") _spriteBatch.Draw(province_plains, position: ((Camera_position) / zoom + camera_offset) + province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (SS == "farmland") _spriteBatch.Draw(province_farmland, position: ((Camera_position) / zoom + camera_offset) + province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (SS == "forest") _spriteBatch.Draw(province_forest, position: ((Camera_position) / zoom + camera_offset) + province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (SS == "mountains") _spriteBatch.Draw(province_mountains, position: ((Camera_position) / zoom + camera_offset) + province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (SS == "jungle") _spriteBatch.Draw(province_jungle, position: ((Camera_position) / zoom + camera_offset) + province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else _spriteBatch.Draw(province_sea, position: ((Camera_position) / zoom + camera_offset) + province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                }
            }
            //_spriteBatch.DrawString(font,Camera_position.ToString()+"\n"+Mouse_position.ToString(),Vector2.Zero,Color.Black);
            _spriteBatch.DrawString(font,Xprov.ToString()+"\n"+Yprov.ToString(),Vector2.Zero,Color.Black);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
