using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    using System;
    class Truck : Auto
    {
        public const int MaxPayloadWeight = 5000;

        private int payloadWeight;

        public Truck(int maxGas, int consumption, int maxSpeed, int payloadWeight) : base(maxGas, consumption, maxSpeed)
        {
            this.payloadWeight = payloadWeight;
        }

        public override void Print()
        {
            base.Print();
            double capacity = Math.Round(Convert.ToDouble(payloadWeight) / MaxPayloadWeight * 100, 2);
            Console.WriteLine($"Загружен на {capacity}%");
        }

        public void Load(int tonnage)
        {
            payloadWeight += tonnage;
            if (payloadWeight > MaxPayloadWeight)
            {
                throw new Exception($"Вес груза превышает допустимое значение {MaxPayloadWeight} тонн.");
            }
        }

        public void Unload(int tonnage)
        {
            payloadWeight = Math.Max(payloadWeight - tonnage, 0);
        }

        public override int Weight
        {
            get { return payloadWeight; }
        }

        public override double Speed
        {
            get
            {
                if (Weight >= 4000)
                {
                    return Math.Round(maxSpeed / 2, 2);
                }
                else if (Weight >= 2500)
                {
                    return Math.Round(maxSpeed / 1.5, 2);
                }
                return maxSpeed;
            }
        }
    }

}
