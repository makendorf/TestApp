using System;
using TestApp.Models.Enums;

namespace TestApp.Models
{
    /// <summary>
    /// Модель данных, представляющая сотрудника организации.
    /// Хранит основную информацию: ФИО, должность и дату рождения.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Уникальный идентификатор сотрудника (генерируется автоматически БД).
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Полное ФИО сотрудника.
        /// </summary>
        public virtual string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Должность сотрудника (перечисление Position).
        /// </summary>
        public virtual Position Position { get; set; }

        /// <summary>
        /// Дата рождения сотрудника.
        /// </summary>
        public virtual DateTime BirthDate { get; set; }
    }
}
