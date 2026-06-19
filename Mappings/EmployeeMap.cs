using FluentNHibernate.Mapping;
using TestApp.Models;

namespace TestApp.Mappings
{
    /// <summary>
    /// Маппинг NHibernate для модели Employee.
    /// Определяет отображение свойств сотрудника на таблицу «Employees».
    /// </summary>
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("Employees");

            // Первичный ключ с автогенерацией через Identity
            Id(x => x.Id).GeneratedBy.Identity();

            // ФИО — строка до 255 символов, обязательное поле
            Map(x => x.FullName).Length(255).Not.Nullable();

            // Должность — хранится как целое число (значение перечисления)
            Map(x => x.Position).CustomType<int>().Not.Nullable();

            // Дата рождения — обязательное поле
            Map(x => x.BirthDate).Not.Nullable();
        }
    }
}
