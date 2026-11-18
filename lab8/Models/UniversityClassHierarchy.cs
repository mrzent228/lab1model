using System;
using System.Collections.Generic;
using Lab8_UniversityClasses.Interfaces;

namespace Lab8_UniversityClasses.Models
{
    // Абстрактний базовий клас (Generalization)
    public abstract class Person : IReportable
    {
        private static int _idCounter = 1; // Статичне поле

        public int Id { get; }
        public string FullName { get; protected set; }
        public string Email { get; protected set; }

        protected Person(string fullName, string email)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("ПІБ не може бути порожнім.");

            Id = _idCounter++;
            FullName = fullName.Trim();
            Email = email.Trim();
        }

        public abstract string GetRole(); // Поліморфний метод
        public virtual string GetSummaryLine() => $"[{GetRole()}] {FullName} ({Email})";
    }

    // Клас-спадкоємець 1: Викладач
    public class Teacher : Person
    {
        public string AcademicDegree { get; }
        public string Department { get; }

        public Teacher(string fullName, string email, string academicDegree, string department)
            : base(fullName, email)
        {
            AcademicDegree = academicDegree;
            Department = department;
        }

        public override string GetRole() => "Викладач";
        public override string GetSummaryLine() => $"{base.GetSummaryLine()} | {AcademicDegree}, каф. {Department}";
    }

    // Композитна частина: Студентський квиток (створюється всередині Student)
    public class StudentCard
    {
        public string CardNumber { get; }
        public DateTime IssueDate { get; }
        public bool IsActive { get; private set; }

        internal StudentCard(string cardNumber, DateTime issueDate)
        {
            CardNumber = cardNumber;
            IssueDate = issueDate;
            IsActive = true;
        }

        public void Deactivate() => IsActive = false;
    }

    // Клас-спадкоємець 2: Студент (Композиція зі StudentCard)
    public class Student : Person
    {
        public AcademicGroup Group { get; private set; }
        public StudentCard Card { get; } // Композиція 1..1
        public List<Enrollment> Enrollments { get; } = new List<Enrollment>();

        public Student(string fullName, string email, string cardNumber, AcademicGroup group)
            : base(fullName, email)
        {
            Card = new StudentCard(cardNumber, new DateTime(2024, 9, 1));
            Group = group;
            group.AddStudent(this);
        }

        public override string GetRole() => "Студент";
        public override string GetSummaryLine() => $"{base.GetSummaryLine()} | Група: {Group.Name} | Квиток: {Card.CardNumber}";
    }

    // Агрегація: Академічна група об'єднує студентів
    public class AcademicGroup
    {
        public string Name { get; }
        public string Specialty { get; }
        private readonly List<Student> _students = new List<Student>();

        public IReadOnlyList<Student> Students => _students;

        public AcademicGroup(string name, string specialty)
        {
            Name = name;
            Specialty = specialty;
        }

        public void AddStudent(Student student)
        {
            if (!_students.Contains(student))
                _students.Add(student);
        }
    }

    // Навчальна дисципліна (Асоціація з Teacher)
    public class Course
    {
        public int Id { get; }
        public string Title { get; }
        public int Credits { get; }
        public Teacher Lecturer { get; }

        public Course(int id, string title, int credits, Teacher lecturer)
        {
            Id = id;
            Title = title;
            Credits = credits;
            Lecturer = lecturer;
        }
    }

    // Асоціативний клас успішності (Реалізує IGradable та IReportable)
    public class Enrollment : IGradable, IReportable
    {
        public Student Student { get; }
        public Course Course { get; }
        public int? Grade { get; private set; }

        public Enrollment(Student student, Course course)
        {
            Student = student;
            Course = course;
        }

        public void SetGrade(int grade)
        {
            if (grade < 0 || grade > 100)
                throw new ArgumentOutOfRangeException(nameof(grade), "Оцінка має бути від 0 до 100.");
            Grade = grade;
        }

        public string GetEctsLetter() => Grade switch
        {
            null => "—",
            >= 90 => "A",
            >= 82 => "B",
            >= 74 => "C",
            >= 64 => "D",
            >= 60 => "E",
            >= 35 => "FX",
            _ => "F"
        };

        public string GetSummaryLine() =>
            $"{Student.FullName,-18} | Курс: {Course.Title,-16} ({Course.Credits} кред.) | Викладач: {Course.Lecturer.FullName,-16} | Бал: {Grade} [{GetEctsLetter()}]";
    }
}