
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProdutoRepository : IBaseRepository<Produto>
    {
        Task CreateAsync(Produto entity);
        
    }
}