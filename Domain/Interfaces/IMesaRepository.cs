using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMesaRepository : IBaseRepository<Mesa>
    {
        Task CreateAsync(Mesa entity);
        // Task<bool> MudaStatusMesa(int entityId, bool statusMesa);
        Task<bool> MudaStatusMesaAsync(int entityId);


        Task GetByNumeroAsync(int numeroMesa);
    }
}