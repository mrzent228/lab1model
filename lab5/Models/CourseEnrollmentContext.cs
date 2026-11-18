using System;
using System.Collections.Generic;

namespace Lab5_UniversityStates.Models
{
    // Стани скінченного автомата (UML State Machine)
    public enum EnrollmentState
    {
        Draft,      // Чернетка заявки
        Enrolled,   // Зараховано на курс (активне навчання)
        Retake,     // На перескладанні (академічна заборгованість)
        Passed,     // Успішно складено (кінцевий стан)
        Expelled,   // Відраховано за неуспішність (кінцевий стан)
        Cancelled   // Скасовано за власним бажанням (кінцевий стан)
    }

    // Контекст скінченного автомата (зберігає поточний стан та атрибути)
    public class CourseEnrollmentContext
    {
        public string StudentName { get; }
        public string GroupName { get; }
        public string CardNumber { get; }
        public bool IsCardActive { get; }
        public string CourseTitle { get; }

        public EnrollmentState CurrentState { get; set; }
        public int? FinalGrade { get; set; }
        public int AttemptCount { get; set; } = 0;
        public List<string> TransitionHistory { get; } = new List<string>();

        public CourseEnrollmentContext(string studentName, string groupName, string cardNumber, bool isCardActive, string courseTitle)
        {
            StudentName = studentName;
            GroupName = groupName;
            CardNumber = cardNumber;
            IsCardActive = isCardActive;
            CourseTitle = courseTitle;
            CurrentState = EnrollmentState.Draft;
            TransitionHistory.Add($"[Init] Створено контекст у стані: {CurrentState}");
        }
    }
}