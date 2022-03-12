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
        //semi-global variables
        //monogame stuff
        public GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //map display
        float targetX = 128; //province size
        float targetY;
        float zoom;
        //texture declarations
        Texture2D province_desert, province_farmland, province_forest, province_jungle, province_lake, province_mountains, province_plains, province_sea, province_taiga, province_tundra, province_coast, province_hills, province_highlight, province_city, province_interface, city_interface, new_building, trading_post, trading_post_province, nationInterface, turnHUD, kobold_settler, allied_ZoC_R, allied_ZoC_U, allied_ZoC_D, allied_ZoC_L, hostile_ZoC_L, hostile_ZoC_R, hostile_ZoC_U, hostile_ZoC_D;
        Texture2D player_skin,player_on_water;
        //vector variables 
        Vector2 Camera_position = Vector2.Zero;
        Vector2 Player_position;
        Vector2 Player_position_on_map;
        Vector2 Mouse_position;
        Vector2 Scale;
        Vector2 Highlighted_province=new Vector2(1,1);
        Vector2 Prev_highlighted_province= new Vector2(1, 1);
        //Settler objects to allow for city placement
        Province Debug; //used to initialize the settler, this province is far beyond the map
        Settler PrevClickedSettler;
      
        //bools used to determine what menu to display/what actions to allow
        bool is_province_clicked = false;
        
        Province ClickedProvince;
        Province[,] map;
        SpriteFont font;
        private FrameCounter _frameCounter = new FrameCounter();
        int x, y; //Yprov will be always remembered :(
                                     //faction declaration (TODO: other factions)
        Camera Camera = new Camera();
        Player player;
        Nation Kobold = new Nation(1, "Kobolds");
        bool check_unit = true;
        
        
        int MapSize = 144;

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
            _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width; //window size to be the display size
            _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();

            Player_position = Vector2.Zero;

            MapGenerator generator = new MapGenerator();
            map = generator.Randommap(MapSize); x = MapSize; y = MapSize;
            Province spawnpoint = map[MapSize / 2, MapSize / 2]; //making the settler
            Debug = new Province(99999, 999, terrain.sea, false);
            Camera.Initialize(Camera_position, _graphics);
            player = new Player(player_skin, 100, 1, _graphics);

            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice); //loading graphics
            font = Content.Load<SpriteFont>("defaultFont");
            province_desert = Content.Load<Texture2D>("desert2");
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
            province_interface = Content.Load<Texture2D>("provInterfacePlaceholder");
            player_skin = Content.Load<Texture2D>("player_skin");
            player_on_water = Content.Load<Texture2D>("player_boat");
            /*
            nationInterface = Content.Load<Texture2D>("countryHUD");
            turnHUD = Content.Load<Texture2D>("turnHUD");
            kobold_settler = Content.Load<Texture2D>("kobold_osadnik_papież");
            province_city = Content.Load<Texture2D>("village");
            city_interface = Content.Load<Texture2D>("city_interface_2");
            trading_post = Content.Load<Texture2D>("trading_post");
            new_building = Content.Load<Texture2D>("new_building");
            trading_post_province = Content.Load<Texture2D>("trading_post_province");
            allied_ZoC_D = Content.Load<Texture2D>("allied_ZoC_down");
            allied_ZoC_R = Content.Load<Texture2D>("allied_ZoC_right");
            allied_ZoC_U = Content.Load<Texture2D>("allied_ZoC_up");
            allied_ZoC_L = Content.Load<Texture2D>("allied_ZoC_left");
            hostile_ZoC_D = Content.Load<Texture2D>("hostile_ZoC_down");
            hostile_ZoC_R = Content.Load<Texture2D>("hostile_ZoC_right");
            hostile_ZoC_U = Content.Load<Texture2D>("hostile_ZoC_up");
            hostile_ZoC_L = Content.Load<Texture2D>("hostile_ZoC_left"); 
            */
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keystate = Keyboard.GetState();
            MouseState mousestate = Mouse.GetState();
            
            //Camera && Player_camera logics
            player.Update(keystate, mousestate); 
            Camera_position = Camera.GetCameraPosition();
            if (Camera.GetLocked())
            {
                Player_position = player.GetPlayerPosition(MapSize,targetX);
            }
            Camera.Update(keystate, mousestate, Player_position);
            zoom = Camera.GetZoom();
            Scale = new Vector2(targetX / zoom / (float)province_desert.Width, targetX / zoom / (float)province_desert.Height); // adjusting scrolling to different camera positions
            targetY = targetX;
           
            //Player_skin_alghoritm
            Player_position_on_map = new Vector2((int)((Player_position.X -(0.5*targetX)) / (int)targetX), (int)((Player_position.Y - (0.5 * targetY)) / (int)targetY))*-1;
            if (map[(int)Player_position_on_map.Y, (int)Player_position_on_map.X].GetTerrain().ToString() == "sea") { player.SetSkin(player_on_water); }
            else player.SetSkin(player_skin);
      
            player.SetTempSpeed(map[(int)Player_position_on_map.Y, (int)Player_position_on_map.X]);

            int Xpos, Ypos; //reading mouse position
            Xpos = Mouse.GetState().Position.X;
            Ypos = Mouse.GetState().Position.Y;
            Mouse_position = new Vector2(Xpos, Ypos);

            Vector2 Test = MouseToMapCoordinate(Mouse_position); // testing if mouse is inside the map

            // reading map coordinates based on current mouse position
            if ((int)Test.X > -1 && (int)Test.Y > -1 && (int)Test.X < x && (int)Test.Y < y) // x and y are map dimensions; determining which province is selected
            {
                Highlighted_province = MouseToMapCoordinate(Mouse_position);
            }
            if (mousestate.LeftButton==ButtonState.Pressed)
            {
                is_province_clicked = true;
                map[(int)Prev_highlighted_province.X, (int)Prev_highlighted_province.Y].SetClicked(false);
                map[(int)Highlighted_province.X, (int)Highlighted_province.Y].SetClicked(true);
                Prev_highlighted_province = Highlighted_province;
            }    
         
            if (mousestate.RightButton == ButtonState.Pressed) //unclicking everything
            {
                is_province_clicked = false;
                map[(int)Prev_highlighted_province.X, (int)Prev_highlighted_province.Y].SetClicked(false);
                IsMouseVisible = true;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //background
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            Vector2 Camera_offset = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            bool flip = false;
            //drawing provinces 

            int mx_y = (int)((zoom * Camera_offset.X - Camera_position.X) / targetX) + 1;
            if (mx_y > MapSize) mx_y = MapSize;
            int mx_x = (int)((zoom * Camera_offset.Y - Camera_position.Y) / targetY) + 1;
            if (mx_x > MapSize) mx_x = MapSize;

            int mn_y = (int)((-zoom * Camera_offset.X - Camera_position.X) / targetX) - 1;
            if (mn_y < 0) mn_y = 0;
            int mn_x = (int)((-zoom * Camera_offset.Y - Camera_position.Y) / targetY) - 1;
            if (mn_x < 0) mn_x = 0;

            for (int k = mn_y; k < mx_y; k++)
            {
                if (k % 2 == 0) flip = true;
                else flip = false;
                for (int k2 = mn_x; k2 < mx_x; k2++)
                {
                    terrain SS = map[k2, k].GetTerrain();
                    Vector2 Province_offset = new Vector2(targetX / zoom * k, targetY / zoom * k2);
                    if (flip)
                    {
                        _spriteBatch.Draw(TerrainToTexture(SS), position: ((Camera_position) / zoom + Camera_offset) + Province_offset, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0);
                    }
                    else
                    {
                        _spriteBatch.Draw(TerrainToTexture(SS), position: ((Camera_position) / zoom + Camera_offset) + Province_offset, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
                    }
                    if (map[k, k2].GetClicked()) _spriteBatch.Draw(province_highlight, position: ((Camera_position) / zoom + Camera_offset) + Province_offset, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
                }
            }
            if (!player.GetRotation())
            {
                _spriteBatch.Draw(player.GetSkin(), position: ((Camera_position) / zoom) + Camera_offset - Player_position / zoom, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            }
            else
            {
                _spriteBatch.Draw(player.GetSkin(), position: ((Camera_position) / zoom) + Camera_offset - Player_position / zoom, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0);
            }
            //show camera position and province id and FPS
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _frameCounter.Update(deltaTime);
            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
            //debug_info
            _spriteBatch.DrawString(font, Camera_position.ToString() + "\n" + Mouse_position.ToString() + '\n' + map[(int)Highlighted_province.X, (int)Highlighted_province.Y].GetID() + '\n' + map[(int)Prev_highlighted_province.X, (int)Prev_highlighted_province.Y].GetID() + '\n' + Player_position.ToString() + '\n' + fps, Vector2.Zero + Vector2.UnitY * 200, Color.OrangeRed);
            _spriteBatch.DrawString(font, (Player_position / zoom + Camera_offset).ToString() + '\n' + Camera.GetLocked(), Vector2.Zero, Color.Blue);
            _spriteBatch.DrawString(font, "\n"+'\n'+ map[(int)Player_position_on_map.Y,(int) Player_position_on_map.X].GetTerrain().ToString()+'\n'+player.GetTempSpeed(), Vector2.Zero, Color.Purple);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

