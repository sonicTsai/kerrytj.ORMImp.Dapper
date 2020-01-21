using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        Task<bool> AddAsync(Products prod);
        Task<Products> GetByIDAsync(int id);
    }
}
