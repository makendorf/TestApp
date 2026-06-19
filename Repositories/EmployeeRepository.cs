using System.Collections.Generic;
using NHibernate;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Repositories
{
    /// <summary>
    /// Интерфейс репозитория для работы с сотрудниками.
    /// Наследует общие CRUD-операции от IRepository&lt;Employee&gt;.
    /// </summary>
    public interface IEmployeeRepository : IRepository<Employee> { }

    /// <summary>
    /// Репозиторий для работы с сотрудниками.
    /// Переопределяет GetAll для корректной загрузки связей.
    /// </summary>
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ISessionFactoryProvider sessionProvider) : base(sessionProvider) { }

        /// <summary>
        /// Получить всех сотрудников.
        /// </summary>
        public override IEnumerable<Employee> GetAll()
        {
            using var session = SessionProvider.OpenSession();
            return session.QueryOver<Employee>().List<Employee>();
        }
    }
}
