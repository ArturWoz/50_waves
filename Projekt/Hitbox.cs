using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projekt
{
    public class Hitbox
    {
        private Texture2D texture;
        private int x_size;
        private int y_size;
        private int x_position;
        private int y_position;
        private bool collision = false;
        public Hitbox(Texture2D texture,int x_position, int y_position)
        {
            this.texture = texture;
            this.x_size = texture.Width;
            this.y_size = texture.Height;
            this.x_position = x_position;
            this.y_position = y_position;
        }
        public Texture2D Get_Texture() { return this.texture; }
        public void Close() { this.collision = false; }
        public void Open() { this.collision = true; }
        public void Detect_Collision(Vector2 mouse_position,MouseState mousestate) 
        {
            if (mousestate.LeftButton == ButtonState.Pressed && mouse_position.X >= x_position && mouse_position.X <= x_position + x_size && mouse_position.Y >= y_position && mouse_position.Y <= y_position + y_size) 
            {
                collision = true;
            }
            if (mousestate.RightButton == ButtonState.Pressed)
            {
                collision = false;
            }
        }
        public bool Get_Collision() { return this.collision; }
    }
}
