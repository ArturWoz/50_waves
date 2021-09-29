﻿using System;
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
        protected Province position;
        protected stance stance = stance.idle;
        protected status status = status.alive;
        protected int hp;
        protected byte melee_defence;
        protected byte melee_attack;
        protected byte ranged_defence;
        protected byte attack_damage;
        protected int movement_points;
        protected byte owner_id;
        protected int cost;
       public void move(Province position) 
        {
            if (movement_points >= position.GetProvince_movement())
            {
                this.position = position;
                stance = stance.moving;
                movement_points = movement_points - position.GetProvince_movement();
            }
        }
        public void kill(Nation killer) { position = null;stance = stance.idle;status = status.dead;hp = 0;melee_defence = 0;melee_attack = 0;ranged_defence = 0;movement_points = 0; if (killer.GetID() != owner_id) killer.AddSlaves(cost/2); }
       public int GetCost() { return this.cost; }
       public Province GetPosition() { return this.position; }
       public void cancel_stance() { this.stance = stance.idle; }
    }
}
