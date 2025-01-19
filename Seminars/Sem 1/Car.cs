using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedalCarAccauntingInformationSystem;

public class Car
{
    private static readonly Random _random = new();

    public required int Number { get; set; }

    public Engine Engine { get; }

    public Car()
    {
        Engine = new Engine { Size = _random.Next(1, 10) };
    }

    public override string ToString()
    {
        return $"Номер: {Number}, Размер педалей: {Engine.Size}";
    }
}

