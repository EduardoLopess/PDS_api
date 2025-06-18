using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Repository
{
    public class AdicionalRepository : IAdicionalRepository
    {
        private readonly DataContext _context;
        public AdicionalRepository(DataContext context)
        {
            _context = context;
        }
        public async Task CreateAdicionalAsync(Adicional entity)
        {
            _context.Adicionals.Add(entity);
            await
                _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int entityId)
        {
            var adicional = await _context.Adicionals.FindAsync(entityId);
            if (adicional != null)
            {
                _context.Adicionals.Remove(adicional);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IList<Adicional>> GetAllAsync()
        {
            return await _context.Adicionals
                    .ToListAsync();
        }

        public async Task<Adicional?> GetByIdAsync(int entityId)
        {
            return
                await _context.Adicionals
                    .SingleOrDefaultAsync(a => a.Id == entityId);

        }

        public async Task UpdateAsync(Adicional entity)
        {
            var existeAdicional = await _context.Adicionals
                .FirstOrDefaultAsync(a => a.Id == entity.Id);

            if (existeAdicional != null)
            {
                _context.Entry(existeAdicional).CurrentValues.SetValues(entity);
                await
                    _context.SaveChangesAsync();
            }
        }

       

        public async Task<List<Adicional>> BuscarAdicionaisAsync(List<int> adicionalIds)
        {
            return await _context.Adicionals
                    .Where(a => adicionalIds.Contains(a.Id))
                    .ToListAsync();
        }
    }
}