using System.Collections.Generic;

namespace UniversityDomainApp.Models
{
    public class Group
    {
        public string Name { get; set; }
        public List<Student> Students { get; } = new List<Student>();

        public Group(string name)
        {
            Name = name;
        }

        public void AddStudent(Student student)
        {
            student.Group = this;
            Students.Add(student);
        }
    }
}