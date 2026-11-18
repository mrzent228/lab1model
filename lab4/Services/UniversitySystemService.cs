using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lab4_University.Models;

namespace Lab4_University.Services
{
    public class UniversitySystemService
    {
        private readonly IEdeboRegistryService _edeboService;
        private readonly INotificationService _notificationService;

        public List<Group> Groups { get; } = new List<Group>();
        public List<Student> Students { get; } = new List<Student>();
        public List<Course> Courses { get; } = new List<Course>();
        public List<string> AuditLog { get; } = new List<string>();

        private int _studentIdSeq = 1;
        private int _courseIdSeq = 1;

        public UniversitySystemService(IEdeboRegistryService edeboService, INotificationService notificationService)
        {
            _edeboService = edeboService;
            _notificationService = notificationService;
        }

        private void Log(string action)
        {
            AuditLog.Add($"[{DateTime.Now:HH:mm:ss.fff}] {action}");
        }

        public Group AddGroup(string name)
        {
            var group = new Group(name);
            Groups.Add(group);
            Log($"Створено групу '{name}'.");
            return group;
        }

        public Student RegisterStudent(string fullName, string email, string groupName, string cardNumber)
        {
            var sw = Stopwatch.StartNew();

            var group = Groups.FirstOrDefault(g => g.Name == groupName)
                ?? throw new InvalidOperationException($"Групу '{groupName}' не знайдено.");

            if (!_edeboService.ValidateCardFormatAndRegister(cardNumber))
            {
                Log($"ВІДХИЛЕНО: некоректний або дубльований номер квитка '{cardNumber}' для {fullName}.");
                throw new InvalidOperationException($"Помилка валідації ЄДЕБО: квиток '{cardNumber}' не відповідає формату ST-XXXX або вже існує.");
            }

            var student = new Student(_studentIdSeq++, fullName, email, group)
            {
                Card = new StudentCard(cardNumber, new DateTime(2024, 9, 1)) { IsVerifiedInEdebo = true }
            };

            group.Students.Add(student);
            Students.Add(student);

            sw.Stop();
            Log($"Зареєстровано студента {fullName} (квиток {cardNumber}, ЄДЕБО: ОК). Час відгуку: {sw.Elapsed.TotalMilliseconds:F2} мс.");
            _notificationService.SendNotification(email, $"Вітаємо! Вас зараховано до групи {groupName}. Студентський квиток: {cardNumber}.");

            return student;
        }

        public Course AddCourse(string title, int credits)
        {
            var course = new Course(_courseIdSeq++, title, credits);
            Courses.Add(course);
            Log($"Додано курс '{title}' ({credits} кред.).");
            return course;
        }

        public void EnrollStudent(int studentId, int courseId)
        {
            var student = Students.First(s => s.Id == studentId);
            var course = Courses.First(c => c.Id == courseId);

            if (student.Card == null || !student.Card.IsVerifiedInEdebo)
                throw new InvalidOperationException("Запис неможливий: квиток не підтверджено в ЄДЕБО.");

            if (student.Enrollments.Any(e => e.Course.Id == courseId))
                throw new InvalidOperationException($"Студент {student.FullName} вже записаний на курс '{course.Title}'.");

            student.Enrollments.Add(new Enrollment(student, course));
            Log($"Студента {student.FullName} записано на курс '{course.Title}'.");
        }

        public void SetGrade(int studentId, int courseId, int grade)
        {
            if (grade < 0 || grade > 100)
            {
                Log($"ВІДХИЛЕНО: спроба виставити некоректний бал ({grade}).");
                throw new ArgumentOutOfRangeException(nameof(grade), "Бізнес-правило BR-04: оцінка має бути в межах від 0 до 100.");
            }

            var enrollment = Students.First(s => s.Id == studentId)
                .Enrollments.FirstOrDefault(e => e.Course.Id == courseId)
                ?? throw new InvalidOperationException("Запис на курс не знайдено.");

            enrollment.Grade = grade;
            Log($"Виставлено оцінку {grade} ({enrollment.EctsGrade}) студенту {enrollment.Student.FullName} з курсу '{enrollment.Course.Title}'.");
            _notificationService.SendNotification(enrollment.Student.Email, $"Оновлено оцінку з курсу «{enrollment.Course.Title}»: {grade} балів (ECTS: {enrollment.EctsGrade}).");
        }

        public void PrintSummaryReport()
        {
            Console.WriteLine("\n=== ВІДОМІСТЬ УСПІШНОСТІ (ШКАЛА ECTS) ===");
            foreach (var st in Students)
            {
                double avg = st.Enrollments.Where(e => e.Grade.HasValue).Select(e => e.Grade.Value).DefaultIfEmpty(0).Average();
                Console.WriteLine($"Студент: {st.FullName} | Група: {st.Group.Name} | Квиток: {st.Card.CardNumber} (ЄДЕБО: ✓) | Середній бал: {avg:F1}");
                foreach (var enr in st.Enrollments)
                {
                    Console.WriteLine($"  - {enr.Course.Title} ({enr.Course.Credits} кред.): {enr.Grade} балів [ECTS: {enr.EctsGrade}]");
                }
            }

            Console.WriteLine("\n=== ЖУРНАЛ АУДИТУ ТА ПРОДУКТИВНОСТІ (FURPS+) ===");
            foreach (var log in AuditLog)
            {
                Console.WriteLine(log);
            }
        }
    }
}