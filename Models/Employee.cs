using System;
using TestApp.Models.Enums;

namespace TestApp.Models
{
    public class Employee
    {
        public virtual int Id { get; set; }
        public virtual string FullName { get; set; } = string.Empty;
        public virtual Position Position { get; set; }
        public virtual DateTime BirthDate { get; set; }
    }
}
