using Blessings.Data.Entities;

namespace Blessings.Services.Contracts
{
    public interface ISetService
    {
        Task<List<Set>> GetSetsAsync();
        Task AddSetAsync(Set set);
        Task AddAssortmentAsync(Assortment assortment);
    }
}
