using FluentNHibernate.Mapping;
using TestApp.Models;

namespace TestApp.Mappings
{
    /// <summary>
    /// Маппинг NHibernate для модели Order.
    /// Определяет отображение свойств заказа на таблицу «Orders».
    /// </summary>
    public class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            Table("Orders");

            // Первичный ключ с автогенерацией через Identity
            Id(x => x.Id).GeneratedBy.Identity();

            // Дата заказа — обязательное поле
            Map(x => x.Date).Not.Nullable();

            // Сумма заказа — точность 18 цифр, 2 знака после запятой
            Map(x => x.Amount).Precision(18).Scale(2).Not.Nullable();

            // Внешний ключ на сотрудника
            References(x => x.Employee).Column("EmployeeId").Not.Nullable();

            // Внешний ключ на контрагента
            References(x => x.Counterparty).Column("CounterpartyId").Not.Nullable();
        }
    }
}
