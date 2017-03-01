using System;
using System.Linq;

namespace Buh.Domain.Services
{
    public class BuhService
    {
        public string GenerateRandomMessage()
        {
            var random = new Random();

            switch (random.Next(0, 3))
            {
                case 0:
                    return "Б" + string.Join("", Enumerable.Range(0, random.Next(2, 6)).Select(x => "y")) + "х!";
                case 1:
                    return "Ен" + string.Join("", Enumerable.Range(0, random.Next(2, 6)).Select(x => "о")) + "т!";
                case 2:
                    return "Прип" + string.Join("", Enumerable.Range(0, random.Next(2, 6)).Select(x => "о")) + "лз енот";
                default:
                    return "Всё равно бух.";
            }
        }
    }
}