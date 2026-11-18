namespace Lab8_UniversityClasses.Interfaces
{
    // Інтерфейс для об'єктів, що підлягають оцінюванню
    public interface IGradable
    {
        int? Grade { get; }
        string GetEctsLetter();
    }

    // Інтерфейс для формування текстового представлення у звітах
    public interface IReportable
    {
        string GetSummaryLine();
    }
}