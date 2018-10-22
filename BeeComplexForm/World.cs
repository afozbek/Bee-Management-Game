using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeComplexForm
{
    class World
    {
        //Constants
        private const double NectarHarvestedPerNewFlower = 50.0;
        private const int FieldMinX = 15;
        private const int FieldMinY = 177;
        private const int FieldMaxX = 690;
        private const int FieldMaxY = 290;

        //Keep track of List of Bees,Flowers && Hive
        public Hive Hive;
        public List<Bee> Bees;
        public List<Flower> Flowers;

        public World()
        {
            Flowers = new List<Flower>();
            Bees = new List<Bee>();
            Hive = new Hive(this);
            Random random = new Random();
            for (int i = 0 ; i < 10 ; i++)
                AddFlower(random);
        }

        public void Go(Random random)
        {
            Hive.Go(random);
            for (int i = Bees.Count - 1 ; i >= 0 ; i--)
            {
                Bee bee = Bees[i];
                bee.Go(random);
                if (bee.CurrentState == Bee.BeeState.Retired)
                    Bees.Remove(bee);
            }

            double totalNectarHarvested = 0;
            for (int i = Flowers.Count - 1 ; i >= 0 ; i--)
            {
                Flower flower = Flowers[i];
                flower.Go();
                totalNectarHarvested += flower.NectarHarvested;
                if (!flower.Alive)
                    Flowers.Remove(flower);
            }

            if (totalNectarHarvested > NectarHarvestedPerNewFlower)
            {
                foreach (Flower flower in Flowers)
                    flower.NectarHarvested = 0;
                AddFlower(random);
            }
        }

        private void AddFlower(Random random)
        {
            Point location = new Point(random.Next(FieldMinX , FieldMaxX) ,
                random.Next(FieldMinY , FieldMaxY));
            Flower newFlower = new Flower(location , random);

            Flowers.Add(newFlower);
        }
    }
}