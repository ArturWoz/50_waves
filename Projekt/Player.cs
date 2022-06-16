using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Projekt
{
    class Player
    {
        Texture2D skin;
        int hp;
        int speed;
        int tempspeed;
        Vector2 player_position;
        GraphicsDeviceManager _graphics;
        KeyboardState keystate;
        MouseState mousestate;
        bool rotation; // true = Right, false = Left
        public Player(Texture2D skin,int hp, int speed,GraphicsDeviceManager _graphics)
        {
            this.skin = skin;
            this.hp = hp;
            this.speed = speed;
            this.tempspeed = speed;
            this._graphics = _graphics;
            this.rotation = true;
            this.player_position =  Vector2.Zero;
        }
        public void Update(KeyboardState keystate,MouseState mouseState)
        {
            this.keystate = keystate;
            this.mousestate = mouseState;
        }
        public void SetSkin(Texture2D skin)
        {
            this.skin = skin;
        }
        public Texture2D GetSkin()
        {
            return this.skin;
        }
        public void SetTempSpeed(Province where_player_is)
        {
            tempspeed = speed * where_player_is.getProvinceMovement();
        }
        public int GetTempSpeed() { return this.tempspeed; }
        public void move()
        {
            if (keystate.IsKeyDown(Keys.S)) //camera movement
            {
                player_position.Y = player_position.Y - tempspeed;
            }
            if (keystate.IsKeyDown(Keys.W))
            {
                player_position.Y = player_position.Y + tempspeed;
            }
            if (keystate.IsKeyDown(Keys.D))
            {
                player_position.X = player_position.X - tempspeed;
                rotation = false;
            }
            if (keystate.IsKeyDown(Keys.A))
            {
                player_position.X = player_position.X + tempspeed;
                rotation = true;
            }
        }
        public Vector2 GetPlayerPosition(int MapSize,float Scale)
        {
            move();
            if (player_position.X > 0) player_position.X = 0;
            if (player_position.Y > 0) player_position.Y = 0;
            if (player_position.X < -1 * MapSize * Scale + Scale) player_position.X = -1 * MapSize * Scale + Scale;
            if (player_position.Y < -1 * MapSize * Scale + Scale) player_position.Y = -1 * MapSize * Scale + Scale;
            return player_position;
        }
        public bool GetRotation()
        {
            return this.rotation;
        }
    }
}
