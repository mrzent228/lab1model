using System;
using System.Collections.Generic;
using System.Linq;
using UniversityUseCaseApp.Models;

namespace UniversityUseCaseApp.Services
{
    public class UniversityUseCaseService
    {
        public List<Group> Groups { get; } = new List<Group>();
        public List<Student> Students { get; } = new List<Student>();
        public List<Course> Courses { get; } = new List<Course>();

        private int _studentSeq = 1;
        private int _courseSeq = 1;
        private int _cardSeq = 1001;

        public Group CreateGroup(string name)
        {
            var group = new Group(name);
            Groups.Add(group);
            return group;
        }

        // UC-1: Реєстрація студента + <<include>> UC-2: Видача студентського квитка
        public Student UC1_RegisterStudent(string fullName, string groupName)
        {
            var group = Groups.FirstOrDefault(g => g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));
            if (group == null)
                throw new InvalidOperationException($"Альтернативний потік: групу '{groupName}' не знайдено!");

            var student = new Student(_studentSeq++, fullName, group);
            group.Students.Add(student);
            Students.Add(student);

            // <<include>> UC-2
            UC2_IssueStudentCard(student);
            return student;
        }

        // UC-2: Оформлення студентського квитка
        public void UC2_IssueStudentCard(Student student)
        {
            var cardNumber = $"ST-{_cardSeq++}";
            student.Card = new StudentCard(cardNumber, new DateTime(2024, 9, 1));
            Console.WriteLine($"[UC-1 / UC-2] Зареєстровано: {student.FullName} (гр. {student.Group.Name}), видано квиток № {cardNumber}");
        }

        // UC-3: Адміністрування навчальних курсів
        public Course UC3_AddCourse(string title, int credits)
        {
            var course = new Course(_courseSeq++, title, credits);
            Courses.Add(course);
            return course;
        }

        // UC-6: Перевірка студентського квитка (<<include>>)
        public bool UC6_VerifyStudentCard(Student student)
        {
            return student?.Card != null && student.Card.IsActive;
        }

        // UC-4: Запис студента на курс
        public void UC4_EnrollStudent(int studentId, int courseId)
        {
            var student = Students.FirstOrDefault(s => s.Id == studentId)
                ?? throw new InvalidOperationException("Студента не знайдено.");
            var course = Courses.FirstOrDefault(c => c.Id == courseId)
                ?? throw new InvalidOperationException("Курс не знайдено.");

            // Виклик включеного прецеденту UC-6
            if (!UC6_VerifyStudentCard(student))
            {
                Console.WriteLine($"[UC-4 Помилка] У студента {student.FullName} відсутній активний квиток!");
                return;
            }

            if (student.Enrollments.Any(e => e.Course.Id == courseId))
            {
                Console.WriteLine($"[UC-4 Альтернативний потік] {student.FullName} вже записаний на курс '{course.Title}'.");
                return;
            }

            var enrollment = new Enrollment(student, course);
            student.Enrollments.Add(enrollment);
            course.Enrollments.Add(enrollment);
            Console.WriteLine($"[UC-4] Студента {student.FullName} записано на курс '{course.Title}'.");
        }

        // UC-5: Виставлення підсумкової оцінки
        public void UC5_SetGrade(int studentId, int courseId, int grade)
        {
            if (grade < 0 || grade > 100)
            {
                Console.WriteLine("[UC-5 Помилка] Оцінка має бути в діапазоні від 0 до 100.");
                return;
            }

            var enrollment = Students.FirstOrDefault(s => s.Id == studentId)?
                .Enrollments.FirstOrDefault(e => e.Course.Id == courseId);

            if (enrollment == null)
            {
                Console.WriteLine("[UC-5 Помилка] Студент не записаний на цей курс.");
                return;
            }

            enrollment.Grade = grade;
            Console.WriteLine($"[UC-5] Викладач виставив {grade} балів студенту {enrollment.Student.FullName} з курсу '{enrollment.Course.Title}'.");
        }

        // UC-7: Перегляд залікової книжки студента
        public void UC7_PrintGradebook(int studentId)
        {
            var student = Students.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                Console.WriteLine("Студента не знайдено.");
                return;
            }

            Console.WriteLine($"\n=== [UC-7] ЗАЛІКОВА КНИЖКА: {student.FullName} ===");
            Console.WriteLine($"Група: {student.Group.Name} | Квиток: № {student.Card.CardNumber} (діє з {student.Card.IssueDate:dd.MM.yyyy})");
            foreach (var enr in student.Enrollments)
            {
                string mark = enr.Grade.HasValue ? $"{enr.Grade} балів" : "не оцінено";
                Console.WriteLine($"  * {enr.Course.Title} ({enr.Course.Credits} кред.) — {mark}");
            }
        }
    }
}