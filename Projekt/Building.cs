using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt
{
    class Building
    {
        double income;
        double cost;
        building_status status;
        building_type type;
        bool active;

        Building(double income, double cost, building_status status, building_type type)
        {
            this.income = income;
            this.cost = cost;
            this.status = status;
            this.type = type;
            active = true;
        }
        public void DestroyBuilding()
        {
            active = false;
        }

        public double GetIncome()
        {
            return income;
        }

        public double GetCost()
        {
            return cost;
        }

        public building_status GetStatus()
        {
            return status;
        }

        public building_type GetBuildingType()
        {
            return type;
        }
        public bool GetActive()
        {
            return active;
        }
    }

    public enum building_status
    { 
        working, in_progress, damaged
    }

    public enum building_type
    {
        succub_centre,mine,pits,tax_office,fort,barracs,harbour
    }
}
