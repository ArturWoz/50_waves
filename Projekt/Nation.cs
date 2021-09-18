using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt
{
    public class Nation
    {
        int nation_id;
        string nation_name;
        bool is_player_led;
        double nation_gold;

        public Nation(int nation_id, string nation_name, bool is_player_led, double nation_gold)
        {
            this.nation_id = nation_id;
            this.nation_name = nation_name;
            this.is_player_led = is_player_led;
            this.nation_gold = nation_gold;
        }
        public void SetGold(double nation_gold)
        {
            if (nation_gold < 0) throw new Exception("[REDACTED]");
            this.nation_gold = nation_gold;
        }
    }
}
