using System.Collections.Generic;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        ValueProvider requiredValues = new ValueProvider();
        Dictionary<String, String> values = requiredValues.getValues();
    }
}
