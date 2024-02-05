using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{


    class Bus : Auto
    {

        public const int PersonWeight = 80;
        public const int MaxPassangers = 40;
        public const int TicketCost = 100; // 100 рублей за проезд

        private int passangerCount;
        private int ticketsSold;

        public Bus(int maxGas, int consumption, int maxSpeed, int passangerCount) : base(maxGas, consumption, maxSpeed)
        {
            this.passangerCount = passangerCount;
            this.ticketsSold = passangerCount;
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Количество пассажиров: {passangerCount}");
            Console.WriteLine($"Выручка за билеты: {TicketSales} рублей");
        }

        public void AddPassengers(int inCount, int outCount)
        {
            passangerCount += inCount - outCount;

            if (passangerCount > 40)
            {
                throw new Exception("Кол-во пассажиров превышает допустимое значение 40.");
            }
            if (passangerCount < 0)
            {
                throw new Exception("Кол-во пассажиров не может быть отрицательным.");
            }

            ticketsSold += inCount;
        }

        public int TicketSales
        {
            get { return ticketsSold * TicketCost; }
        }

        public int PassangerCount
        {
            get { return passangerCount; }
        }

        public override int Weight
        {
            get { return passangerCount * PersonWeight; }
        }

        public override double Speed
        {
            get
            {
                if (Weight >= 2000)
                {
                    return Math.Round(maxSpeed / 2, 2);
                }
                else if (Weight >= 1000)
                {
                    return Math.Round(maxSpeed / 1.5, 2);
                }

                return maxSpeed;
            }
        }
    }


}

