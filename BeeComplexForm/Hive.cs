using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeComplexForm
{
    class Hive
    {
        //Constants
        private const int InitialBees = 6;
        private const int MaxNumberOfBees = 8;
        private const double InitialHoney = 3.2;
        private const double MaxAmountOfHoney = 15.0;
        private const double NectarHoneyRatio = .25;
        private const double MinimumHoneyForCreatingBees = 4.0;

        //Property & Variables
        private int beeCount;
        public double Honey { get; private set; }
        private Dictionary<string , Point> locations;


        public Hive()
        {
            //Startup initial Honey amount
            Honey = InitialHoney;

            //Create Hive's Locations
            InitializeLocations();

            Random random = new Random();

            for (int i = 0 ; i < InitialBees ; i++)
                AddBee(random);

        }

        public void InitializeLocations()
        {
            locations.Add("Entrance" , new Point(600 , 100));
            locations.Add("Nursery" , new Point(95 , 174));
            locations.Add("HoneyFactory" , new Point(157 , 98));
            locations.Add("Exit" , new Point(194 , 213));
        }

        public Point GetLocation(string location)
        {
            if (locations.Keys.Contains(location))
            {
                return locations[location];
            }
            else
            {
                throw new ArgumentException("Unknown Location:" + location);
            }

        }

        //Not Implemented Methods
        public bool AddHoney(double Nektar)
        {
            throw new NotImplementedException();
        }

        public bool ConsumeHoney(double amount)
        {
            throw new NotImplementedException();
        }

        public void AddBee(Random random)
        {
            throw new NotImplementedException();
        }

        public void Go()
        {
            throw new NotImplementedException();
        }



    }
}
