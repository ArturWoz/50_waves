using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt
{
    public class TradingPost : Building
    {
        double income = 10;
        double cost = 150;
        float income_modifier;
        public TradingPost( building_status status, Province location):base(status,location)
        {
            this.status = status;
            this.location = location;
            if (location.GetTerrain() == Terrain.coast || location.GetTerrain() == Terrain.farmland)
            {
                income_modifier = 2f;
            }
            else if(location.GetTerrain() == Terrain.plains)
            {
                income_modifier = 1.5f;
            }
            else if(location.GetTerrain() == Terrain.desert || location.GetTerrain() == Terrain.jungle || location.GetTerrain() == Terrain.mountains || location.GetTerrain() == Terrain.tundra)
            {
                income_modifier = 0.5f;
            }
        }
        public double GetIncome()
        {
            return this.income*income_modifier;
        }

    }
}
