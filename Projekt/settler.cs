using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt
{
    class Settler:Entity
    {
       int hp=10;
       byte melee_defence=0;
        byte melee_attack=1;
        byte ranged_defence=0;
        byte attack_damage=1;
        int movement_points=4;
        byte owner_id;
        int cost=150;
        public Settler(Province position,Nation Owner) 
        {
            this.position = position;
            this.owner_id = Owner.GetID();
        }
        public Settler(Settler settler)
        {
            this.hp = 10;
            this.melee_defence = 0;
            this.melee_attack = 1;
            this.ranged_defence = 0;
            this.attack_damage = 1;
            this.movement_points = 4;
            this.cost = 150;
            this.position = settler.position;
            this.owner_id = settler.owner_id;
        }
        public void move(Settler settler,Province position, int x, int y)  // x and y are size of the map
        {
            if (movement_points >= position.GetProvince_movement())
            {
                this.position.SetUnit(false);
                this.position = position;
                this.position.SetUnit(true);
                stance = stance.moving;
                movement_points = movement_points - position.GetProvince_movement();
            }
        }
            public void CreateCity(Nation creator,int city_id,string city_name) 
        { 
            kill(creator); 
            position.SetTerrain(terrain.city);
            City City = new City(city_id, city_name, position, 1000, 100, 10, 10);
            creator.SetCity(City, city_id);
        }
    }
}
