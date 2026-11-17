using System;
using System.Collections.Generic;

namespace UniversityUseCaseApp.Models
{
	public class Group
	{
		public string Name { get; set; }
		public List<Student> Students { get; } = new List<Student>();
		public Group(string name) => Name = name;
	}

	public class StudentCard
	{
		public string CardNumber { get; set; }
		public DateTime IssueDate { get; set; }
		public bool IsActive { get; set; } = true;

		public StudentCard(string cardNumber, DateTime issueDate)
		{
			CardNumber = cardNumber;
			IssueDate = issueDate;
		}
	}

	public class Student
	{
		public int Id { get; set; }
		public string FullName { get; set; }
		public Group Group { get; set; }
		public StudentCard Card { get; set; }
		public List<Enrollment> Enrollments { get; } = new List<Enrollment>();

		public Student(int id, string fullName, Group group)
		{
			Id = id;
			FullName = fullName;
			Group = group;
		}
	}

	public class Course
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public int Credits { get; set; }
		public List<Enrollment> Enrollments { get; } = new List<Enrollment>();

		public Course(int id, string title, int credits)
		{
			Id = id;
			Title = title;
			Credits = credits;
		}
	}

	public class Enrollment
	{
		public Student Student { get; set; }
		public Course Course { get; set; }
		public int? Grade { get; set; }

		public Enrollment(Student student, Course course)
		{
			Student = student;
			Course = course;
		}
	}
}