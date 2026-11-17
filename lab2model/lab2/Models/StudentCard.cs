using System;

namespace UniversityDomainApp.Models
{
    public class StudentCard
    {
        public string CardNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public Student Owner { get; set; }

        public StudentCard(string cardNumber, DateTime issueDate)
        {
            CardNumber = cardNumber;
            IssueDate = issueDate;
        }
    }
}