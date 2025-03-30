namespace DrinkLibrary
{
    public class Drink
    {
        private int ID;
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string DrinkType { get; set; }

        public Manufacturer Manufacturer { get; set; }

        public static Drink Create(int id, string name, string serialNumber, string drinkType, Manufacturer manufacturer)
        {
            return new Drink { ID = id, Name = name, SerialNumber = serialNumber, DrinkType = drinkType, Manufacturer = manufacturer };
        }

        public void PrintObject()
        {
            Console.WriteLine($"Drink: Name={Name}, SerialNumber={SerialNumber}, DrinkType={DrinkType}, Manufacturer={Manufacturer.Name}");
        }
    }
}
