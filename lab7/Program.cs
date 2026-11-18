using System;
using System.Text;
using Lab7_UniversityPackages.BusinessLogic;
using Lab7_UniversityPackages.DataAccess;
using Lab7_UniversityPackages.Presentation;

namespace Lab7_UniversityPackages
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Збірка залежностей між рівнями (Dependency Injection)
            IUniversityRepository repository = new InMemoryUniversityRepository();
            var deaneryService = new DeaneryService(repository);
            var ui = new UniversityConsoleUI(deaneryService);

            ui.RunDemo();

            Console.ReadLine();
        }
    }
}