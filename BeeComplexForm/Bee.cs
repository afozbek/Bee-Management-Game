﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeComplexForm
{
    class Bee
    {
        //Constants
        private const double HoneyConsumed = 0.5;
        private const int MoveRate = 3;
        private const double MinimumFlowerNectar = 1.5;
        private const int CareerSpan = 1000;

        //Properties
        public int Age { get; private set; }
        public bool InsideHive { get; private set; }
        public double NectarCollected { get; private set; }
        public BeeState CurrentState { get; private set; }
        private Point location;
        public Point Location { get { return location; } }
        private int ID;
        private Flower destinationFlower;
        private World world;
        private Hive hive;


        public Bee(int id , Point location , World world , Hive hive)
        {
            this.world = world;
            this.hive = hive;
            this.ID = id;
            CurrentState = BeeState.Idle;
            Age = 0;
            this.location = location;
            InsideHive = true;
            destinationFlower = null;
            NectarCollected = 0;
        }

        public void Go(Random random)
        {
            Age++;
            switch (CurrentState)
            {
                //Çalışmama durumu
                case BeeState.Idle:
                    if (Age > CareerSpan)
                    {
                        CurrentState = BeeState.Retired;
                    }
                    else if (world.Flowers.Count > 0
                             && hive.ConsumeHoney(HoneyConsumed))
                    {
                        //Yeni flower nesnesi nerde bitmiş ise flower.count=6 ise flower[6]==null olduğundan... flower[6]=new flower(...);
                        Flower flower =
                            world.Flowers[random.Next(world.Flowers.Count)];
                        if (flower.Nectar >= MinimumFlowerNectar && flower.Alive)
                        {
                            destinationFlower = flower;
                            CurrentState = BeeState.FlyingToFlower;
                        }
                    }
                    break;

                case BeeState.FlyingToFlower:
                    // move towards the flower we’re heading to
                    if (!world.Flowers.Contains(destinationFlower))
                        CurrentState = BeeState.ReturningToHive;
                    else if (InsideHive)
                    {
                        if (MoveTowardsLocation(hive.GetLocation("Exit")))
                        {
                            InsideHive = false;
                            location = hive.GetLocation("Entrance");
                        }
                    }
                    else
                    if (MoveTowardsLocation(destinationFlower.Location))
                        CurrentState = BeeState.GatheringNectar;
                    break;

                case BeeState.GatheringNectar:
                    double nectar = destinationFlower.HarvestNectar();
                    if (nectar > 0)
                        NectarCollected += nectar;
                    else
                        CurrentState = BeeState.ReturningToHive;
                    break;

                case BeeState.ReturningToHive:
                    if (!InsideHive)
                    {
                        // move towards the hive
                        if (MoveTowardsLocation(hive.GetLocation("Entrance")))
                        {
                            InsideHive = true;
                            location = hive.GetLocation("Exit");
                        }
                    }
                    else
                    {
                        //If we are inside the hive
                        if (MoveTowardsLocation(hive.GetLocation("HoneyFactory")))
                            CurrentState = BeeState.MakingHoney;
                        // what do we do if we’re inside the hive?
                    }

                    break;

                case BeeState.MakingHoney:
                    if (NectarCollected < 0.5)
                    {
                        NectarCollected = 0;
                        CurrentState = BeeState.Idle;
                    }
                    else
                    {
                        // once we have a Hive, we’ll turn the nectar into honey
                        if (hive.AddHoney(0.5))
                            NectarCollected -= 0.5;
                        else
                            NectarCollected = 0;

                    }
                    break;

                case BeeState.Retired:
                    // Do nothing! We’re retired!
                    break;
                default: break;
            }
        }

        private bool MoveTowardsLocation(Point destination)
        {
            if (Math.Abs(destination.X - location.X) <= MoveRate &&
                Math.Abs(destination.Y - location.Y) <= MoveRate)
                return true;
            if (destination.X > location.X)
                location.X += MoveRate;
            else if (destination.X < location.X)
                location.X -= MoveRate;
            if (destination.Y > location.Y)
                location.Y += MoveRate;
            else if (destination.Y < location.Y)
                location.Y -= MoveRate;
            return false;
        }

        public enum BeeState
        {
            Idle,
            FlyingToFlower,
            GatheringNectar,
            ReturningToHive,
            MakingHoney,
            Retired
        }
    }
}
