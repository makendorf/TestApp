using FluentNHibernate.Mapping;
using TestApp.Models;

namespace TestApp.Mappings
{
    public class CounterpartyMap : ClassMap<Counterparty>
    {
        public CounterpartyMap()
        {
            Table("Counterparties");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Length(255).Not.Nullable();
            Map(x => x.Inn).Length(12).Not.Nullable();
            References(x => x.Curator).Column("CuratorId").Not.Nullable();
        }
    }
}
