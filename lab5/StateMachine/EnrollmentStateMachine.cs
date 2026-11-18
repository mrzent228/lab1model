using System;
using Lab5_UniversityStates.Models;

namespace Lab5_UniversityStates.StateMachine
{
    public class EnrollmentStateMachine
    {
        private readonly CourseEnrollmentContext _ctx;

        public EnrollmentStateMachine(CourseEnrollmentContext context)
        {
            _ctx = context;
        }

        // Допоміжний метод для виконання переходу з вхідними/вихідними ефектами
        private void TransitionTo(EnrollmentState newState, string trigger, string effectDescription)
        {
            var oldState = _ctx.CurrentState;

            // Вихідний ефект (exit/)
            OnExitState(oldState);

            _ctx.CurrentState = newState;
            string log = $"[Перехід] {oldState} --({trigger}) / {effectDescription}--> {newState}";
            _ctx.TransitionHistory.Add(log);
            Console.WriteLine(log);

            // Вхідний ефект (entry/)
            OnEntryState(newState);
        }

        private void OnExitState(EnrollmentState state)
        {
            if (state == EnrollmentState.Retake)
                Console.WriteLine("   exit / Закриття відомості перескладання");
        }

        private void OnEntryState(EnrollmentState state)
        {
            switch (state)
            {
                case EnrollmentState.Enrolled:
                    Console.WriteLine($"   entry / Відкрито доступ до матеріалів курсу «{_ctx.CourseTitle}»");
                    break;
                case EnrollmentState.Retake:
                    Console.WriteLine("   entry / Призначено дату комісійного перескладання");
                    break;
                case EnrollmentState.Passed:
                    Console.WriteLine($"   entry / Занесено оцінку {_ctx.FinalGrade} балів до електронної залікової книжки");
                    break;
                case EnrollmentState.Expelled:
                    Console.WriteLine("   entry / Сформовано подання на відрахування студента");
                    break;
            }
        }

        // Тригер 1: Підтвердження запису на курс
        public void ConfirmEnrollment()
        {
            Console.WriteLine($"\n> Тригер: ConfirmEnrollment() для {_ctx.StudentName} ({_ctx.CourseTitle})");

            if (_ctx.CurrentState != EnrollmentState.Draft)
            {
                Console.WriteLine($"   [Відхилено] Неможливо підтвердити запис зі стану {_ctx.CurrentState}.");
                return;
            }

            // Захисна умова [Guard]: студентський квиток має бути активним
            if (!_ctx.IsCardActive)
            {
                Console.WriteLine($"   [Guard False] Студентський квиток {_ctx.CardNumber} неактивний! Перехід заблоковано.");
                return;
            }

            TransitionTo(EnrollmentState.Enrolled, "ConfirmEnrollment [IsCardActive]", "зарахуватиНаКурс()");
        }

        // Тригер 2: Складання підсумкового іспиту
        public void SubmitExam(int grade)
        {
            Console.WriteLine($"\n> Тригер: SubmitExam({grade}) для {_ctx.StudentName} ({_ctx.CourseTitle})");

            if (_ctx.CurrentState != EnrollmentState.Enrolled)
            {
                Console.WriteLine($"   [Відхилено] Складання іспиту недоступне у стані {_ctx.CurrentState}.");
                return;
            }

            if (grade < 0 || grade > 100)
            {
                Console.WriteLine("   [Помилка] Некоректний бал (допустимо 0..100).");
                return;
            }

            _ctx.AttemptCount = 1;
            _ctx.FinalGrade = grade;

            // Розгалуження за захисною умовою [grade >= 60] або [grade < 60]
            if (grade >= 60)
            {
                TransitionTo(EnrollmentState.Passed, $"SubmitExam [grade={grade} >= 60]", "зарахуватиДисципліну()");
            }
            else
            {
                TransitionTo(EnrollmentState.Retake, $"SubmitExam [grade={grade} < 60]", "направитиНаПерескладання()");
            }
        }

        // Тригер 3: Перескладання іспиту
        public void SubmitRetake(int grade)
        {
            Console.WriteLine($"\n> Тригер: SubmitRetake({grade}) для {_ctx.StudentName} ({_ctx.CourseTitle})");

            if (_ctx.CurrentState != EnrollmentState.Retake)
            {
                Console.WriteLine($"   [Відхилено] Перескладання можливе лише зі стану Retake (поточний: {_ctx.CurrentState}).");
                return;
            }

            if (grade < 0 || grade > 100)
            {
                Console.WriteLine("   [Помилка] Некоректний бал (допустимо 0..100).");
                return;
            }

            _ctx.AttemptCount++;
            _ctx.FinalGrade = grade;

            if (grade >= 60)
            {
                TransitionTo(EnrollmentState.Passed, $"SubmitRetake [grade={grade} >= 60]", "зарахуватиПісляПерескладання()");
            }
            else
            {
                TransitionTo(EnrollmentState.Expelled, $"SubmitRetake [grade={grade} < 60]", "ініціюватиВідрахування()");
            }
        }

        // Тригер 4: Скасування запису студентом
        public void CancelEnrollment()
        {
            Console.WriteLine($"\n> Тригер: CancelEnrollment() для {_ctx.StudentName} ({_ctx.CourseTitle})");

            if (_ctx.CurrentState != EnrollmentState.Draft && _ctx.CurrentState != EnrollmentState.Enrolled)
            {
                Console.WriteLine($"   [Відхилено] Скасування неможливе у стані {_ctx.CurrentState}.");
                return;
            }

            TransitionTo(EnrollmentState.Cancelled, "CancelEnrollment", "анулюватиЗапис()");
        }
    }
}