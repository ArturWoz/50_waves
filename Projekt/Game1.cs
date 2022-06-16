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
        float TileXSize = 128; //province size in pixels at default zoom
        float TileYSize;
        float zoom;
        //texture declarations
        //255 textures
        Texture2D province_desert, province_farmland, province_forest, province_jungle, province_lake, province_mountains, province_plains, province_sea, province_taiga, province_tundra, province_coast, province_hills, province_highlight, province_city, province_interface, city_interface, new_building, trading_post, trading_post_province, nationInterface, turnHUD, kobold_settler, allied_ZoC_R, allied_ZoC_U, allied_ZoC_D, allied_ZoC_L, hostile_ZoC_L, hostile_ZoC_R, hostile_ZoC_U, hostile_ZoC_D;
        //player_skins
        Texture2D player_skin,player_on_water;
        //512 textures
        Texture2D castle_tp;
        //vector variables 
        Vector2 Camera_position = Vector2.Zero;
        Vector2 Player_position;
        Vector2 Player_position_on_map;
        Vector2 Mouse_position;
        Vector2 Scale;
        Vector2 Highlighted_province=new Vector2(1,1);
        Vector2 Prev_highlighted_province= new Vector2(1, 1);
        //Settler objects to allow for city placement

        Province ClickedProvince;
        Province[,] map;
        List<dynamic> MapObjects;
        SpriteFont font;
        private FrameCounter _frameCounter = new FrameCounter();
        int x, y; //Yprov will be always remembered :(
        Camera Camera = new Camera();
        Player player;
        int MapSize = 144;
        float totalTime;

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
            Vector2 Province_size = new Vector2(this.TileXSize / this.zoom, this.TileYSize / this.zoom);
            Vector2 Camera_offset = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            Vector2 Temp = Mouse_position - this.Camera_position / zoom - Camera_offset;
            Output.X = (int)(Temp.X / Province_size.X);
            Output.Y = (int)(Temp.Y / Province_size.Y);
            return Output;
        }
        protected Texture2D TerrainToTexture(Terrain SS)
        {
            if (SS == Terrain.desert) return province_desert;
            else if (SS == Terrain.sea) return province_sea;
            else if (SS == Terrain.coast) return province_coast;
            else if (SS == Terrain.lake) return province_lake;
            else if (SS == Terrain.hills) return province_hills;
            else if (SS == Terrain.taiga) return province_taiga;
            else if (SS == Terrain.tundra) return province_tundra;
            else if (SS == Terrain.plains) return province_plains;
            else if (SS == Terrain.farmland) return province_farmland;
            else if (SS == Terrain.forest) return province_forest;
            else if (SS == Terrain.mountains) return province_mountains;
            else if (SS == Terrain.jungle) return province_jungle;
            else if (SS == Terrain.city) return province_city;
            else return province_sea;
        }
        protected Texture2D MapObjectToTexture(Object SS)
        {
            if (SS == Object.castle) return castle_tp;
            else return player_skin; // solely debug purposes
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width; //window size to be the display size
            _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            Player_position = Vector2.Zero;
            MapGenerator generator = new MapGenerator();
            map = generator.Randommap(MapSize); x = MapSize; y = MapSize;
            Camera.Initialize(Camera_position, _graphics);
            player = new Player(player_skin, 100, 1, _graphics);
            MapObjects = new List<dynamic>();
            MapObject KeepingTest = new MapObject(4500, Object.castle);
            MapObjectTest KeepingTest2 = new MapObjectTest(4495, 0);
            MapObjects.Add(KeepingTest);
            MapObjects.Add(KeepingTest2);
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
            castle_tp = Content.Load<Texture2D>("castle_tp");
            province_city = Content.Load<Texture2D>("village");

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
                Player_position = player.GetPlayerPosition(MapSize,TileXSize);
            }
            Camera.Update(keystate, mousestate, Player_position);
            zoom = Camera.GetZoom();
            Scale = new Vector2(TileXSize / zoom / (float)province_desert.Width, TileXSize / zoom / (float)province_desert.Height); // adjusting scrolling to different camera positions
            TileYSize = TileXSize;
           
            //Player_skin_alghoritm
            Player_position_on_map = new Vector2((int)((Player_position.X -(0.5*TileXSize)) / (int)TileXSize), (int)((Player_position.Y - (0.5 * TileYSize)) / (int)TileYSize))*-1;
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
                map[(int)Prev_highlighted_province.X, (int)Prev_highlighted_province.Y].SetClicked(false);
                map[(int)Highlighted_province.X, (int)Highlighted_province.Y].SetClicked(true);
                Prev_highlighted_province = Highlighted_province;
            }    
            if (mousestate.RightButton == ButtonState.Pressed) //unclicking everything
            {
       
                map[(int)Prev_highlighted_province.X, (int)Prev_highlighted_province.Y].SetClicked(false);
                IsMouseVisible = true;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //background
            GraphicsDevice.Clear(Color.DarkBlue);
            _spriteBatch.Begin();
            Vector2 Camera_offset = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            bool flip = false;
            //drawing provinces 
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            totalTime+= deltaTime;
            _frameCounter.Update(deltaTime);
            int max_y = (int)((zoom * Camera_offset.X - Camera_position.X) / TileXSize) + 1;
            if (max_y > MapSize) max_y = MapSize;
            int max_x = (int)((zoom * Camera_offset.Y - Camera_position.Y) / TileYSize) + 1;
            if (max_x > MapSize) max_x = MapSize;

            int min_y = (int)((-zoom * Camera_offset.X - Camera_position.X) / TileXSize) - 1;
            if (min_y < 0) min_y = 0;
            int min_x = (int)((-zoom * Camera_offset.Y - Camera_position.Y) / TileYSize) - 1;
            if (min_x < 0) min_x = 0;

            for (int k = min_y; k < max_y; k++)
            {
                if (k % 2 == 0) flip = true;
                else flip = false;
                for (int k2 = min_x; k2 < max_x; k2++)
                {
                    Terrain SS = map[k2, k].GetTerrain();
                    Vector2 Province_offset = new Vector2(TileXSize * k, TileYSize* k2)/zoom;
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
            foreach(var mapobject in MapObjects)
            { // if objectPosition is in area visible on screen should be there, but I am not sure if checking conditions isn't more costly than just drawing not visible areas
                if ((int)totalTime % 2 == 0) mapobject.Flip();
                Vector2 objectPosition; // id of a province it should be on 
                objectPosition = ProvinceIDToMapCoordinate(mapobject.GetPosition()); // map position 
                Vector2 sizeOfVisibleProvince = new Vector2(TileXSize, TileYSize)/zoom; 
                Vector2 coordinates = objectPosition * sizeOfVisibleProvince; // id converted to map position times size of a visible province
                _spriteBatch.Draw(MapObjectToTexture(mapobject.GetObject()), position: ((Camera_position) / zoom + Camera_offset + coordinates), sourceRectangle: null, color: Color.White, rotation: 0, origin: Vector2.Zero, scale: Scale, effects: 0, layerDepth: 1);        
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
            
           
            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
            //debug_info
           // _spriteBatch.DrawString(font, deltaTime.ToString(),Vector2.One,Color.Pink);
            _spriteBatch.DrawString(font, Camera_position.ToString() + "\n" + Mouse_position.ToString() + '\n' + map[(int)Highlighted_province.X, (int)Highlighted_province.Y].GetID() + '\n' + map[(int)Prev_highlighted_province.X, (int)Prev_highlighted_province.Y].GetID() + '\n' + Player_position.ToString() + '\n' + fps, Vector2.Zero + Vector2.UnitY * 200, Color.OrangeRed);
            _spriteBatch.DrawString(font, (Player_position / zoom + Camera_offset).ToString() + '\n' + Camera.GetLocked(), Vector2.Zero, Color.Blue);
            _spriteBatch.DrawString(font, "\n"+'\n'+ map[(int)Player_position_on_map.Y,(int) Player_position_on_map.X].GetTerrain().ToString()+'\n'+player.GetTempSpeed(), Vector2.Zero, Color.Purple);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

