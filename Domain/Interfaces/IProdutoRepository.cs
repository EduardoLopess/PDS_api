
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProdutoRepository : IBaseRepository<Produto>
    {
        Task CreateAsync(Produto entity);
        Task<List<Produto>> BuscarPorIdsAsync(List<int> Ids);
        Task<List<Produto>> BuscarProdutoAsync(List<int> produtosIds);
        
    }
}