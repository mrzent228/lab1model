using System;
using System.Collections.Generic;
using System.Linq;
using Lab7_UniversityPackages.DataAccess;
using Lab7_UniversityPackages.Domain;

namespace Lab7_UniversityPackages.BusinessLogic
{
    public class DeaneryService
    {
        private readonly IUniversityRepository _repo;
        private int _studentSeq = 1;
        private int _courseSeq = 1;
        private int _cardSeq = 1001;

        public DeaneryService(IUniversityRepository repo)
        {
            _repo = repo;
        }

        public Group CreateGroup(string name)
        {
            var group = new Group(name);
            _repo.AddGroup(group);
            return group;
        }

        public Student RegisterStudent(string fullName, string groupName)
        {
            var group = _repo.GetGroup(groupName)
                ?? throw new InvalidOperationException($"Групу '{groupName}' не знайдено.");

            var student = new Student(_studentSeq++, fullName, group)
            {
                Card = new StudentCard($"ST-{_cardSeq++}", new DateTime(2024, 9, 1))
            };

            group.Students.Add(student);
            _repo.AddStudent(student);
            return student;
        }

        public Course CreateCourse(string title, int credits)
        {
            var course = new Course(_courseSeq++, title, credits);
            _repo.AddCourse(course);
            return course;
        }

        public void EnrollStudent(int studentId, int courseId)
        {
            var student = _repo.GetStudent(studentId) ?? throw new InvalidOperationException("Студента не знайдено.");
            var course = _repo.GetCourse(courseId) ?? throw new InvalidOperationException("Курс не знайдено.");

            if (student.Enrollments.Any(e => e.Course.Id == courseId))
                throw new InvalidOperationException($"Студент {student.FullName} вже записаний на курс '{course.Title}'.");

            student.Enrollments.Add(new Enrollment(student, course));
        }

        public void AssignGrade(int studentId, int courseId, int grade)
        {
            if (grade < 0 || grade > 100)
                throw new ArgumentOutOfRangeException(nameof(grade), "Оцінка має бути в діапазоні 0..100.");

            var enrollment = _repo.GetStudent(studentId)?.Enrollments.FirstOrDefault(e => e.Course.Id == courseId)
                ?? throw new InvalidOperationException("Запис на курс не знайдено.");

            enrollment.Grade = grade;
        }

        public IReadOnlyList<Student> GetAllStudents() => _repo.GetAllStudents();
    }
}