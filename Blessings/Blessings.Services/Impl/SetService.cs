using Blessings.Data;
using Blessings.Data.Entities;
using Blessings.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Blessings.Services.Impl
{
    public class SetService : ISetService
    {
        private readonly ApplicationDbContext _context;
        public SetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Set>> GetSetsAsync() =>
               _context.Sets.Include(x=>x.Assortments).OrderByDescending(x => x.Id).ToListAsync();

        public async Task AddSetAsync(Set set)
        {
            _context.Sets.Add(set);
            await _context.SaveChangesAsync();
        }

        public async Task AddAssortmentAsync(Assortment assortment)
        {
            _context.Assortments.Add(assortment);
            await _context.SaveChangesAsync();
        }
        
    }
}
