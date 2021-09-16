using System;
using System.Collections.Generic;
using System.Text;

namespace project_project_temporary_name_
{
    class Wasteland:Tile
    {
        public Wasteland(int province_id, string province_name, int province_owner, terrain terrain, bool is_water):base(province_id, province_name, province_owner, terrain, is_water)
        {
            this.province_id = province_id;
            this.province_name = province_name;
            this.province_owner = province_owner;
            this.terrain = terrain;
            this.is_water = is_water;
        }
        public bool Equals(Wasteland wasteland)
        {
            return wasteland.province_id == this.province_id;
        }
    }
}
