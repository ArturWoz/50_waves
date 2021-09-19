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
        float targetX = 16;
        float targetY;
        Texture2D province_desert, province_farmland, province_forest, province_jungle, province_lake, province_mountains, province_plains, province_sea, province_taiga, province_tundra,province_coast,province_hills;
        Vector2 province_position;
        Vector2 province_offset;
        Vector2 scale;
        string path = "map.txt";
        string s;
        terrain s2;
        int i = 0;
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
            province_hills = Content.Load<Texture2D>("mountains1");
            scale = new Vector2(targetX / (float)province_desert.Width, targetX / (float)province_desert.Width);
            targetY = province_desert.Height * scale.Y;
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            StreamReader sr = File.OpenText(path);
                int x = int.Parse(sr.ReadLine());
                int y = int.Parse(sr.ReadLine());
                Province[,] mapa = new Province[x, y];
            _spriteBatch.Begin();
            for (int k = 0; k < y; k++)
                {
                    for (int k2 = 0; k2 <x ; k2++)
                    {
                    s = sr.ReadLine();
                    s2 = (terrain)Enum.Parse(typeof(terrain), s, true);
                    Province load = new Province(i, "placeholder", 0, s2, false, 0, 0);
                    mapa[k2, k] = load;
                    province_offset = new Vector2(16 * k, 16 * k2);
                    if (s == "desert") _spriteBatch.Draw(province_desert, position: province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (s == "sea") _spriteBatch.Draw(province_sea, position: province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (s == "coast") _spriteBatch.Draw(province_coast, position: province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (s == "lake") _spriteBatch.Draw(province_lake, position: province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (s == "hills") _spriteBatch.Draw(province_hills, position: province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (s == "taiga") _spriteBatch.Draw(province_taiga, position: province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (s == "tundra") _spriteBatch.Draw(province_tundra, position: province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (s == "plains") _spriteBatch.Draw(province_plains, position: province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (s == "farmland") _spriteBatch.Draw(province_farmland, position: province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (s == "forest") _spriteBatch.Draw(province_forest, position: province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else if (s == "mountains") _spriteBatch.Draw(province_mountains, position: province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    else _spriteBatch.Draw(province_sea, position: province_offset, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                    }
                }
            sr.Close();
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
