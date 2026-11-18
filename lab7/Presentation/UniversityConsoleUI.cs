using System;
using System.Linq;
using Lab7_UniversityPackages.BusinessLogic;
using Lab7_UniversityPackages.Domain;

namespace Lab7_UniversityPackages.Presentation
{
    public class UniversityConsoleUI
    {
        private readonly DeaneryService _deaneryService;

        public UniversityConsoleUI(DeaneryService deaneryService)
        {
            _deaneryService = deaneryService;
        }

        public void RunDemo()
        {
            Console.WriteLine("=== АРХІТЕКТУРНІ ПАКЕТИ СИСТЕМИ УНІВЕРСИТЕТУ ===");
            Console.WriteLine("[Presentation] -> [BusinessLogic] -> [DataAccess] -> [Domain]\n");

            // 1. Ініціалізація через рівень бізнес-логіки
            _deaneryService.CreateGroup("ПД-32");

            var nazar = _deaneryService.RegisterStudent("Синявський Назар", "ПД-32");
            var rodion = _deaneryService.RegisterStudent("Тараканов Родіон", "ПД-32");
            var roman = _deaneryService.RegisterStudent("Трошкін Роман", "ПД-32");

            var c1 = _deaneryService.CreateCourse("Програмування", 5);
            var c2 = _deaneryService.CreateCourse("Бази даних", 4);
            var c3 = _deaneryService.CreateCourse("Вища математика", 3);

            // 2. Запис на курси та виставлення оцінок
            _deaneryService.EnrollStudent(nazar.Id, c1.Id);
            _deaneryService.EnrollStudent(nazar.Id, c2.Id);
            _deaneryService.EnrollStudent(rodion.Id, c1.Id);
            _deaneryService.EnrollStudent(roman.Id, c3.Id);

            _deaneryService.AssignGrade(nazar.Id, c1.Id, 95);
            _deaneryService.AssignGrade(nazar.Id, c2.Id, 88);
            _deaneryService.AssignGrade(rodion.Id, c1.Id, 91);
            _deaneryService.AssignGrade(roman.Id, c3.Id, 84);

            // 3. Відображення даних на рівні представлення (UI)
            PrintAcademicReport();
        }

        private void PrintAcademicReport()
        {
            Console.WriteLine("=== ЗВІТ ПРО УСПІШНІСТЬ СТУДЕНТІВ (РІВЕНЬ PRESENTATION) ===");
            foreach (Student st in _deaneryService.GetAllStudents())
            {
                double avg = st.Enrollments.Where(e => e.Grade.HasValue).Select(e => e.Grade.Value).DefaultIfEmpty(0).Average();
                Console.WriteLine($"Студент: {st.FullName,-18} | Група: {st.Group.Name} | Квиток: {st.Card.CardNumber} | Середній бал: {avg:F1}");
                foreach (var enr in st.Enrollments)
                {
                    Console.WriteLine($"   * {enr.Course.Title} ({enr.Course.Credits} кред.) — {enr.Grade} балів");
                }
            }
        }
    }
}