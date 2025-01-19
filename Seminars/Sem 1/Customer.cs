using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedalCarAccauntingInformationSystem
{
    public class Customer
    {
        public required string Name { get; set; }

        public Car? Car { get; set; }

        public override string ToString()
        {
            return Car == null ? $"Имя: {Name}, Машины нет" : $"Имя: {Name}, Номер машины: {Car?.Number ?? -1}";
        }
    }
}
