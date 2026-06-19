using FluentNHibernate.Mapping;
using TestApp.Models;

namespace TestApp.Mappings
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("Employees");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.FullName).Length(255).Not.Nullable();
            Map(x => x.Position).CustomType<int>().Not.Nullable();
            Map(x => x.BirthDate).Not.Nullable();
        }
    }
}
