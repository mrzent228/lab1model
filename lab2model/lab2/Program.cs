using System;
using System.Text;
using UniversityUseCaseApp.Services;

namespace UniversityUseCaseApp
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            var service = new UniversityUseCaseService();

            // Ініціалізація початкових даних через прецеденти
            service.CreateGroup("ПД-32");
            service.CreateGroup("ПД-33");

            var nazar = service.UC1_RegisterStudent("Синявський Назар", "ПД-32");
            var rodion = service.UC1_RegisterStudent("Тараканов Родіон", "ПД-32");
            var roman = service.UC1_RegisterStudent("Трошкін Роман", "ПД-32");

            var c1 = service.UC3_AddCourse("Програмування", 5);
            var c2 = service.UC3_AddCourse("Бази даних", 4);
            var c3 = service.UC3_AddCourse("Вища математика", 3);

            Console.WriteLine();
            service.UC4_EnrollStudent(nazar.Id, c1.Id);
            service.UC4_EnrollStudent(nazar.Id, c2.Id);
            service.UC4_EnrollStudent(rodion.Id, c1.Id);
            service.UC4_EnrollStudent(roman.Id, c3.Id);

            // Перевірка альтернативного потоку (повторний запис)
            service.UC4_EnrollStudent(nazar.Id, c1.Id);

            Console.WriteLine();
            service.UC5_SetGrade(nazar.Id, c1.Id, 95);
            service.UC5_SetGrade(nazar.Id, c2.Id, 88);
            service.UC5_SetGrade(rodion.Id, c1.Id, 91);
            service.UC5_SetGrade(roman.Id, c3.Id, 90);

            // Перегляд заліковки студента (UC-7)
            service.UC7_PrintGradebook(nazar.Id);
            service.UC7_PrintGradebook(rodion.Id);
            service.UC7_PrintGradebook(roman.Id);
            Console.ReadLine();
        }
    }
}