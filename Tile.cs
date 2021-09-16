using System;
using System.Collections.Generic;
using System.Text;

public enum terrain
{
    farmland, desert, lake, sea, forest, jungle, hills, mountains, taiga, tundra, plains
}
namespace project_project_temporary_name_
{
    abstract class Tile
    {
        protected int province_id;
        protected string province_name;
        protected int province_owner = 0;
        protected terrain terrain;
        protected bool is_water;

        public Tile(int province_id, string province_name, int province_owner, terrain terrain, bool is_water)
        {
            this.province_id = province_id;
            this.province_name = province_name;
            this.province_owner = province_owner;
            this.terrain = terrain;
            this.is_water = is_water;
        }
    }
}
