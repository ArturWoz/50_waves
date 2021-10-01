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
        float targetX = 128; //province size
        float targetY;
        float zoom = 1;
        Texture2D province_desert, province_farmland, province_forest, province_jungle, province_lake, province_mountains, province_plains, province_sea, province_taiga, province_tundra, province_coast, province_hills,province_highlight,province_city, provInterface, nationInterface,turnHUD,kobold_settler;
        Vector2 Camera_position = Vector2.Zero;
        Vector2 Mouse_position;
        Vector2 Scale;
        Vector2 Highlighted_province;
        Vector2 Prev_highlighted_province;
        Settler ClickedSettler;
        bool global_clicked = false;
        object Unit; // selected unit
        string path = "map.txt";
        string s; //string used for loading map files 
        Province[,] mapa;
        terrain s2; //string s converted to type terrain
        SpriteFont font;
        private FrameCounter _frameCounter = new FrameCounter();
        int i = 0, x, y, scroll = 0; //Yprov will be always remembered :(
        Nation Kobold = new Nation(1,"Kobolds");
        bool check_unit = true;
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
            Vector2 Temp = Mouse_position - this.Camera_position / zoom - Camera_offset;
            Output.X = (int)(Temp.X / Province_size.X);
            Output.Y = (int)(Temp.Y / Province_size.Y);
            return Output;
        }

        protected Texture2D TerrainToTexture(terrain SS)
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
            else if (SS == terrain.city) return province_city;
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
            Province spawnpoint = mapa[15, 15];
            Settler kobold_settler=new Settler(spawnpoint,Kobold);
            Kobold.AddUnits(kobold_settler);
            
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
            province_highlight = Content.Load<Texture2D>("provinceHighlight");
            provInterface = Content.Load<Texture2D>("provInterfacePlaceholder");
            nationInterface = Content.Load<Texture2D>("countryHUD");
            turnHUD = Content.Load<Texture2D>("turnHUD");
            kobold_settler = Content.Load<Texture2D>("kobold_osadnik_papież");
            province_city = Content.Load<Texture2D>("village");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState keystate = Keyboard.GetState();
            MouseState mousestate = Mouse.GetState();

            if (keystate.IsKeyDown(Keys.S) | mousestate.Y >= _graphics.PreferredBackBufferHeight - 15) //camera movement
            {
                Camera_position.Y = Camera_position.Y - (30 * zoom);
            }
            if (keystate.IsKeyDown(Keys.W) | mousestate.Y <= 25)
            {
                Camera_position.Y = Camera_position.Y + (30 * zoom);
            }
            if (keystate.IsKeyDown(Keys.D) | mousestate.X >= _graphics.PreferredBackBufferWidth - 15)
            {
                Camera_position.X = Camera_position.X - (30 * zoom);
            }
            if (keystate.IsKeyDown(Keys.A) | mousestate.X <= 25)
            {
                Camera_position.X = Camera_position.X + (30 * zoom);
            }
            if (keystate.IsKeyDown(Keys.Up) && zoom > 0.5) //zoom
            {
                zoom = zoom - (float)0.025;
            }
            if (keystate.IsKeyDown(Keys.Down) && zoom < 8)
            {
                zoom = zoom + (float)0.025;
            }
            if (keystate.IsKeyDown(Keys.Space))
            {
                if(ClickedSettler != null)
                {
                    ClickedSettler.CreateCity(Kobold, 1, "Nirwana");
                    Kobold.RemoveUnits(ClickedSettler);
                    ClickedSettler = null;
                }
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
                if (mapa[(int)Highlighted_province.X, (int)Highlighted_province.Y].HasUnit()!=false)
                {
                    while (check_unit)
                    {
                        foreach (Settler settler in Kobold.Units)
                        {
                            if (settler.GetPosition().GetID() == Highlighted_province.X *x + Highlighted_province.Y * 1)
                            { 
                                ClickedSettler = settler;
                                check_unit = false;
                            }
                        }
                    }
                }
                else
                {
                    global_clicked = true;
                    mapa[(int)Prev_highlighted_province.X, (int)Prev_highlighted_province.Y].SetClicked(false);
                    mapa[(int)Highlighted_province.X, (int)Highlighted_province.Y].SetClicked(true);
                    Prev_highlighted_province = Highlighted_province;
                }
            }
            if (mousestate.RightButton == ButtonState.Pressed)
            {
                global_clicked = false;
                mapa[(int)Prev_highlighted_province.X, (int)Prev_highlighted_province.Y].SetClicked(false);
            }
           

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            Vector2 Camera_offset = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            for (int k = 0; k < y; k++) //drawing provinces
            {

                for (int k2 = 0; k2 < x; k2++)
                {
                    terrain SS = mapa[k2, k].GetTerrain();
                    Vector2 Province_offset = new Vector2(targetX / zoom * k, targetY / zoom * k2);
                    _spriteBatch.Draw(TerrainToTexture(SS), position: ((Camera_position) / zoom + Camera_offset) + Province_offset, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
                    if (mapa[k, k2].GetClicked()) _spriteBatch.Draw(province_highlight, position: ((Camera_position) / zoom + Camera_offset) + Province_offset, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
                }
            }
            foreach(Settler Settler in Kobold.Units)
            {
                Province settler_positionProvince = Settler.GetPosition();
                Vector2 settler_positionID = ProvinceIDToMapCoordinate(settler_positionProvince.GetID());
                Vector2 unit_offset = new Vector2(targetX / zoom * settler_positionID.X, targetY / zoom * settler_positionID.Y);
                Settler.GetPosition().SetUnit(true);
                _spriteBatch.Draw(kobold_settler, position: ((Camera_position) / zoom + Camera_offset) + unit_offset, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);

            }
            //show camera position and province id


            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _frameCounter.Update(deltaTime);
            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
            Vector2 interface_position = new Vector2(0, _graphics.PreferredBackBufferHeight - 289);
            Vector2 HUD_position = new Vector2(_graphics.PreferredBackBufferWidth - 350, 0);
            Vector2 interface_offset1 = new Vector2(100, 35);
            Vector2 interface_offset2 = new Vector2(150, 200);
            if (global_clicked)
            {
                _spriteBatch.Draw(provInterface, position: interface_position, null, Color.White, 0, Vector2.Zero, 1, 0, 0);
                _spriteBatch.DrawString(font, "type" + " " + mapa[(int)Prev_highlighted_province.Y, (int)Prev_highlighted_province.X].GetTerrain().ToString(), interface_position + interface_offset1, Color.OrangeRed);
                _spriteBatch.DrawString(font, "Movement Cost" + " " + mapa[(int)Prev_highlighted_province.Y, (int)Prev_highlighted_province.X].GetProvince_movement().ToString(), interface_position + interface_offset2, Color.OrangeRed);

            }
            _spriteBatch.Draw(nationInterface, position: Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, 0, 0);
            _spriteBatch.Draw(turnHUD, position: HUD_position,null, Color.White, 0, Vector2.Zero, 1, 0, 0);
            _spriteBatch.DrawString(font, "100", Vector2.Zero + Vector2.UnitY * 40 + Vector2.UnitX * 50, Color.OrangeRed);
            _spriteBatch.DrawString(font, "696969", Vector2.Zero + Vector2.UnitY * 40 + Vector2.UnitX * 350, Color.OrangeRed);
            _spriteBatch.DrawString(font, "420", Vector2.Zero + Vector2.UnitY * 40 + Vector2.UnitX * 650, Color.OrangeRed);
            _spriteBatch.DrawString(font, "0", HUD_position+Vector2.UnitY*60+Vector2.UnitX*75, Color.OrangeRed);
            _spriteBatch.DrawString(font, Camera_position.ToString() + "\n" + Mouse_position.ToString() + '\n' + mapa[(int)Highlighted_province.X, (int)Highlighted_province.Y].GetID() + '\n' + mapa[(int)Highlighted_province.X, (int)Highlighted_province.Y].HasUnit() + '\n' + fps, Vector2.Zero + Vector2.UnitY * 200, Color.OrangeRed);

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
