using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt
{
    public class Nation
    {
        byte nation_id;
        int slaves=0;
        string nation_name;
        City[] cities;
        bool is_player_led;
        double nation_gold;

        public Nation(byte nation_id, string nation_name, bool is_player_led, double nation_gold)
        {
            this.nation_id = nation_id;
            this.nation_name = nation_name;
            this.is_player_led = is_player_led;
            this.nation_gold = nation_gold;
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
        public byte GetID()
        {
            return nation_id;
        }
    }
}
