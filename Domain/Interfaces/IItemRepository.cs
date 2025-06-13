namespace Domain.Interfaces
{
    public interface IItemRepository
    {
        Task BuscarProdutosById(IList<int> ids);
    }
}