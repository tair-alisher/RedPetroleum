using System;
using System.Threading.Tasks;
using RedPetroleum.Models.Repositories;

namespace RedPetroleum.Models.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        DepartmentRepository Departments { get; }
        EmployeeRepository Employees { get; }
        PositionRepository Positions { get; }
        TaskListRepository TaskLists { get; }
        void Save();
        Task SaveAsync();
    }
}
