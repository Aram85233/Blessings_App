using Blessings.Data.Entities;

namespace Blessings.Services.Contracts
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployees(bool isBusy);
        Task UpdateEmployees(params Employee[] employees);
    }
}
