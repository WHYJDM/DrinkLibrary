using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        try
        {
            Assembly assembly = Assembly.LoadFrom("DrinkLibrary.dll");

            Console.WriteLine("Классы в библиотеке:");
            foreach (Type type in assembly.GetTypes())
            {
                Console.WriteLine(type.FullName);
                Console.WriteLine("Свойства:");
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    Console.WriteLine($" - {prop.Name} ({prop.PropertyType.Name})");
                }
                Console.WriteLine();
            }

            Type manufacturerType = assembly.GetType("DrinkLibrary.Manufacturer");
            MethodInfo createManufacturer = manufacturerType.GetMethod("Create");
            object manufacturer = createManufacturer.Invoke(null, new object[] { "Coca-Cola", "Atlanta, USA", false });

            Type drinkType = assembly.GetType("DrinkLibrary.Drink");
            MethodInfo createDrink = drinkType.GetMethod("Create");
            object drink = createDrink.Invoke(null, new object[] { 1, "Sprite", "SN123456", "Soft", manufacturer });

            MethodInfo printMethod = drinkType.GetMethod("PrintObject");
            printMethod.Invoke(drink, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
