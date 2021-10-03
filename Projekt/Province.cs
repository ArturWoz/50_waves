using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt
{
    public class Province:Tile
    {
        int province_value;
        int province_movement = 2;
        bool clicked;
        bool has_unit = false;
        City City_on_this_province = null;
        Object Building_on_this_province = null;

        public Province(int province_id, int province_owner, terrain terrain, bool is_water,
            int province_value):base(province_id, province_owner, terrain, is_water)
        {
            this.province_id = province_id;
            this.province_owner = province_owner;
            this.terrain = terrain;
            this.is_water = is_water;

        }
        public void AddCity(City City)
        {
            this.City_on_this_province = City;
        }
        public City GetCity()
        {
            return this.City_on_this_province;
        }
        public void AddBuilding(Object Building)
        {
            this.Building_on_this_province = Building;
        }
        public Object GetBuilding()
        {
            return Building_on_this_province;
        }
        public void ChangeOwnership(int province_owner)
        {
            this.province_owner = province_owner;
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
        public bool HasUnit() { return this.has_unit; }
        public void SetUnit(bool unit) { this.has_unit = unit;  }

        public void SetClicked(bool value) { this.clicked = value; }
    }
}
