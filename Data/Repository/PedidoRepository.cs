using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly DataContext _context;
        public PedidoRepository(DataContext context)
        {
            _context = context;
        }
        public async Task CreatePedidoAsync(Pedido entity, IList<int> ItemIds)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Pedido>> GetAllAsync()
        {
            return await _context.Pedidos.ToListAsync();
        }

        public Task<Pedido> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Pedido entity)
        {
            throw new NotImplementedException();
        }
    }
}