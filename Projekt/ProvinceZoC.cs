using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Projekt
{
    class ProvinceZoC:Province
    {
        bool U;
        bool D;
        bool L;
        bool R;
        ArrayList Building = new ArrayList();
        public ProvinceZoC(int province_id, int province_owner, terrain terrain, bool is_water):base(province_id,province_owner,terrain,is_water)
        {
            this.province_id = province_id;
            this.province_owner = province_owner;
            this.terrain = terrain;
            this.is_water = is_water;
        }

    }
}
