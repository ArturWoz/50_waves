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
            this._graphics = _graphics;
            this.rotation = true;
            this.player_position = new Vector2((float) _graphics.PreferredBackBufferWidth / 2, (float)_graphics.PreferredBackBufferHeight / 2);
        }
        public void Update(KeyboardState keystate,MouseState mouseState)
        {
            this.keystate = keystate;
            this.mousestate = mouseState;
        }
        public void move()
        {
            if (keystate.IsKeyDown(Keys.S)) //camera movement
            {
                player_position.Y = player_position.Y - speed;
            }
            if (keystate.IsKeyDown(Keys.W))
            {
                player_position.Y = player_position.Y + speed;
            }
            if (keystate.IsKeyDown(Keys.D))
            {
                player_position.X = player_position.X - speed;
                rotation = false;
            }
            if (keystate.IsKeyDown(Keys.A))
            {
                player_position.X = player_position.X + speed;
                rotation = true;
            }
        }
        public Vector2 GetPlayerPosition()
        {
            move();
            return player_position;
        }
        public bool GetRotation()
        {
            return this.rotation;
        }
    }
}
