using System;

namespace Lb2
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }

        public Customer(int id, string fullName, string phone)
        {
            Id = id;
            FullName = fullName;
            Phone = phone;
        }

        public override string ToString() => $"{FullName} (Тел: {Phone})";
    }
}