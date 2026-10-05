using System;

namespace Lb2
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => $"[Категорія №{Id}] {Name}";
    }
}