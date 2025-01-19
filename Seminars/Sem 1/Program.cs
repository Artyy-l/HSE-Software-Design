namespace PedalCarAccauntingInformationSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var customers = new List<Customer>
        {
            new() { Name = "Ivan" },
            new() { Name = "Petr" },
            new() { Name = "Sidr" },
        };

            var factory = new FactoryAF(customers);

            for (int i = 0; i < 5; i++)
                factory.AddCar();

            Console.WriteLine("\u001b[36mBefore\u001b[0m");
            Print(factory);
            Console.WriteLine("==========================================================================\n");

            factory.SaleCar();

            Console.WriteLine("\u001b[36mAfter\u001b[0m");
            Print(factory);
        }

        static void Print(FactoryAF factory)
        {
            Console.WriteLine(string.Join(Environment.NewLine, factory.Cars));
            Console.WriteLine(string.Join(Environment.NewLine, factory.Customers));
        }
    }
}
