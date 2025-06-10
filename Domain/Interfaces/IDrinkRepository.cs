using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDrinkRepository : IBaseRepository<Drink>
    {
        Task CreateDrinkAsync(Drink entity);
    }
}