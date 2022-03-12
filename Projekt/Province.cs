using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt
{
    public class Province
    {
        int province_movement = 1;
        bool clicked;
        protected int province_id;
        protected terrain terrain;

        public Province(int province_id, int province_owner, terrain terrain, bool is_water)
        {
            this.province_id = province_id;
            this.terrain = terrain;
            TerrainToMovementSpeed(terrain);
        }
        public void TerrainToMovementSpeed(terrain terrain)
        {
            if (terrain == terrain.farmland  || terrain == terrain.coast || terrain == terrain.city || terrain == terrain.plains) province_movement = 10;
            else if (terrain == terrain.forest || terrain == terrain.hills || terrain == terrain.tundra || terrain == terrain.jungle || terrain == terrain.taiga || terrain == terrain.desert) province_movement = 5;
            else if (terrain == terrain.lake || terrain == terrain.sea) province_movement = 15;
            else province_movement = 1;
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
        public void SetTerrain(terrain terrain) { this.terrain = terrain; TerrainToMovementSpeed(terrain); }
        public int GetID() { return this.province_id; }

        public bool GetClicked() { return this.clicked; } //LMAO get clicked n00b
        public int GetProvince_movement() { return this.province_movement; }

        public void SetClicked(bool value) { this.clicked = value; }
    }
}
