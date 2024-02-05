using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{

    using System;

    abstract class Auto
    {
        protected int maxGas;
        protected int consumption;
        protected double remainingGas;
        protected double maxSpeed;
        protected double travelled;
        protected double minutesInTravel;

        public Auto(int maxGas, int consumption, double maxSpeed)
        {
            this.maxGas = maxGas;
            this.remainingGas = new Random().Next(1, maxGas + 1);
            this.consumption = consumption;
            this.maxSpeed = maxSpeed;
        }

        public virtual void Print()
        {
            Console.WriteLine($"Бензин: {Math.Round(remainingGas, 2)}/{maxGas}л, Расход топлива: {consumption}л на 100км");
            Console.WriteLine($"На данный момент пройдено: {Math.Round(travelled, 2)}км, Время в пути: {Math.Round(minutesInTravel / 60, 2)}ч.");
            Console.WriteLine($"Вес: {Weight}кг, Достигаемая скорость при текущем весе: {Speed}км/ч");
        }

        public double Move(double maxDistance)
        {
            double distanceCovered = remainingGas / (Convert.ToDouble(consumption) / 100.0);
            distanceCovered = Math.Min(distanceCovered, maxDistance);
            travelled += distanceCovered;

            double consumedGas = distanceCovered * (Convert.ToDouble(consumption) / 100.0);
            remainingGas = Math.Max(remainingGas - consumedGas, 0);

            minutesInTravel += distanceCovered / Speed * 60.0;

            return distanceCovered;
        }

        public void Refuel(double amount)
        {
            remainingGas = Math.Min(remainingGas + amount, maxGas);
        }

        public double RemainingGas
        {
            get { return remainingGas; }
        }

        public double MaxGas
        {
            get { return maxGas; }
        }

        public double Travelled
        {
            get { return travelled; }
        }

        public abstract int Weight { get; }
        public abstract double Speed { get; }
    }


}
