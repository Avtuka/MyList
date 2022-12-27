using System.Collections.Generic;

namespace MyList
{
    internal class Program
    {
        private static void Main(string[] args)
        {
        }

        private class Person
        {
            public int Age { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return $"Name: {Name} --- Age: {Age}";
            }
        }

        private class PersonComparer : IComparer<Person>
        {
            public int Compare(Person x, Person y)
            {
                return x.Age - y.Age;
            }
        }
    }
}