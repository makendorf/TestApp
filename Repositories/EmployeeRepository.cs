using System.Collections.Generic;
using NHibernate;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee> { }

    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ISessionFactoryProvider sessionProvider) : base(sessionProvider) { }

        public override IEnumerable<Employee> GetAll()
        {
            using var session = SessionProvider.OpenSession();
            return session.QueryOver<Employee>().List<Employee>();
        }
    }
}
