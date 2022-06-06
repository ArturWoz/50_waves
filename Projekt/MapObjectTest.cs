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
        public override void Flip() { if (position % 2 == 0) position = position + 1; else position = position - 1; } // alternative Flip implementation to see if we can use this type of architecture
    }
}
