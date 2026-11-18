// See https:/using System;
using System.Text;
using Lab4_University.Services;

namespace Lab4_University
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var edebo = new EdeboRegistryStub();
            var notifier = new ConsoleNotificationService();
            var system = new UniversitySystemService(edebo, notifier);

            system.AddGroup("ПД-32");

            Console.WriteLine("=== 1. Реєстрація студентів та інтеграція з ЄДЕБО ===");
            var nazar = system.RegisterStudent("Синявський Назар", "nazar.s@duikt.edu.ua", "ПД-32", "ST-1001");
            var rodion = system.RegisterStudent("Тараканов Родіон", "rodion.t@duikt.edu.ua", "ПД-32", "ST-1002");
            var roman = system.RegisterStudent("Трошкін Роман", "roman.t@duikt.edu.ua", "ПД-32", "ST-1003");

            try
            {
                system.RegisterStudent("Тестовий Студент", "test@duikt.edu.ua", "ПД-32", "INVALID-99");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Контроль надійності] Перехоплено помилку: {ex.Message}");
            }

            Console.WriteLine("\n=== 2. Запис на курси та виставлення оцінок ===");
            var c1 = system.AddCourse("Програмування", 5);
            var c2 = system.AddCourse("Бази даних", 4);
            var c3 = system.AddCourse("Вища математика", 3);

            system.EnrollStudent(nazar.Id, c1.Id);
            system.EnrollStudent(nazar.Id, c2.Id);
            system.EnrollStudent(rodion.Id, c1.Id);
            system.EnrollStudent(roman.Id, c3.Id);

            system.SetGrade(nazar.Id, c1.Id, 95);
            system.SetGrade(nazar.Id, c2.Id, 88);
            system.SetGrade(rodion.Id, c1.Id, 91);
            system.SetGrade(roman.Id, c3.Id, 84);

            try
            {
                system.SetGrade(nazar.Id, c1.Id, 120);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Контроль бізнес-правил] Перехоплено помилку: {ex.Message}");
            }

            system.PrintSummaryReport();
            Console.ReadLine();
        }
    }
}