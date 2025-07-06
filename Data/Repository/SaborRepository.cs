using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class SaborRepository : ISaborRepository
    {
        private readonly DataContext _context;
        public SaborRepository(DataContext context)
        {
            _context = context;
        }
        public async Task CreateSabor(Sabor entity)
        {
            _context.Sabores.Add(entity);
            await
                _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int entityId)
        {
            var existeSabor = await _context.Sabores
                .FirstOrDefaultAsync(s => s.Id == entityId);

            if (existeSabor != null)
            {
                _context.Sabores.Remove(existeSabor);
                await
                    _context.SaveChangesAsync();
            }
        }

        public async Task<IList<Sabor>> GetAllAsync()
        {
            return
                await _context.Sabores
                    .ToListAsync();
        }

        public async Task<Sabor?> GetByIdAsync(int entityId)
        {
            var sabor = await _context.Sabores
                .SingleOrDefaultAsync(s => s.Id == entityId);

            return sabor;
        }
    
        public async Task UpdateAsync(Sabor entity)
        {
            var existeSabor = await _context.Sabores
                .FirstOrDefaultAsync(s => s.Id == entity.Id);

            if (existeSabor != null)
            {
                _context.Entry(existeSabor).CurrentValues.SetValues(entity);
                await
                    _context.SaveChangesAsync();
            }
        }

        //EXTRA
        public async Task<bool> MudarDisponibilidadeAsync(int entityId)
        {
            var sabor = await _context.Sabores.FirstOrDefaultAsync(s => s.Id == entityId);
            if (sabor == null)
                return false;

            sabor.Disponivel = !sabor.Disponivel;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}