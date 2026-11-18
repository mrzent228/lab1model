using System;
using System.Collections.Generic;

namespace Lab4_University.Models
{
    public class Group
    {
        public string Name { get; }
        public List<Student> Students { get; } = new List<Student>();

        public Group(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Назва групи не може бути порожньою.");
            Name = name.Trim();
        }
    }

    public class StudentCard
    {
        public string CardNumber { get; }
        public DateTime IssueDate { get; }
        public bool IsVerifiedInEdebo { get; set; }

        public StudentCard(string cardNumber, DateTime issueDate)
        {
            CardNumber = cardNumber;
            IssueDate = issueDate;
        }
    }

    public class Student
    {
        public int Id { get; }
        public string FullName { get; }
        public string Email { get; }
        public Group Group { get; }
        public StudentCard Card { get; set; }
        public List<Enrollment> Enrollments { get; } = new List<Enrollment>();

        public Student(int id, string fullName, string email, Group group)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            Group = group;
        }
    }

    public class Course
    {
        public int Id { get; }
        public string Title { get; }
        public int Credits { get; }

        public Course(int id, string title, int credits)
        {
            if (credits <= 0 || credits > 15)
                throw new ArgumentException("Кількість кредитів має бути в межах 1–15 (Бізнес-правило BR-03).");

            Id = id;
            Title = title;
            Credits = credits;
        }
    }

    public class Enrollment
    {
        public Student Student { get; }
        public Course Course { get; }
        public int? Grade { get; set; }

        public string EctsGrade => Grade switch
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

        public Enrollment(Student student, Course course)
        {
            Student = student;
            Course = course;
        }
    }
}