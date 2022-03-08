using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt
{
    public class Province
    {
        int province_movement = 2;
        bool clicked;
        protected int province_id;
        protected terrain terrain;

        public Province(int province_id, int province_owner, terrain terrain, bool is_water)
        {
            this.province_id = province_id;
            this.terrain = terrain;
        }
        public Province(Province Province)
        {
            this.province_id = Province.province_id;
            this.terrain = Province.terrain;
            this.clicked = Province.clicked;
            this.province_movement = Province.province_movement;
        }
     
        public bool Equals(Province province)
        {
            return province.province_id == this.province_id;
        }
        public terrain GetTerrain() { return this.terrain; }
        public void SetTerrain(terrain terrain) { this.terrain = terrain; }
        public int GetID() { return this.province_id; }

        public bool GetClicked() { return this.clicked; } //LMAO get clicked n00b
        public int GetProvince_movement() { return this.province_movement; }

        public void SetClicked(bool value) { this.clicked = value; }
    }
}
