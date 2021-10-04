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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
            //map display
        float targetX = 128; //province size
        float targetY;
        float zoom = 1;
            //texture declarations
        Texture2D province_desert, province_farmland, province_forest, province_jungle, province_lake, province_mountains, province_plains, province_sea, province_taiga, province_tundra, province_coast, province_hills,province_highlight,province_city, province_interface, city_interface, new_building, trading_post,trading_post_province,  nationInterface,turnHUD,kobold_settler, allied_ZoC_R, allied_ZoC_U, allied_ZoC_D, allied_ZoC_L, hostile_ZoC_L, hostile_ZoC_R, hostile_ZoC_U, hostile_ZoC_D;
            //vector variables 
        Vector2 Camera_position = Vector2.Zero;
        Vector2 Mouse_position;
        Vector2 Scale;
        Vector2 Highlighted_province;
        Vector2 Prev_highlighted_province;
        Vector2 City_interface_position;
        Vector2 City_building_slots_origin_coordinates;
            //Settler objects to allow for city placement
        Settler ClickedSettler;
        Province Debug; //used to initialize the settler, this province is far beyond the map
        Settler PrevClickedSettler;
            //bools used to determine what menu to display/what actions to allow
        bool global_clicked_province = false,global_clicked_unit=false;
        bool global_clicked_province_is_city = false;
        bool building_to_be_made = false;
        Province ClickedProvince;
        City MostRecentlyClickedCity;
        //if_interfaces_open
        bool city_interface_open = false;
        string path = "map.txt";
        string s; //string used for loading map files 
        Province[,] map;
        terrain s2; //string s converted to type terrain
        SpriteFont font;
        private FrameCounter _frameCounter = new FrameCounter();
        int i = 0, x, y, scroll = 0; //Yprov will be always remembered :(
            //faction declaration (TODO: other factions)
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
            _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width; //window size to be the display size
            _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            StreamReader sr = File.OpenText(path);
            x = int.Parse(sr.ReadLine()); //map dimensions
            y = int.Parse(sr.ReadLine());
            map = new Province[x, y];

            for (int k = 0; k < y; k++) //loading map file
            {
                for (int k2 = 0; k2 < x; k2++)
                {
                    s = sr.ReadLine();
                    s2 = (terrain)Enum.Parse(typeof(terrain), s, true);
                    Province load = new Province(i, 0, s2, false, 0);
                    map[k2, k] = load;
                    i++;
                }
            }
            sr.Close();
            Province spawnpoint = map[15, 15]; //making the settler
            Settler kobold_settler=new Settler(spawnpoint,Kobold);
            Kobold.AddUnits(kobold_settler);
            Debug = new Province(99999, 999, terrain.sea, false, 0);
            PrevClickedSettler = new Settler(Debug, Kobold);

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
            province_interface = Content.Load<Texture2D>("provInterfacePlaceholder");
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
            if (mousestate.ScrollWheelValue > scroll && zoom > 0.5) //mouse zoom
            {
                zoom = zoom - (float)0.25;
            }
            if (mousestate.ScrollWheelValue < scroll && zoom < 8)
            {
                zoom = zoom + (float)0.25;
            }
            if (zoom < 0.5) zoom = 0.5f; //zoom cap
            if (zoom > 8) zoom = 8;

            if (keystate.IsKeyDown(Keys.Space)) //settling a city
            {
                if (PrevClickedSettler.GetClicked())
                {
                    PrevClickedSettler.CreateCity(Kobold, 1, "Nirwana");
                    Kobold.RemoveUnits(PrevClickedSettler);
                }
            }
            Scale = new Vector2(targetX / zoom / (float)province_desert.Width, targetX / zoom / (float)province_desert.Height); // adjusting scrolling to different camera positions
            targetY = targetX;
            scroll = mousestate.ScrollWheelValue;

            int Xpos, Ypos; //reading mouse position
            Xpos = Mouse.GetState().Position.X;
            Ypos = Mouse.GetState().Position.Y;
            Mouse_position = new Vector2(Xpos, Ypos);
            Vector2 Test = MouseToMapCoordinate(Mouse_position);
            //checking if mouse is on the city interface (comment added to obscure the fact, that this is total spaghetti)
            if (city_interface_open && Mouse_position.X < City_interface_position.X + city_interface.Width && Mouse_position.Y > City_interface_position.Y && mousestate.LeftButton == ButtonState.Pressed)
            {
                int number_of_taken_building_slots = ClickedProvince.GetCity().GetBuildings().Count;

                if (Mouse_position.X < City_building_slots_origin_coordinates.X + ((number_of_taken_building_slots % 3) + 1) * new_building.Width)
                    if (Mouse_position.X > City_building_slots_origin_coordinates.X + (number_of_taken_building_slots % 3) * new_building.Width)
                        if (Mouse_position.Y > City_building_slots_origin_coordinates.Y + (number_of_taken_building_slots / 3) * new_building.Width)
                            if (Mouse_position.Y < City_building_slots_origin_coordinates.Y + ((number_of_taken_building_slots / 3) + 1) * new_building.Width)
                            {
                                building_to_be_made = true;
                                IsMouseVisible = false;
                            }
            }
            else
            {
                if ((int)Test.X > -1 && (int)Test.Y > -1 && (int)Test.X < x && (int)Test.Y < y) // x and y are map dimensions; determining which province is selected
                {
                    Highlighted_province = MouseToMapCoordinate(Mouse_position);
                }
                if (mousestate.LeftButton == ButtonState.Pressed)
                {
                    if (map[(int)Highlighted_province.X, (int)Highlighted_province.Y].HasUnit() != false) //if a unit has been selected
                    {
                        foreach (Settler settler in Kobold.Units)
                        {
                            if (settler.GetPosition().GetID() == Highlighted_province.X * x + Highlighted_province.Y * 1)
                            {
                                global_clicked_province = false;
                                global_clicked_province_is_city = false;
                                global_clicked_unit = true;
                                ClickedSettler = settler;
                                PrevClickedSettler.SetClicked(false);
                                ClickedSettler.SetClicked(true);
                                PrevClickedSettler = ClickedSettler;
                                map[(int)Prev_highlighted_province.X, (int)Prev_highlighted_province.Y].SetClicked(false);
                                break;
                            }
                        }
                    }
                    else //if a province has been selected
                    {
                        global_clicked_province = true;
                        global_clicked_unit = false;
                        ClickedProvince = map[(int)Highlighted_province.X, (int)Highlighted_province.Y];
                        map[(int)Prev_highlighted_province.X, (int)Prev_highlighted_province.Y].SetClicked(false);
                        map[(int)Highlighted_province.X, (int)Highlighted_province.Y].SetClicked(true);
                        PrevClickedSettler.SetClicked(false);
                        Prev_highlighted_province = Highlighted_province;
                        if (map[(int)Highlighted_province.X, (int)Highlighted_province.Y].GetTerrain() == terrain.city) // if the province is a city
                        {
                            global_clicked_province_is_city = true;
                            MostRecentlyClickedCity = ClickedProvince.GetCity();
                        }
                        else
                        {
                            global_clicked_province_is_city = false;
                            if(building_to_be_made && ClickedProvince.GetBuilding()==null)
                            {
                                TradingPost TradingPost = new TradingPost(building_status.working, ClickedProvince);
                                MostRecentlyClickedCity.AddBuilding(TradingPost);
                                ClickedProvince.AddBuilding(TradingPost);
                                building_to_be_made = false;
                                IsMouseVisible = true;
                            }
                        }
                    }
                }
            }
            if (mousestate.RightButton == ButtonState.Pressed) //unclicking everything
            {
                global_clicked_province = false;
                global_clicked_unit = false;
                map[(int)Prev_highlighted_province.X, (int)Prev_highlighted_province.Y].SetClicked(false);
                PrevClickedSettler.SetClicked(false);
                building_to_be_made = false;
                IsMouseVisible = true;
            }

            city_interface_open = global_clicked_province && global_clicked_province_is_city;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //background
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            Vector2 Camera_offset = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            //drawing provinces 
            for (int k = 0; k < y; k++) 
            {

                for (int k2 = 0; k2 < x; k2++)
                {
                    terrain SS = map[k2, k].GetTerrain();
                    Vector2 Province_offset = new Vector2(targetX / zoom * k, targetY / zoom * k2);
                    _spriteBatch.Draw(TerrainToTexture(SS), position: ((Camera_position) / zoom + Camera_offset) + Province_offset, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
                    if(map[k, k2].GetBuilding()!=null)
                        if(map[k,k2].GetBuilding().GetType()==typeof(TradingPost))
                        {
                            _spriteBatch.Draw(trading_post_province, position: ((Camera_position) / zoom + Camera_offset) + Province_offset, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
                        }
                    if (map[k, k2].GetClicked()) _spriteBatch.Draw(province_highlight, position: ((Camera_position) / zoom + Camera_offset) + Province_offset, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);

                }
            }
            //drawing units
            foreach (Settler Settler in Kobold.Units)
            {
                Province settler_positionProvince = Settler.GetPosition();
                Vector2 settler_positionID = ProvinceIDToMapCoordinate(settler_positionProvince.GetID());
                Vector2 unit_offset = new Vector2(targetX / zoom * settler_positionID.X, targetY / zoom * settler_positionID.Y);
                Settler.GetPosition().SetUnit(true);
                _spriteBatch.Draw(kobold_settler, position: ((Camera_position) / zoom + Camera_offset) + unit_offset, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
                if(Settler.GetClicked()==true) { _spriteBatch.Draw(province_highlight, position: ((Camera_position) / zoom + Camera_offset) + unit_offset, null, Color.White, 0, Vector2.Zero, Scale, 0, 0); }
            }
            //draw city interface
            if(global_clicked_province_is_city && global_clicked_province)
            {
                City_interface_position = new Vector2(0, _graphics.PreferredBackBufferHeight - city_interface.Height);
                _spriteBatch.Draw(city_interface, position: City_interface_position, null, Color.White, 0, Vector2.Zero, 1, 0, 0);
                // Drawing an array of building slots
                float fraction_of_interfaces_height = (float)1 / 15;
                float fraction_of_interfaces_width = (float)1 / 3;
                Vector2 building_slot_position = new Vector2(city_interface.Width * fraction_of_interfaces_width, _graphics.PreferredBackBufferHeight - city_interface.Height + fraction_of_interfaces_height * city_interface.Height); //Git gud
                int k = 0;
                City_building_slots_origin_coordinates = building_slot_position;
                if (ClickedProvince.GetCity().GetBuildings()!=null)
                {
                    foreach (TradingPost tradingPost in ClickedProvince.GetCity().GetBuildings())
                    {
                        _spriteBatch.Draw(trading_post, position: building_slot_position, null, Color.White, 0, Vector2.Zero, 1, 0, 0);
                        
                        if (k > 1)
                        {
                            building_slot_position.Y += trading_post.Height;
                            building_slot_position.X -= trading_post.Width*2;
                            k = 0;
                        }
                        else
                        {
                            building_slot_position.X += trading_post.Width;
                            k++;
                        }
                    }
                }
                _spriteBatch.Draw(new_building, position: building_slot_position, null, Color.White, 0, Vector2.Zero, 1, 0, 0);

            }
            //show camera position and province id and FPS
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _frameCounter.Update(deltaTime);
            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
            Vector2 interface_position = new Vector2(0, _graphics.PreferredBackBufferHeight - province_interface.Height);
            Vector2 HUD_position = new Vector2(_graphics.PreferredBackBufferWidth - 350, 0);
            Vector2 interface_offset1 = new Vector2(100, 35);
            Vector2 interface_offset2 = new Vector2(150, 200);
            //draw province interface
            if (global_clicked_province && !global_clicked_province_is_city)
            {
                _spriteBatch.Draw(province_interface, position: interface_position, null, Color.White, 0, Vector2.Zero, 1, 0, 0);
                _spriteBatch.DrawString(font, "type" + " " + map[(int)Prev_highlighted_province.Y, (int)Prev_highlighted_province.X].GetTerrain().ToString(), interface_position + interface_offset1, Color.OrangeRed);
                _spriteBatch.DrawString(font, "Movement Cost" + " " + map[(int)Prev_highlighted_province.Y, (int)Prev_highlighted_province.X].GetProvince_movement().ToString(), interface_position + interface_offset2, Color.OrangeRed);

            }
            if(building_to_be_made) // building icon should follow mouse until it is placed
            {
                _spriteBatch.Draw(trading_post, position: Mouse_position, null, Color.White, 0, Vector2.Zero, 1/zoom, 0, 0);
            }
            //drawing nation interface
            _spriteBatch.Draw(nationInterface, position: Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, 0, 0);
            _spriteBatch.Draw(turnHUD, position: HUD_position,null, Color.White, 0, Vector2.Zero, 1, 0, 0);
            _spriteBatch.DrawString(font, "100", Vector2.Zero + Vector2.UnitY * 40 + Vector2.UnitX * 50, Color.OrangeRed);
            _spriteBatch.DrawString(font, "696969", Vector2.Zero + Vector2.UnitY * 40 + Vector2.UnitX * 350, Color.OrangeRed);
            _spriteBatch.DrawString(font, "420", Vector2.Zero + Vector2.UnitY * 40 + Vector2.UnitX * 650, Color.OrangeRed);
            _spriteBatch.DrawString(font, "0", HUD_position+Vector2.UnitY*60+Vector2.UnitX*75, Color.OrangeRed);
            _spriteBatch.DrawString(font, Camera_position.ToString() + "\n" + Mouse_position.ToString() + '\n' + map[(int)Highlighted_province.X, (int)Highlighted_province.Y].GetID() + '\n' + map[(int)Highlighted_province.X, (int)Highlighted_province.Y].HasUnit() + '\n' + fps, Vector2.Zero + Vector2.UnitY * 200, Color.OrangeRed);
            if (ClickedProvince != null && ClickedProvince.GetBuilding() != null)
            {  
                _spriteBatch.DrawString(font, ","+'\n' + '\n' + '\n' + '\n' + '\n' + '\n' + '\n' + '\n' + '\n' + '\n' + ClickedProvince.GetBuilding().ToString(), Vector2.Zero , Color.OrangeRed);
                //TradingPost post = ClickedProvince.GetBuilding();
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
