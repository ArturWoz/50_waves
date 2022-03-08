using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt
{
    public abstract class Building
    {
        protected double cost;
        protected building_status status;
        protected Province location;

        public Building(building_status status, Province location)
        {
            this.status = status;
            this.location = location;
        }
        public void SetStatus(building_status status)
        {
            this.status = status;
        }
        public double GetCost()
        {
            return cost;
        }

        public building_status GetStatus()
        {
            return status;
        }
    }

    public enum building_status
    { 
        working, in_progress, damaged
    }
}
