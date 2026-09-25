using System.Collections.Generic;

namespace UniversityApp.Models
{
    /// <summary>
    /// Студент.
    /// - Group -> Student: один-до-багатьох (FK GroupId)
    /// - Student -> StudentCard: один-до-одного
    /// - Student <-> Course: багато-до-багатьох (через Enrollment)
    /// </summary>
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        // Зовнішній ключ "один-до-багатьох"
        public int GroupId { get; set; }
        public Group Group { get; set; }

        // Зв'язок "один-до-одного"
        public StudentCard Card { get; set; }

        // Зв'язок "багато-до-багатьох" (через таблицю-зв'язку Enrollment)
        public List<Enrollment> Enrollments { get; set; } = new();

        public override string ToString() => $"{FullName} (ID: {Id})";
    }
}
