using System;
using System.Text;
using UniversityDomainApp.Models;

namespace UniversityDomainApp
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // 1. Створення груп
            var pd32 = new Group("ПД-32");
            var pd33 = new Group("ПД-33");

            // 2. Створення студентів та асоціація з групою (1 до 0..*)
            var nazar = new Student("Синявський Назар");
            var rodion = new Student("Тараканов Родіон");
            var roman = new Student("Трошкін Роман");

            pd32.AddStudent(nazar);
            pd32.AddStudent(rodion);
            pd32.AddStudent(roman);

            // 3. Видача студентських квитків (1 до 0..1)
            nazar.AssignCard(new StudentCard("ST-1001", new DateTime(2024, 9, 1)));
            rodion.AssignCard(new StudentCard("ST-1002", new DateTime(2024, 9, 1)));
            roman.AssignCard(new StudentCard("ST-1003", new DateTime(2024, 9, 1)));

            // 4. Створення курсів та запис студентів (асоціація через Enrollment)
            var programming = new Course("Програмування", 5);
            var databases = new Course("Бази даних", 4);
            var math = new Course("Вища математика", 3);

            nazar.EnrollInCourse(programming, 95);
            nazar.EnrollInCourse(databases, 88);

            rodion.EnrollInCourse(programming, 91);
            rodion.EnrollInCourse(math, 84);

            roman.EnrollInCourse(databases, 77);
            roman.EnrollInCourse(math, 90);

            // 5. Виведення інформації навігацією по об'єктній моделі
            Console.WriteLine("=== Групи та їхні студенти ===");
            foreach (var group in new[] { pd32, pd33 })
            {
                Console.WriteLine($"Група {group.Name} (студентів: {group.Students.Count}):");
                foreach (var st in group.Students)
                {
                    Console.WriteLine($"  - {st.FullName} | Квиток: {st.Card?.CardNumber ?? "немає"}");
                }
            }

            Console.WriteLine("\n=== Успішність студентів за дисциплінами ===");
            foreach (var course in new[] { programming, databases, math })
            {
                Console.WriteLine($"Курс: {course.Title} ({course.Credits} кредитів):");
                foreach (var enr in course.Enrollments)
                {
                    Console.WriteLine($"  - {enr.Student.FullName} ({enr.Student.Group.Name}) -> Оцінка: {enr.Grade}");
                }
            }

            Console.WriteLine($"\n=== Картка студента: {nazar.FullName} ===");
            Console.WriteLine($"Група: {nazar.Group.Name}");
            Console.WriteLine($"Студентський квиток: № {nazar.Card.CardNumber} (виданий {nazar.Card.IssueDate:dd.MM.yyyy})");
            Console.WriteLine("Записаний на курси:");
            foreach (var enr in nazar.Enrollments)
            {
                Console.WriteLine($"  * {enr.Course.Title} ({enr.Course.Credits} кред.) — оцінка: {enr.Grade}");
            }
        }
    }
}