
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


namespace Projekt
{
   public class Camera
    {
     private KeyboardState keystate;
     private MouseState mousestate;
     private Vector2 Camera_position;
     private Vector2 player_position;
     private float zoom;
     private float scroll;
     private bool locked;
     GraphicsDeviceManager _graphics;
        public void Initialize(Vector2 Camera_position, GraphicsDeviceManager _graphics)
        {
            zoom = 1;
            scroll = 0;
            this.Camera_position = Camera_position;
            this._graphics = _graphics;
            this.locked = false;
        }
        public void Update(KeyboardState keystate,MouseState mousestate,Vector2 player_position)
        {
            this.keystate = keystate;
            this.mousestate = mousestate;
            this.player_position = player_position; 
        }
       public float GetZoom()
        {
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
            if (mousestate.ScrollWheelValue <scroll && zoom < 8)
            {
                zoom = zoom + (float)0.25;
            }
            if (zoom < 0.5) zoom = 0.5f; //zoom cap
            if (zoom > 8) zoom = 8;
            scroll = mousestate.ScrollWheelValue;
            return zoom;
        }
        public Vector2 GetCameraPosition()
        {
            if(keystate.IsKeyDown(Keys.Space))
            {
                locked = true;
                Camera_position = player_position;
            }
            if (keystate.IsKeyDown(Keys.Tab))
            {
                locked = false;
                Camera_position = player_position;
            }
            if (!locked)
            {
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
                return Camera_position;
            }
            if (locked) Camera_position = player_position;
            return Camera_position;
        }
        public bool GetLocked()
        {
            return this.locked;
        }
    }
}
