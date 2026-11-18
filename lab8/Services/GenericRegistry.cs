using System.Collections.Generic;
using Lab8_UniversityClasses.Interfaces;

namespace Lab8_UniversityClasses.Services
{
    // Узагальнений (шаблонний) клас з обмеженням типу where T : IReportable
    public class UniversityRegistry<T> where T : IReportable
    {
        private readonly List<T> _items = new List<T>();

        public void Register(T item) => _items.Add(item);
        public IReadOnlyList<T> GetAll() => _items;
        public int Count => _items.Count;
    }
}