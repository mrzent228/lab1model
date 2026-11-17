-- =====================================================================
-- Лабораторна робота №1. Варіант 2 — система автоматизації роботи університету
-- Завдання №1: реляційні таблиці з моделюванням відношень
--   один-до-одного, один-до-багатьох, багато-до-багатьох
-- =====================================================================

-- Довідкова таблиця "Групи"
CREATE TABLE Groups (
    Id    INT PRIMARY KEY IDENTITY(1,1),
    Name  NVARCHAR(50) NOT NULL          -- напр. "КН-21"
);

-- Таблиця "Студенти"
-- Зв'язок Groups -> Students = ОДИН-ДО-БАГАТЬОХ
-- (одна група має багато студентів, студент належить одній групі)
CREATE TABLE Students (
    Id        INT PRIMARY KEY IDENTITY(1,1),
    FullName  NVARCHAR(150) NOT NULL,
    GroupId   INT NOT NULL,
    FOREIGN KEY (GroupId) REFERENCES Groups(Id)
);

-- Таблиця "Студентські квитки"
-- Зв'язок Students -> StudentCards = ОДИН-ДО-ОДНОГО
-- (кожен студент має рівно один студентський квиток, StudentId унікальний)
CREATE TABLE StudentCards (
    Id          INT PRIMARY KEY IDENTITY(1,1),
    StudentId   INT NOT NULL UNIQUE,
    CardNumber  NVARCHAR(20) NOT NULL,
    IssueDate   DATE NOT NULL,
    FOREIGN KEY (StudentId) REFERENCES Students(Id)
);

-- Таблиця "Курси (дисципліни)"
CREATE TABLE Courses (
    Id       INT PRIMARY KEY IDENTITY(1,1),
    Title    NVARCHAR(150) NOT NULL,
    Credits  INT NOT NULL
);

-- Таблиця-зв'язка "Записи на курси"
-- Зв'язок Students <-> Courses = БАГАТО-ДО-БАГАТЬОХ
-- (студент може відвідувати багато курсів, курс може відвідувати багато студентів)
CREATE TABLE Enrollments (
    StudentId  INT NOT NULL,
    CourseId   INT NOT NULL,
    Grade      INT NULL,
    PRIMARY KEY (StudentId, CourseId),
    FOREIGN KEY (StudentId) REFERENCES Students(Id),
    FOREIGN KEY (CourseId)  REFERENCES Courses(Id)
);
