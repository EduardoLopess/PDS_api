using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAdicionalRepository : IBaseRepository<Adicional>
    {
        Task CreateAdicionalAsync(Adicional entity);
        
    }
}