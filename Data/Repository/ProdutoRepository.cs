using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly DataContext _context;
        public ProdutoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Produto entity)
        {
            _context.Produtos.Add(entity);
            await
                _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int entityId)
        {
            var existeProduto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.Id == entityId);

            if (existeProduto != null)
            {
                _context.Produtos.Remove(existeProduto);
                await
                    _context.SaveChangesAsync();
            }
        }

        public async Task<IList<Produto>> GetAllAsync()
        {
            return
                await _context.Produtos
                    .ToListAsync();
        }

        public async Task<Produto?> GetByIdAsync(int entityId)
        {
            var produto = await _context.Produtos
                                .SingleOrDefaultAsync(p => p.Id == entityId);

            return produto;
        }


        public async Task UpdateAsync(Produto entity)
        {
            var existeProduto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.Id == entity.Id);

            if (existeProduto != null)
            {
                _context.Entry(existeProduto).CurrentValues.SetValues(entity);
                await
                    _context.SaveChangesAsync();
            }
        }

        //Extra
        public async Task<List<Produto>> BuscarPorIdsAsync(List<int> Ids)
        {
            return await _context.Produtos
                .Where(p => Ids.Contains(p.Id))
                .ToListAsync();
        }

        public async Task<List<Produto>> BuscarProdutoAsync(List<int> produtoIds)
        {
            return await _context.Produtos
                    .Where(i => produtoIds.Contains(i.Id))
                    .ToListAsync();
        }
    }
}