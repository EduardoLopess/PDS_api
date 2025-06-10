using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class DrinkRepository : IDrinkRepository
    {
        private readonly DataContext _context;
        public DrinkRepository(DataContext context)
        {
            _context = context;
        }
        public async Task CreateDrinkAsync(Drink entity)
        {
            _context.Drinks.Add(entity);
            await
                _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int entityId)
        {
            var drink = await _context.Drinks.FindAsync(entityId);
            if (drink != null)
            {
                _context.Drinks.Remove(drink);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IList<Drink>> GetAllAsync()
        {
            return await _context.Drinks
                .Include(s => s.Sabores)
                .ToListAsync();
        }

        public async Task<Drink?> GetByIdAsync(int entityId)
        {
            return
                await _context.Drinks
                    .Include(s => s.Sabores)
                    .SingleOrDefaultAsync(d => d.Id == entityId);
        }

        public Task UpdateAsync(Drink entity)
        {
            throw new NotImplementedException();
        }
    }
}