using System;
using System.IO;
using System.Reflection;

class Program
{
    const string DllFileName = "DrinkLibrary.dll";
    const string ManufacturerClassName = "DrinkLibrary.Manufacturer";
    const string DrinkClassName = "DrinkLibrary.Drink";
    const string CreateMethodName = "Create";
    const string PrintMethodName = "PrintObject";

    const string ManufacturerName = "Coca-Cola";
    const string ManufacturerAddress = "Atlanta";
    const bool IsAChildCompany = false;

    const int DrinkId = 1;
    const string DrinkName = "Sprite";
    const string SerialNumber = "SN123456";
    const string DrinkType = "Soft";

    static void Main()
    {
        try
        {
            string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DllFileName);
            if (!File.Exists(dllPath))
            {
                Console.WriteLine($"Файл {DllFileName} не найден по пути: {dllPath}");
                return;
            }

            Assembly assembly = Assembly.LoadFrom(dllPath);

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

            Console.WriteLine("--- Выполнение задания 3: создание и вывод объектов ---");

            Type manufacturerType = assembly.GetType(ManufacturerClassName);
            MethodInfo createManufacturer = manufacturerType.GetMethod(CreateMethodName);
            object manufacturer = createManufacturer.Invoke(null, new object[]
            {
                ManufacturerName,
                ManufacturerAddress,
                IsAChildCompany
            });

            Type drinkType = assembly.GetType(DrinkClassName);
            MethodInfo createDrink = drinkType.GetMethod(CreateMethodName);
            object drink = createDrink.Invoke(null, new object[]
            {
                DrinkId,
                DrinkName,
                SerialNumber,
                DrinkType,
                manufacturer
            });

            MethodInfo printMethod = drinkType.GetMethod(PrintMethodName);
            printMethod.Invoke(drink, null);

            RunMethodInvoker(assembly);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void RunMethodInvoker(Assembly asm)
    {
        Console.WriteLine("\n--- Задание 1: Инвокатор методов через ввод ---");
        try
        {
            Console.Write("Введите полное имя класса (например, DrinkLibrary.Drink): ");
            string className = Console.ReadLine();

            Console.Write("Введите имя метода: ");
            string methodName = Console.ReadLine();

            Console.Write("Введите аргументы через запятую (без пробелов): ");
            string[] args = Console.ReadLine().Split(',');

            Type type = asm.GetType(className);
            if (type == null)
            {
                Console.WriteLine("Класс не найден.");
                return;
            }

            MethodInfo method = type.GetMethod(methodName);
            if (method == null)
            {
                Console.WriteLine("Метод не найден.");
                return;
            }

            ParameterInfo[] parameters = method.GetParameters();
            object[] parsedArgs = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                parsedArgs[i] = Convert.ChangeType(args[i], parameters[i].ParameterType);
            }

            object instance = method.IsStatic ? null : Activator.CreateInstance(type);
            object result = method.Invoke(instance, parsedArgs);

            Console.WriteLine("t" + "Метод выполнен успешно.");
            if (result != null)
                Console.WriteLine("r" + "Результат: " + result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("f" + "Ошибка при вызове метода: " + ex.Message);
        }
    }
}
