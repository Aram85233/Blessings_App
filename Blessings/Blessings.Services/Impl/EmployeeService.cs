using Blessings.Data;
using Blessings.Data.Entities;
using Blessings.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Blessings.Services.Impl
{
    public class EmployeeService : IEmployeeService
    {
        public ApplicationDbContext _context { get; set; }
        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Employee>> GetEmployees(bool isBusy) =>
             _context.Employees.Where(x => x.IsBusy == isBusy && x.IsActive == true).ToListAsync();

        public async Task UpdateEmployees(params Employee[] employees)
        {
            _context.Employees.UpdateRange(employees);
            await _context.SaveChangesAsync();
        }
             

    }
}
