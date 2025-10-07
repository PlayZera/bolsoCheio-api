using BolsoCheio.Data.Models;

namespace BolsoCheio.Data.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
    }
}