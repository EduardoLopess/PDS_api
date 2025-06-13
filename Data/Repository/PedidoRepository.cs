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
        public async Task CreatePedidoAsync(Pedido entity)
        {
            _context.Pedidos.Add(entity);
            await
                _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Pedido>> GetAllAsync()
        {
            return await _context.Pedidos
            .Include(p => p.Mesa)
            .Include(p => p.Itens).ThenInclude(i => i.Produto)
            .ToListAsync();
        }

        public async Task<Pedido?> GetByIdAsync(int entityId)
        {
            var pedido = await _context.Pedidos
                        .Include(p => p.Mesa)
                        .Include(p => p.Itens).ThenInclude(i => i.Produto)
                        .SingleOrDefaultAsync(p => p.Id == entityId);
            return pedido;
        }

        public Task UpdateAsync(Pedido entity)
        {
            throw new NotImplementedException();
        }
    }
}