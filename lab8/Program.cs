using System;
using System.Text;
using Lab8_UniversityClasses.Interfaces;
using Lab8_UniversityClasses.Models;
using Lab8_UniversityClasses.Services;

namespace Lab8_UniversityClasses
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Шаблонні реєстри (Generic Classes)
            var peopleRegistry = new UniversityRegistry<Person>();
            var enrollmentRegistry = new UniversityRegistry<Enrollment>();

            // Агрегація: Група
            var groupPd32 = new AcademicGroup("ПД-32", "121 Інженерія програмного забезпечення");

            // Спадкоємці Person: Викладачі та Студенти (з композицією StudentCard)
            var teacher1 = new Teacher("Гаманюк І. М.", "hamaniuk@duikt.edu.ua", "ст. викладач", "ІПЗ");
            var teacher2 = new Teacher("Коваленко О. В.", "kovalenko@duikt.edu.ua", "доцент, к.т.н.", "ІПЗ");

            var nazar = new Student("Синявський Назар", "nazar.s@duikt.edu.ua", "ST-1001", groupPd32);
            var rodion = new Student("Тараканов Родіон", "rodion.t@duikt.edu.ua", "ST-1002", groupPd32);
            var roman = new Student("Трошкін Роман", "roman.t@duikt.edu.ua", "ST-1003", groupPd32);

            foreach (Person p in new Person[] { teacher1, teacher2, nazar, rodion, roman })
                peopleRegistry.Register(p);

            // Курси (асоціація з викладачами)
            var c1 = new Course(1, "Програмування", 5, teacher1);
            var c2 = new Course(2, "Бази даних", 4, teacher2);
            var c3 = new Course(3, "Моделювання ПЗ", 4, teacher1);

            // Записи на курси та оцінювання (реалізація IGradable)
            var e1 = new Enrollment(nazar, c1); e1.SetGrade(95);
            var e2 = new Enrollment(nazar, c3); e2.SetGrade(92);
            var e3 = new Enrollment(rodion, c2); e3.SetGrade(88);
            var e4 = new Enrollment(roman, c3); e4.SetGrade(85);

            foreach (var e in new[] { e1, e2, e3, e4 })
            {
                e.Student.Enrollments.Add(e);
                enrollmentRegistry.Register(e);
            }

            Console.WriteLine($"=== УЧАСНИКИ ОСВІТНЬОГО ПРОЦЕСУ (ПОЛІМОРФІЗМ PERSON, ВСЬОГО: {peopleRegistry.Count}) ===");
            foreach (IReportable person in peopleRegistry.GetAll())
            {
                Console.WriteLine("  " + person.GetSummaryLine());
            }

            Console.WriteLine($"\n=== ВІДОМІСТЬ ОЦІНОК (РЕАЛІЗАЦІЯ IGRADABLE ТА IREPORTABLE, ЗАПИСІВ: {enrollmentRegistry.Count}) ===");
            foreach (var enr in enrollmentRegistry.GetAll())
            {
                Console.WriteLine("  " + enr.GetSummaryLine());
            }

            Console.ReadLine();
        }
    }
}