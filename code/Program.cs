using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace code
{

    class Program
    {

        public static void Main()
        {
            string vehicleType = inputVehicleType();
            int maxGas = inputMaxGas();
            int maxSpeed = inputMaxSpeed(vehicleType);
            int consumption = inputConsumption();

            Auto vehicle;

            switch (vehicleType)
            {
                case "BUS":
                    int passangerCountCount = inputPassangerCount();
                    vehicle = new Bus(maxGas, consumption, maxSpeed, passangerCountCount);
                    break;
                case "TRUCK":
                    int payloadWeight = inputPayloadWeight();
                    vehicle = new Truck(maxGas, consumption, maxSpeed, payloadWeight);
                    break;
                default:
                    throw new Exception("Неизвестный тип транспортного средства");
            }

            Console.WriteLine("\n===== Ваше новое транспортное средство: =====");
            vehicle.Print();
            Console.WriteLine("=============================================");


            Random random = new Random();

            do
            {
                double distance = random.Next(200, 1201);

                RideToDestination(vehicle, distance);

                Console.Write("\n> Хотите повторить путешествие? (1 = да / 2 = нет): ");
            } while (Console.ReadLine().ToLower() == "1");
        }

        public static void RideToDestination(Auto vehicle, double distance)
        {

            if (vehicle.Travelled > 0)
            {
                // Если это не первое путешествие, то даём возможность пользователю загрузить/выгрузить груз или зайти пассажирам
                StopAction(vehicle);
            }

            while (distance > 0)
            {
                Console.WriteLine($"\nВам осталось проехать: {Math.Round(distance, 2)}км\n\n> Нажмите Enter, чтобы продолжить");
                Console.ReadKey();
                Console.Write("Едем... ");


                double distanceCovered = vehicle.Move(distance);
                distance -= distanceCovered;

                // Задержка в зависимости от пройденного расстояния
                System.Threading.Thread.Sleep(Convert.ToInt32(distanceCovered * 5.0));

                Console.WriteLine($"Пройдено: {Math.Round(distanceCovered, 2)}км. Осталось проехать: {Math.Round(distance, 2)}км\n");

                if (distance <= 0)
                {
                    Console.WriteLine("Путь закончен. Вы проехали всю дистанцию.\n");
                    vehicle.Print();
                    break;
                }

                Console.Write($"> Бензин закончился! На сколько заправиться? Введите число не больше {vehicle.MaxGas - vehicle.RemainingGas}л или нажмите Enter, чтобы заправиться до полного бака: ");
                try
                {
                    double amount = double.Parse(Console.ReadLine());
                    vehicle.Refuel(amount);
                }
                catch
                {
                    // Если пользователь нажал Enter или ввёл некорректное значение, то заправляем до полного бака
                    vehicle.Refuel(vehicle.MaxGas - vehicle.RemainingGas);
                }

                StopAction(vehicle);
            }
        }

        // Метод запрашивает у пользователя действие, которое он хочет совершить на остановке
        private static void StopAction(Auto vehicle)
        {
            Random random = new Random();
            Console.WriteLine("");

            if (vehicle is Bus bus)
            {
                int inCount = random.Next(0, Bus.MaxPassangers - bus.PassangerCount);
                int outCount = random.Next(0, bus.PassangerCount);

                bus.AddPassengers(inCount, outCount);

                Console.WriteLine($"На остановке вошло {inCount} и вышло {outCount} пассажиров. Прибыль за билеты: {inCount * Bus.TicketCost}. Сейчас в автобусе: {bus.PassangerCount} человек");
            }
            else if (vehicle is Truck truck)
            {
                Console.Write("> Введите 1 для загрузки, 2 для разгрузки (Enter чтобы пропустить): ");
                string action = Console.ReadLine();

                if (action == "1")
                {
                    Console.WriteLine("> Сколько груза загрузить? Введите вес в килограммах");
                    int kgs = int.Parse(Console.ReadLine());
                    truck.Load(kgs);
                }
                else if (action == "2")
                {
                    Console.WriteLine("> Сколько груза выгрузить? Введите вес в килограммах");
                    int kgs = int.Parse(Console.ReadLine());
                    truck.Unload(kgs);
                }
            }

            Console.WriteLine("\n==== Текущее состояние транспортного средства: =====");
            vehicle.Print();
            Console.WriteLine("=====================================================");
        }

        private static string inputVehicleType()
        {
            Console.WriteLine("> Введите тип транспортного средства: ");
            Console.WriteLine("  1. Автобус");
            Console.WriteLine("  2. Грузовик");
            string vehicleType = Console.ReadLine();

            if (vehicleType == "1")
            {
                return "BUS";
            }
            else if (vehicleType == "2")
            {
                return "TRUCK";
            }
            else
            {
                Console.WriteLine("Неверный тип транспортного средства. Попробуйте еще раз.");
                return inputVehicleType();
            }
        }

        private static int inputMaxGas()
        {
            Console.Write("> Введите размер бензобака в литрах (от 0 до 30): ");
            int maxGas = int.Parse(Console.ReadLine());

            if (maxGas < 0 || maxGas > 30)
            {
                Console.WriteLine("Неверное количество бензина. Попробуйте еще раз.");
                return inputMaxGas();
            }

            return maxGas;
        }

        private static int inputMaxSpeed(string vehicleType)
        {
            string vehicleName = vehicleType == "BUS" ? "автобуса" : "грузовика";
            int speedLimit = vehicleType == "BUS" ? 110 : 80;

            Console.Write($"> Введите максимальную скорость (макс {speedLimit}км/ч для {vehicleName}): ");
            int maxSpeed = int.Parse(Console.ReadLine());

            if (maxSpeed < 1)
            {
                Console.WriteLine("Скорость не может быть меньше 1. Попробуйте еще раз.");
                return inputMaxSpeed(vehicleType);
            }

            if (maxSpeed > speedLimit)
            {
                Console.WriteLine("Слишком большая скорость. Попробуйте еще раз.");
                return inputMaxSpeed(vehicleType);
            }

            return maxSpeed;
        }

        private static int inputConsumption()
        {
            Console.Write("> Введите потребление бензина (от 5 до 15): ");
            int consumption = int.Parse(Console.ReadLine());

            if (consumption < 5 || consumption > 15)
            {
                Console.WriteLine("Неверное потребление. Попробуйте еще раз.");
                return inputConsumption();
            }

            return consumption;
        }

        private static int inputPassangerCount()
        {
            Console.Write($"> Введите количество людей в автобусе (не больше {Bus.MaxPassangers}): ");
            int passangerCountCount = int.Parse(Console.ReadLine());

            if (passangerCountCount < 0 || passangerCountCount > Bus.MaxPassangers)
            {
                Console.WriteLine("Неверное количество людей. Попробуйте еще раз.");
                return inputPassangerCount();
            }

            return passangerCountCount;
        }

        private static int inputPayloadWeight()
        {
            Console.Write($"> Введите вес груза в килограммах (не больше {Truck.MaxPayloadWeight}кг): ");
            int payloadWeight = int.Parse(Console.ReadLine());

            if (payloadWeight < 0 || payloadWeight > Truck.MaxPayloadWeight)
            {
                Console.WriteLine("Неверный вес груза. Попробуйте еще раз.");
                return inputPayloadWeight();
            }

            return payloadWeight;
        }
    }

}


