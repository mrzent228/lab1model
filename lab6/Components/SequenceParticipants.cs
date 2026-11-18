using System;
using System.Collections.Generic;
using System.Linq;
using Lab6_UniversitySequence.Models;

namespace Lab6_UniversitySequence.Components
{
    // Лінія життя 2: Репозиторій студентів
    public class StudentRepository
    {
        private readonly List<Student> _students = new List<Student>();
        private int _nextId = 1;

        public Student SaveStudent(string fullName, string groupName)
        {
            Console.WriteLine($"   [DeaneryController -> StudentRepository]: SaveStudent(\"{fullName}\", \"{groupName}\")");
            var student = new Student(_nextId++, fullName, groupName);
            _students.Add(student);
            Console.WriteLine($"   [StudentRepository --> DeaneryController]: повернено Student (Id={student.Id})");
            return student;
        }

        public Student FindById(int id) => _students.FirstOrDefault(s => s.Id == id);
    }

    // Лінія життя 3: Сервіс студентських квитків
    public class CardService
    {
        private int _cardCounter = 1001;

        public StudentCard IssueCard(Student student)
        {
            Console.WriteLine($"   [DeaneryController -> CardService]: IssueCard(StudentId={student.Id})");
            var card = new StudentCard($"ST-{_cardCounter++}", new DateTime(2024, 9, 1));
            student.Card = card;
            Console.WriteLine($"   [CardService --> DeaneryController]: повернено StudentCard (№ {card.CardNumber})");
            return card;
        }
    }

    // Лінія життя 4: Сервіс запису на навчальні курси
    public class CourseEnrollmentService
    {
        private readonly List<EnrollmentRecord> _records = new List<EnrollmentRecord>();

        public EnrollmentRecord Enroll(Student student, string courseTitle, int credits)
        {
            Console.WriteLine($"   [DeaneryController -> CourseEnrollmentService]: Enroll(StudentId={student.Id}, \"{courseTitle}\")");

            // Комбінований фрагмент alt [student.Card != null && student.Card.IsActive]
            if (student.Card == null || !student.Card.IsActive)
            {
                Console.WriteLine("   [CourseEnrollmentService --> DeaneryController]: Відмова (відсутній активний квиток)");
                return null;
            }

            if (_records.Any(r => r.Student.Id == student.Id && r.CourseTitle == courseTitle))
            {
                Console.WriteLine($"   [CourseEnrollmentService --> DeaneryController]: Відмова (вже записаний на «{courseTitle}»)");
                return null;
            }

            var record = new EnrollmentRecord(student, courseTitle, credits);
            _records.Add(record);
            Console.WriteLine($"   [CourseEnrollmentService --> DeaneryController]: Запис підтверджено (EnrollmentRecord)");
            return record;
        }

        public IReadOnlyList<EnrollmentRecord> GetAllRecords() => _records;
    }

    // Лінія життя 1: Контролер деканату (координує послідовність викликів)
    public class DeaneryController
    {
        private readonly StudentRepository _studentRepo;
        private readonly CardService _cardService;
        private readonly CourseEnrollmentService _enrollmentService;

        public DeaneryController(StudentRepository studentRepo, CardService cardService, CourseEnrollmentService enrollmentService)
        {
            _studentRepo = studentRepo;
            _cardService = cardService;
            _enrollmentService = enrollmentService;
        }

        // Головний сценарій взаємодії (Sequence Flow)
        public void RegisterAndEnrollStudent(string fullName, string groupName, string courseTitle, int credits)
        {
            Console.WriteLine($"\n[Методист деканату -> DeaneryController]: RegisterAndEnrollStudent(\"{fullName}\", \"{groupName}\", \"{courseTitle}\")");

            // Крок 1: Збереження студента
            Student student = _studentRepo.SaveStudent(fullName, groupName);

            // Крок 2: Оформлення студентського квитка
            StudentCard card = _cardService.IssueCard(student);

            // Крок 3: Запис на навчальний курс
            EnrollmentRecord record = _enrollmentService.Enroll(student, courseTitle, credits);

            if (record != null)
                Console.WriteLine($"[DeaneryController --> Методист деканату]: Успіх! {student.FullName} (квиток {card.CardNumber}) зарахований на курс «{courseTitle}».");
            else
                Console.WriteLine($"[DeaneryController --> Методист деканату]: Помилка зарахування на курс «{courseTitle}».");
        }

        // Додатковий запис існуючого студента (перевірка гілки alt при дублюванні)
        public void EnrollExistingStudent(int studentId, string courseTitle, int credits)
        {
            Console.WriteLine($"\n[Методист деканату -> DeaneryController]: EnrollExistingStudent(Id={studentId}, \"{courseTitle}\")");
            var student = _studentRepo.FindById(studentId);
            if (student == null)
            {
                Console.WriteLine("[DeaneryController --> Методист деканату]: Студента не знайдено.");
                return;
            }

            var record = _enrollmentService.Enroll(student, courseTitle, credits);
            if (record != null)
                Console.WriteLine($"[DeaneryController --> Методист деканату]: Успіх! Додано курс «{courseTitle}» для {student.FullName}.");
            else
                Console.WriteLine($"[DeaneryController --> Методист деканату]: Операцію відхилено.");
        }
    }
}