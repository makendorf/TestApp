using FluentNHibernate.Mapping;
using TestApp.Models;

namespace TestApp.Mappings
{
    public class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            Table("Orders");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Date).Not.Nullable();
            Map(x => x.Amount).Precision(18).Scale(2).Not.Nullable();
            References(x => x.Employee).Column("EmployeeId").Not.Nullable();
            References(x => x.Counterparty).Column("CounterpartyId").Not.Nullable();
        }
    }
}
