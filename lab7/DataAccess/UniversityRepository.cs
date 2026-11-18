using System.Collections.Generic;
using System.Linq;
using Lab7_UniversityPackages.Domain;

namespace Lab7_UniversityPackages.DataAccess
{
    public interface IUniversityRepository
    {
        void AddGroup(Group group);
        Group GetGroup(string name);
        void AddStudent(Student student);
        Student GetStudent(int id);
        IReadOnlyList<Student> GetAllStudents();
        void AddCourse(Course course);
        Course GetCourse(int id);
    }

    public class InMemoryUniversityRepository : IUniversityRepository
    {
        private readonly List<Group> _groups = new List<Group>();
        private readonly List<Student> _students = new List<Student>();
        private readonly List<Course> _courses = new List<Course>();

        public void AddGroup(Group group) => _groups.Add(group);
        public Group GetGroup(string name) => _groups.FirstOrDefault(g => g.Name == name);

        public void AddStudent(Student student) => _students.Add(student);
        public Student GetStudent(int id) => _students.FirstOrDefault(s => s.Id == id);
        public IReadOnlyList<Student> GetAllStudents() => _students;

        public void AddCourse(Course course) => _courses.Add(course);
        public Course GetCourse(int id) => _courses.FirstOrDefault(c => c.Id == id);
    }
}