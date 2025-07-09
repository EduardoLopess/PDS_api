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

        public async Task DeleteAsync(int entityId)
        {
            var pedido = await _context.Pedidos.FindAsync(entityId);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IList<Pedido>> GetAllAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Mesa)
                .Include(p => p.Itens).ThenInclude(i => i.Produto)
                .Include(p => p.Itens).ThenInclude(i => i.Adicionals)
                .Include(p => p.Itens).ThenInclude(i => i.SaborDrink)
                .ToListAsync();

        }

        public async Task<Pedido?> GetByIdAsync(int entityId)
        {
            var pedido = await _context.Pedidos
                        .Include(p => p.Mesa)
                        .Include(p => p.Itens).ThenInclude(i => i.Produto)
                        .Include(a => a.Itens).ThenInclude(a => a.Adicionals)
                        .Include(s => s.Itens).ThenInclude(s => s.SaborDrink)
                        .SingleOrDefaultAsync(p => p.Id == entityId);
            return pedido;
        }

        public async Task UpdateAsync(Pedido entity)
        {
            var pedidoExiste = await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == entity.Id);

            if (pedidoExiste != null)
            {
                _context.Entry(pedidoExiste).CurrentValues.SetValues(entity);
                await
                    _context.SaveChangesAsync();
            }
        }

        public async  Task<Pedido?> BuscarPedidoAtivoMesaAsync(int mesaId)
        {
            return await _context.Pedidos
                .Include(p => p.Itens).ThenInclude(i => i.Produto)
                .Include(p => p.Itens).ThenInclude(i => i.Adicionals)
                .Include(p => p.Itens).ThenInclude(i => i.SaborDrink)
                .Include(p => p.Mesa)
                .FirstOrDefaultAsync(p => p.Mesa.Id == mesaId );
        }
    }
}