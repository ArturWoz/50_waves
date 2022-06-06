using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt
{
   public class MapObject
    {
        protected int position; //id of tile it is on 
        protected Object Object; //texture
       public MapObject(int position, Object Object)
        {
            this.position = position;
            this.Object = Object;
        }
        public Object GetObject() { return this.Object; }
        public int GetPosition() { return this.position; }
    }
}
