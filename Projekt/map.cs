using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace Projekt
{
    class map
    {
        int map_id;
        public ArrayList Provinces = new ArrayList();
        public ArrayList Wastelands = new ArrayList();
        public map(int map_id, ArrayList provinces, ArrayList wastelands)
        {
            this.map_id = map_id;
            Provinces = provinces;
            Wastelands = wastelands;
        }
    }
}
