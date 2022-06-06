using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt
{
    public class MapObjectTest : MapObject
    {
        int newdata=5;
        public MapObjectTest(int position, Object Object) : base(position,Object)
        {
            this.position = position;
            this.Object = Object;
        }
    }
}
