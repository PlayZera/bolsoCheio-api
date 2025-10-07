using BolsoCheio.Data.Interfaces;
using BolsoCheio.Data.Models;

namespace BolsoCheio.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        // Simulando um banco de dados em memória
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Produto 1", Description = "Descrição do produto 1", Price = 10.99m, Stock = 100 },
            new Product { Id = 2, Name = "Produto 2", Description = "Descrição do produto 2", Price = 25.50m, Stock = 50 },
            new Product { Id = 3, Name = "Produto 3", Description = "Descrição do produto 3", Price = 15.75m, Stock = 75 }
        };

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await Task.FromResult(_products);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return await Task.FromResult(product);
        }

        public async Task<Product> CreateAsync(Product entity)
        {
            entity.Id = _products.Max(p => p.Id) + 1;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            _products.Add(entity);
            return await Task.FromResult(entity);
        }

        public async Task<Product> UpdateAsync(Product entity)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == entity.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = entity.Name;
                existingProduct.Description = entity.Description;
                existingProduct.Price = entity.Price;
                existingProduct.Stock = entity.Stock;
                existingProduct.IsActive = entity.IsActive;
                existingProduct.UpdatedAt = DateTime.UtcNow;
            }
            return await Task.FromResult(existingProduct ?? entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var exists = _products.Any(p => p.Id == id);
            return await Task.FromResult(exists);
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            var activeProducts = _products.Where(p => p.IsActive);
            return await Task.FromResult(activeProducts);
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            var products = _products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            return await Task.FromResult(products);
        }
    }
}