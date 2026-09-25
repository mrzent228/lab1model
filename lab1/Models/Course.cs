    using System.Collections.Generic;

    namespace UniversityApp.Models
    {
        /// <summary>
        /// Навчальна дисципліна. Зв'язок Course <-> Student: багато-до-багатьох.
        /// </summary>
        public class Course
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int Credits { get; set; }

            public List<Enrollment> Enrollments { get; set; } = new();

            public override string ToString() => $"{Title} ({Credits} кредитів)";
        }
    }
