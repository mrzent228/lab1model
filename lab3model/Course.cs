using System.Collections.Generic;

namespace UniversityDomainApp.Models
{
    public class Course
    {
        public string Title { get; set; }
        public int Credits { get; set; }
        public List<Enrollment> Enrollments { get; } = new List<Enrollment>();

        public Course(string title, int credits)
        {
            Title = title;
            Credits = credits;
        }
    }
}