using System.Collections.Generic;

namespace UniversityApp.Models
{
    /// <summary>
    /// Група. Зв'язок Group -> Student: один-до-багатьох.
    /// </summary>
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Навігаційна властивість: одна група - багато студентів
        public List<Student> Students { get; set; } = new();

        public override string ToString() => $"Група {Name}";
    }
}
