using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeComplexForm
{
    class Flower
    {
        //Constants
        private const int LifeSpanMin = 15000;
        private const int LifeSpanMax = 30000;
        private const double InitialNectar = 1.5;
        private const double MaxNectar = 5.0;
        private const double NectarAddedPerTurn = 0.01;
        private const double NectarGatheredPerTurn = 0.3;


        public Point Location { get; private set; }
        public int Age { get; private set; }
        public bool Alive { get; private set; }
        public double Nectar { get; private set; }
        public double NectarHarvested { get; set; }

        private int lifeSpan;

        public Flower(Point flowerLocation , Random random)
        {
            Location = flowerLocation;
            Age = 0;
            Alive = true;
            Nectar = InitialNectar;
            this.lifeSpan = random.Next(LifeSpanMin, LifeSpanMax + 1);
        }

        public double HarvestNektar()
        {
            if (NectarGatheredPerTurn > Nectar)
            {
                return 0;
            }

            else
            {
                Nectar = Nectar - NectarGatheredPerTurn;

                return NectarHarvested;
            }
        }

        public void Go()
        {
            Age++;
            if (Age > lifeSpan)
                //Flower will die!
                Alive = false;

            else
            {
                //Flower can live.
                Nectar += NectarAddedPerTurn;
                if (Nectar > MaxNectar)
                    Nectar = MaxNectar;
            }

        }

    }
}
