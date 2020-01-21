using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface IEmployeesRepository
    {
        Task<bool> AddAsync(Employees emp);
        Task<IEnumerable<Employees>> GetAllAsync();
        Task<Employees> GetByIDAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(Employees emp);
        Task<int> GetEmployeeSalesByCountry();
        Task<IEnumerable<Products>> GetAllProductWithCategory();
    }
}
