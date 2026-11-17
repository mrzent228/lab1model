using System.Collections.Generic;

namespace UniversityDomainApp.Models
{
    public class Student
    {
        public string FullName { get; set; }
        public Group Group { get; set; }
        public StudentCard Card { get; private set; }
        public List<Enrollment> Enrollments { get; } = new List<Enrollment>();

        public Student(string fullName)
        {
            FullName = fullName;
        }

        public void AssignCard(StudentCard card)
        {
            Card = card;
            card.Owner = this;
        }

        public void EnrollInCourse(Course course, int? grade = null)
        {
            var enrollment = new Enrollment(this, course, grade);
            Enrollments.Add(enrollment);
            course.Enrollments.Add(enrollment);
        }
    }
}