using System;
using UniversityApp.Repository;

namespace UniversityApp.Services
{
    /// <summary>
    /// Сервіс виведення інформації щодо даних системи автоматизації
    /// роботи університету (демонструє всі три типи зв'язків).
    /// </summary>
    public class UniversityInfoService
    {
        private readonly UniversityRepository _repository;

        public UniversityInfoService(UniversityRepository repository)
        {
            _repository = repository;
        }

        /// <summary>Виводить усі групи разом зі студентами (один-до-багатьох).</summary>
        public void PrintGroupsWithStudents()
        {
            Console.WriteLine("=== Групи та їхні студенти (один-до-багатьох) ===");
            foreach (var group in _repository.GetGroups())
            {
                Console.WriteLine($"{group}:");
                foreach (var student in group.Students)
                    Console.WriteLine($"   - {student}");
            }
            Console.WriteLine();
        }

        /// <summary>Виводить студентів разом з їхніми студентськими квитками (один-до-одного).</summary>
        public void PrintStudentCards()
        {
            Console.WriteLine("=== Студенти та їхні студентські квитки (один-до-одного) ===");
            foreach (var student in _repository.GetStudents())
            {
                var cardInfo = student.Card != null ? student.Card.ToString() : "квиток відсутній";
                Console.WriteLine($"{student} -> {cardInfo}");
            }
            Console.WriteLine();
        }

        /// <summary>Виводить курси разом зі студентами, що на них записані (багато-до-багатьох).</summary>
        public void PrintCoursesWithStudents()
        {
            Console.WriteLine("=== Курси та записані студенти (багато-до-багатьох) ===");
            foreach (var course in _repository.GetCourses())
            {
                Console.WriteLine($"{course}:");
                foreach (var enrollment in course.Enrollments)
                {
                    var gradeInfo = enrollment.Grade.HasValue ? $", оцінка: {enrollment.Grade}" : "";
                    Console.WriteLine($"   - {enrollment.Student.FullName}{gradeInfo}");
                }
            }
            Console.WriteLine();
        }

        /// <summary>Виводить повну інформацію по одному студенту: групу, квиток, курси.</summary>
        public void PrintStudentFullInfo(int studentId)
        {
            var student = _repository.GetStudentById(studentId);
            if (student == null)
            {
                Console.WriteLine($"Студента з ID={studentId} не знайдено.");
                return;
            }

            Console.WriteLine($"=== Повна інформація про студента: {student.FullName} ===");
            Console.WriteLine($"Група: {student.Group?.Name ?? "не призначена"}");
            Console.WriteLine($"Студентський квиток: {student.Card?.ToString() ?? "відсутній"}");
            Console.WriteLine("Курси:");
            foreach (var enrollment in student.Enrollments)
                Console.WriteLine($"   - {enrollment.Course.Title} (оцінка: {enrollment.Grade?.ToString() ?? "немає"})");
            Console.WriteLine();
        }

        public void PrintAll()
        {
            PrintGroupsWithStudents();
            PrintStudentCards();
            PrintCoursesWithStudents();
        }
    }
}
