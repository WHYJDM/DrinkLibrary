namespace DrinkLibrary
{
    public class Manufacturer
    {
        private bool IsAChildCompany;
        public string Name { get; set; }
        public string Address { get; set; }

        public static Manufacturer Create(string name, string address, bool isAChildCompany)
        {
            return new Manufacturer { Name = name, Address = address, IsAChildCompany = isAChildCompany };
        }

        public void PrintObject()
        {
            Console.WriteLine($"Manufacturer: Name={Name}, Address={Address}");
        }
    }
}
