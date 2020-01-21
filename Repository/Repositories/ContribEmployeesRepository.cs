using Dapper.Contrib.Extensions;
using Repository.Models;
using Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ContribProductRepository : IProductRepository
    {
        public async Task<bool> AddAsync(Product prod)
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                return await conn.InsertAsync(prod) > 0;
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                return await conn.GetAllAsync<Product>();
            }
        }

        public async Task<Product> GetByIDAsync(int id)
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                return await conn.GetAsync<Product>(id);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                var entity = await conn.GetAsync<Product>(id);
                return await conn.DeleteAsync(entity);
            }
        }

        public async Task<bool> UpdateAsync(Product prod)
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                return await conn.UpdateAsync(prod);
            }
        }
    }
}
