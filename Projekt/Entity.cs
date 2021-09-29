using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt
{
    public enum stance
    {
        idle, moving, fighting
    }
    public enum status
    {
        dead, alive
    }
    abstract class Entity
    {
        Province position;
        stance stance = stance.idle;
        status status = status.alive;
        int hp;
        byte melee_defence;
        byte melee_attack;
        byte ranged_defence;
        byte attack_damage;
        int movement_points;
        byte owner_id;
        int cost;
        void move(Province position) 
        {
            if (movement_points >= position.GetProvince_movement())
            {
                this.position = position;
                stance = stance.moving;
                movement_points = movement_points - position.GetProvince_movement();
            }
        }
        void kill(Nation killer) { position = null;stance = stance.idle;status = status.dead;hp = 0;melee_defence = 0;melee_attack = 0;ranged_defence = 0;movement_points = 0; if (killer.GetID() != owner_id) killer.AddSlaves(cost/2); }
        int GetCost() { return this.cost; }
        void cancel_stance() { this.stance = stance.idle; }
    }
}
