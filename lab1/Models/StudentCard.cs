using System;

namespace UniversityApp.Models
{
    /// <summary>
    /// Студентський квиток. Зв'язок Student -> StudentCard: один-до-одного.
    /// StudentId унікальний, тобто у кожного студента рівно один квиток.
    /// </summary>
    public class StudentCard
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string CardNumber { get; set; }
        public DateTime IssueDate { get; set; }

        public override string ToString() => $"Квиток № {CardNumber} (виданий {IssueDate:d})";
    }
}
