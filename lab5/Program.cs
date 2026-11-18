using System;
using System.Text;
using Lab5_UniversityStates.Models;
using Lab5_UniversityStates.StateMachine;

namespace Lab5_UniversityStates
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== СЦЕНАРІЙ 1: Успішне складання курсу з першої спроби ===");
            var nazarCtx = new CourseEnrollmentContext("Синявський Назар", "ПД-32", "ST-1001", true, "Програмування");
            var nazarSm = new EnrollmentStateMachine(nazarCtx);
            nazarSm.ConfirmEnrollment();
            nazarSm.SubmitExam(95);

            Console.WriteLine("\n=== СЦЕНАРІЙ 2: Перескладання та успішне закриття боргу ===");
            var rodionCtx = new CourseEnrollmentContext("Тараканов Родіон", "ПД-32", "ST-1002", true, "Бази даних");
            var rodionSm = new EnrollmentStateMachine(rodionCtx);
            rodionSm.ConfirmEnrollment();
            rodionSm.SubmitExam(48);     // < 60 -> перехід у Retake
            rodionSm.SubmitRetake(76);   // >= 60 -> перехід у Passed

            Console.WriteLine("\n=== СЦЕНАРІЙ 3: Заблокований перехід (неактивний квиток) ===");
            var guestCtx = new CourseEnrollmentContext("Петренко Олексій", "ПД-32", "ST-1099", false, "Вища математика");
            var guestSm = new EnrollmentStateMachine(guestCtx);
            guestSm.ConfirmEnrollment(); // Захисна умова [IsCardActive] не пройдена

            Console.WriteLine("\n=== СЦЕНАРІЙ 4: Неуспішне перескладання -> відрахування ===");
            var romanCtx = new CourseEnrollmentContext("Трошкін Роман", "ПД-32", "ST-1003", true, "Вища математика");
            var romanSm = new EnrollmentStateMachine(romanCtx);
            romanSm.ConfirmEnrollment();
            romanSm.SubmitExam(40);     // -> Retake
            romanSm.SubmitRetake(52);   // -> Expelled
            romanSm.SubmitRetake(80);   // Спроба дії з кінцевого стану (відхилено)

            Console.WriteLine("\n=== ПІДСУМКОВИЙ СТАН ОБ'ЄКТІВ (КОНТЕКСТІВ) ===");
            foreach (var c in new[] { nazarCtx, rodionCtx, guestCtx, romanCtx })
            {
                Console.WriteLine($"Студент: {c.StudentName,-18} | Курс: {c.CourseTitle,-16} | Стан: {c.CurrentState,-9} | Бал: {c.FinalGrade?.ToString() ?? "—"}");
            }

            Console.ReadLine();
        }
    }
}