using bookDemo.Models;

namespace bookDemo.Data
{
    public static class ApplicationContext
    {
        public static List<Book> books {get; set;}
        static ApplicationContext()
        {
            books = new List<Book>
            {
                new Book{Id=1,Name="Goriot Baba",Price=36},
                new Book{Id=2,Name="Kumarbaz ",Price=133},
                new Book{Id=3,Name="Hayvan Çiftliği",Price=44},
                new Book{Id=4,Name="Sefiller ",Price=16}
            };
        }
    }
}
