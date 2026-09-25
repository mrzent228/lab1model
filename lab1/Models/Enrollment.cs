namespace UniversityApp.Models
{
    /// <summary>
    /// Таблиця-зв'язка для реалізації відношення багато-до-багатьох
    /// між Student та Course (запис студента на курс, оцінка).
    /// </summary>
    public class Enrollment
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int? Grade { get; set; }
    }
}
