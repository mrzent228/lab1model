using System;
using UniversityApp.Repository;
using UniversityApp.Services;

namespace UniversityApp
{
    class Program
    {
        static void Main()
        {
            var repository = new UniversityRepository();
            var infoService = new UniversityInfoService(repository);

            // --- Групи ---
            var pd32 = repository.AddGroup("ПД-32");
            var pd33 = repository.AddGroup("ПД-33");

            // --- Студенти (один-до-багатьох з групою) ---
            var nazar = repository.AddStudent("Синявський Назар", pd32.Id);
            var rodion = repository.AddStudent("Тараканов Родіон", pd32.Id);
            var roman = repository.AddStudent("Трошкін Роман", pd32.Id);

            // --- Студентські квитки (один-до-одного) ---
            repository.AddStudentCard(nazar.Id, "ST-1001", new DateTime(2024, 9, 1));
            repository.AddStudentCard(rodion.Id, "ST-1002", new DateTime(2024, 9, 1));
            repository.AddStudentCard(roman.Id, "ST-1003", new DateTime(2024, 9, 1));

            // --- Курси ---
            var programming = repository.AddCourse("Програмування", 5);
            var databases = repository.AddCourse("Бази даних", 4);
            var math = repository.AddCourse("Вища математика", 3);

            // --- Запис на курси (багато-до-багатьох) ---
            repository.EnrollStudent(nazar.Id, programming.Id, 95);
            repository.EnrollStudent(nazar.Id, databases.Id, 88);
            repository.EnrollStudent(rodion.Id, programming.Id, 91);
            repository.EnrollStudent(rodion.Id, math.Id, 84);
            repository.EnrollStudent(roman.Id, databases.Id, 77);
            repository.EnrollStudent(roman.Id, math.Id, 90);

            // --- Виведення інформації ---
            infoService.PrintAll();
            infoService.PrintStudentFullInfo(nazar.Id);

            infoService.PrintGroupsWithStudents();
            infoService.PrintStudentCards();
            infoService.PrintCoursesWithStudents();

            Console.ReadKey();
        }
    }
}
