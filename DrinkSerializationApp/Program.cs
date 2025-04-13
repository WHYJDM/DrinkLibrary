using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

namespace DrinkSerializationApp
{
    internal class Program
    {
        private const string FileName = "drinks.xml";

        static void Main()
        {
            List<DrinkLibrary.Drink> drinks = new List<DrinkLibrary.Drink>();

            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Создать 10 напитков");
                Console.WriteLine("2. Сериализовать в XML");
                Console.WriteLine("3. Показать содержимое XML-файла");
                Console.WriteLine("4. Десериализовать из XML и вывести");
                Console.WriteLine("5. Найти все Model (XDocument)");
                Console.WriteLine("6. Найти все Model (XmlDocument)");
                Console.WriteLine("7. Изменить значение атрибута (XDocument)");
                Console.WriteLine("8. Изменить значение атрибута (XmlDocument)");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");

                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        drinks = CreateDrinks();
                        Console.WriteLine("Создано 10 объектов.");
                        break;
                    case "2":
                        SerializeToXml(drinks);
                        break;
                    case "3":
                        Console.WriteLine(File.ReadAllText(FileName));
                        break;
                    case "4":
                        DeserializeFromXml();
                        break;
                    case "5":
                        FindModelWithXDocument();
                        break;
                    case "6":
                        FindModelWithXmlDocument();
                        break;
                    case "7":
                        UpdateAttributeXDocument();
                        break;
                    case "8":
                        UpdateAttributeXmlDocument();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }
            }
        }

        private static List<DrinkLibrary.Drink> CreateDrinks()
        {
            var list = new List<DrinkLibrary.Drink>();

            for (int i = 1; i <= 10; i++)
            {
                var manufacturer = DrinkLibrary.Manufacturer.Create($"Завод #{i}", $"Город #{i}", false);
                var drink = DrinkLibrary.Drink.Create(i, $"Напиток {i}", $"SN{i:0000}", $"Soft", manufacturer);
                list.Add(drink);
            }

            return list;
        }

        private static void SerializeToXml(List<DrinkLibrary.Drink> drinks)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<DrinkLibrary.Drink>));
                using var fs = new FileStream(FileName, FileMode.Create);
                serializer.Serialize(fs, drinks);
                Console.WriteLine($"Сериализация завершена. Файл: {FileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сериализации: {ex.Message}");
            }
        }

        private static void DeserializeFromXml()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<DrinkLibrary.Drink>));
                using var fs = new FileStream(FileName, FileMode.Open);
                var drinks = (List<DrinkLibrary.Drink>)serializer.Deserialize(fs);
                foreach (var drink in drinks)
                {
                    drink.PrintObject();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении XML: {ex.Message}");
            }
        }

        private static void FindModelWithXDocument()
        {
            var doc = XDocument.Load(FileName);
            foreach (var elem in doc.Descendants("Drink"))
            {
                var attr = elem.Attribute("Model");
                if (attr != null)
                    Console.WriteLine($"Model: {attr.Value}");
            }
        }

        private static void FindModelWithXmlDocument()
        {
            var doc = new XmlDocument();
            doc.Load(FileName);
            var nodes = doc.GetElementsByTagName("Drink");
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes?["Model"] != null)
                    Console.WriteLine($"Model: {node.Attributes["Model"].Value}");
            }
        }

        private static void UpdateAttributeXDocument()
        {
            Console.Write("Введите номер элемента (0-9): ");
            int index = int.Parse(Console.ReadLine());

            Console.Write("Введите имя атрибута: ");
            string attrName = Console.ReadLine();

            Console.Write("Введите новое значение: ");
            string newValue = Console.ReadLine();

            var doc = XDocument.Load(FileName);
            var drinks = doc.Descendants("Drink").ToList();

            if (index >= 0 && index < drinks.Count)
            {
                var drink = drinks[index];
                var attr = drink.Attribute(attrName);
                if (attr != null)
                {
                    attr.Value = newValue;
                    doc.Save(FileName);
                    Console.WriteLine("Атрибут обновлён.");
                }
                else
                {
                    Console.WriteLine("Атрибут не найден.");
                }
            }
        }

        private static void UpdateAttributeXmlDocument()
        {
            Console.Write("Введите номер элемента (0-9): ");
            int index = int.Parse(Console.ReadLine());

            Console.Write("Введите имя атрибута: ");
            string attrName = Console.ReadLine();

            Console.Write("Введите новое значение: ");
            string newValue = Console.ReadLine();

            var doc = new XmlDocument();
            doc.Load(FileName);

            var drinks = doc.GetElementsByTagName("Drink");

            if (index >= 0 && index < drinks.Count)
            {
                var node = drinks[index];
                if (node.Attributes?[attrName] != null)
                {
                    node.Attributes[attrName].Value = newValue;
                    doc.Save(FileName);
                    Console.WriteLine("Атрибут обновлён.");
                }
                else
                {
                    Console.WriteLine("Атрибут не найден.");
                }
            }
        }
    }
}
//q