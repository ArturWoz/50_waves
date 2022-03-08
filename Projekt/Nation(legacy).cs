using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Projekt
{
    public class Nation
    {
        byte nation_id;
        int slaves=0;
        string nation_name;
        City[] cities = new City[200];
        double nation_gold=150;
        public ArrayList Units = new ArrayList();

        public Nation(byte nation_id, string nation_name)
        {
            this.nation_id = nation_id;
            this.nation_name = nation_name;
        }
        public void AddGold(double nation_gold)
        {
            this.nation_gold +=nation_gold;
        }
        public void AddSlaves(int slaves)
        {
            this.slaves += slaves;
        }
        public City GetCity(int id) { return this.cities[id]; }
        public void SetCity(City city, int id) { this.cities[id] = city; }
        public void AddUnits(object unit) { Units.Add(unit);}

        public void RemoveUnits(object unit) { Units.Remove(unit); }
        public byte GetID()
        {
            return nation_id;
        }
    }
}
