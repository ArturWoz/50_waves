using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Projekt
{
    public class City
    {
        int city_id;
        string city_name;
        Province City_location;
        int city_population;
        ArrayList CityBuildings;
        int city_happiness;
        int city_defensiveness;
        double city_income;
        
        
        public City(int city_id, string city_name, Province City_location, int city_population, int city_happiness, int city_defensiveness, double city_income)
        {
            this.city_id = city_id;
            this.city_name = city_name;
            this.City_location = City_location;
            this.city_population = city_population;
            this.city_happiness = city_happiness;
            this.city_defensiveness = city_defensiveness;
            this.city_income = city_income;
            CityBuildings = new ArrayList();
        }
        public double GetIncome()
        {
            double income = this.city_income;
            foreach(TradingPost TradingPost in this.CityBuildings)
            {
                income += TradingPost.GetIncome();
            }
            return income;
        }
        public void ChangePopulation(int diff)
        {
            this.city_population += diff;
        }
        public ArrayList GetBuildings()
        {
            return this.CityBuildings;
        }
        public void AddBuilding(Object Building)
        {
            this.CityBuildings.Add(Building);
        }
    }
}
