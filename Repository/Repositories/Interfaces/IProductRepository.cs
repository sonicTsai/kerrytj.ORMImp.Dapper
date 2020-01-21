using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<bool> AddAsync(Product prod);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIDAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(Product prod);
    }
}
