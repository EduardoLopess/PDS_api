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
            await _context.SaveChangesAsync();
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
                .Include(p => p.Itens).ThenInclude(i => i.ItemAdicionais).ThenInclude(ia => ia.Adicional)
                .Include(p => p.Itens).ThenInclude(i => i.SaborDrink)
                .ToListAsync();
        }

        public async Task<Pedido?> GetByIdAsync(int entityId)
        {
            var pedido = await _context.Pedidos
                        .Include(p => p.Mesa)
                        .Include(p => p.Itens).ThenInclude(i => i.Produto)
                        .Include(p => p.Itens).ThenInclude(i => i.ItemAdicionais).ThenInclude(ia => ia.Adicional)
                        .Include(p => p.Itens).ThenInclude(i => i.SaborDrink)
                        .SingleOrDefaultAsync(p => p.Id == entityId);
            return pedido;
        }

        public async Task UpdateAsync(Pedido entity)
        {
            var pedidoExiste = await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == entity.Id);
            if (pedidoExiste != null)
            {
                _context.Entry(pedidoExiste).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Pedido?> BuscarPedidoAtivoMesaAsync(int mesaId)
        {
            return await _context.Pedidos
                .Include(p => p.Itens).ThenInclude(i => i.Produto)
                .Include(p => p.Itens).ThenInclude(i => i.ItemAdicionais).ThenInclude(ia => ia.Adicional)
                .Include(p => p.Itens).ThenInclude(i => i.SaborDrink)
                .Include(p => p.Mesa)
                .FirstOrDefaultAsync(p => p.Mesa.Id == mesaId);
        }

        public async Task<bool> StatusPedido(int id)
        {
            var pedidoEmEdicao = await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == id);
            return pedidoEmEdicao?.StatusPedido == true;
        }

        public async Task<bool> MudarSatusPedido(int entityId)
        {
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == entityId);

            if (pedido == null)
                return false;

            pedido.StatusPedido = !pedido.StatusPedido;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TentarBloquearPedidoAsync(int pedidoId, string connectionIdDoCliente)
        {
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null)
            {
                // Pedido não encontrado, não pode ser bloqueado
                return false;
            }

            // Cenário 1: Pedido já está bloqueado (StatusPedido == true)
            if (pedido.StatusPedido == true)
            {
                // Se já está bloqueado pelo MESMO CLIENTE, consideramos sucesso (já está bloqueado)
                if (pedido.ConnectionId == connectionIdDoCliente)
                {
                    return true;
                }
                else
                {
                    // Se já está bloqueado por OUTRO CLIENTE, não podemos bloquear.
                    // O Controller deve tratar isso como um conflito.
                    return false; // Indica que o bloqueio não foi possível
                }
            }

            // Cenário 2: Pedido está disponível (StatusPedido == false)
            // Tenta bloquear o pedido
            pedido.StatusPedido = true; // Altera o status para "Em Edição" (bloqueado)
            pedido.ConnectionId = connectionIdDoCliente; // Atribui o ID da conexão que o bloqueou

            try
            {
                await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
                return true; // Bloqueio bem-sucedido
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Isso pode ocorrer se outro cliente tentou bloquear ao mesmo tempo.
                // Opcional: Logar a exceção para depuração.
                Console.WriteLine($"Erro de concorrência ao tentar bloquear pedido {pedidoId}: {ex.Message}");
                return false; // Bloqueio falhou devido a concorrência
            }
            catch (Exception ex)
            {
                // Captura outras exceções inesperadas durante o SaveChanges.
                // Opcional: Logar a exceção completa.
                Console.WriteLine($"Erro inesperado ao bloquear pedido {pedidoId}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DesbloquearPedidoAsync(int pedidoId, string connectionIdDoCliente)
        {
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null)
            {
                // Pedido não encontrado
                return false;
            }

            // Cenário 1: Pedido não está bloqueado (StatusPedido == false)
            if (pedido.StatusPedido == false)
            {
                
                return true;
            }

            
            if (pedido.ConnectionId != connectionIdDoCliente)
            {
            
                return false;
            }

           
            pedido.StatusPedido = false; 
            pedido.ConnectionId = null;

            try
            {
                await _context.SaveChangesAsync();
                return true; 
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Erro de concorrência ao tentar desbloquear pedido {pedidoId}: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado ao desbloquear pedido {pedidoId}: {ex.Message}");
                return false;
            }
        }

        // public Task<Pedido> AtualizarMesaPedido(int pedidoId, int mesaId)
        // {
        //     var pedido = _context.Pedidos.FirstOrDefaultAsync(p => p.Id == pedidoId);
        //     var mesa = _context.Pedidos.FirstOrDefaultAsync(m => m.Mesa.Id == mesaId);
            

        //     if (mesa != null && pedido != null)
        //     {

        //     }
        // }
    }
}
