namespace Domain.Interfaces
{
    public interface IBaseRepository <Entity> where Entity : class
    {
        Task<Entity> GetByIdAsync(int entityId);
        Task<IList<Entity>> GetAllAsync();
        Task DeleteAsync(int entityId);
        Task UpdateAsync(Entity entity);
    }
}