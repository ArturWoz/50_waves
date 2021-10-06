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
        public ProvinceZoC(Province Province) : base(Province)
        {
            this.province_id = Province.GetID();
            this.province_owner = Province.GetOwner();
            this.terrain = Province.GetTerrain();
            this.is_water = Province.IsWater();
        }
      public void AddBuilding(object Building) { if(this.Building.Count==0) this.Building.Add(Building); }
      public void RemoveBuilding() { if(this.Building.Count>0) this.Building.RemoveAt(0); }
      public void SetZoCU(bool U) { this.U = U; }
        public void SetZoCD(bool D) { this.D = D; }
        public void SetZoCL(bool L) { this.L = L; }
        public void SetZoCR(bool R) { this.R = R; }
        public bool[] GetZoC() 
        {
            bool[] list = new bool[4];
            list[0]= this.U;
            list[1] = this.D;
            list[2] = this.L;
            list[3] = this.R;
            return list;
        }
    }
}
