using System;
using System.Text;
using Lab6_UniversitySequence.Components;

namespace Lab6_UniversitySequence
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Ініціалізація учасників діаграми послідовності (Lifelines)
            var studentRepo = new StudentRepository();
            var cardService = new CardService();
            var enrollmentService = new CourseEnrollmentService();
            var controller = new DeaneryController(studentRepo, cardService, enrollmentService);

            Console.WriteLine("=== 1. Послідовність реєстрації студентів та запису на курси ===");
            controller.RegisterAndEnrollStudent("Синявський Назар", "ПД-32", "Програмування", 5);
            controller.RegisterAndEnrollStudent("Тараканов Родіон", "ПД-32", "Бази даних", 4);
            controller.RegisterAndEnrollStudent("Трошкін Роман", "ПД-32", "Вища математика", 3);

            Console.WriteLine("\n=== 2. Альтернативний потік (фрагмент alt на діаграмі) ===");
            controller.EnrollExistingStudent(1, "Бази даних", 4);      // Успішний дозапис
            controller.EnrollExistingStudent(1, "Програмування", 5);   // Відмова: дублювання курсу

            Console.WriteLine("\n=== ПІДСУМКОВИЙ РЕЄСТР ЗАПИСІВ НА КУРСИ ===");
            foreach (var rec in enrollmentService.GetAllRecords())
            {
                Console.WriteLine($"Студент: {rec.Student.FullName,-18} | Квиток: {rec.Student.Card.CardNumber} | Курс: {rec.CourseTitle,-16} | Кредитів: {rec.Credits}");
            }

            Console.ReadLine();
        }
    }
}