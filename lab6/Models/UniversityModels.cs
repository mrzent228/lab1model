using System;

namespace Lab6_UniversitySequence.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string GroupName { get; set; }
        public StudentCard Card { get; set; }

        public Student(int id, string fullName, string groupName)
        {
            Id = id;
            FullName = fullName;
            GroupName = groupName;
        }
    }

    public class StudentCard
    {
        public string CardNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IsActive { get; set; } = true;

        public StudentCard(string cardNumber, DateTime issueDate)
        {
            CardNumber = cardNumber;
            IssueDate = issueDate;
        }
    }

    public class EnrollmentRecord
    {
        public Student Student { get; set; }
        public string CourseTitle { get; set; }
        public int Credits { get; set; }
        public DateTime EnrolledAt { get; set; }

        public EnrollmentRecord(Student student, string courseTitle, int credits)
        {
            Student = student;
            CourseTitle = courseTitle;
            Credits = credits;
            EnrolledAt = DateTime.Now;
        }
    }
}