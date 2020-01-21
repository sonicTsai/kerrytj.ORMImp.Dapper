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
    public class ContribProductRepository : IProductsRepository
    {
        public async Task<bool> AddAsync(Products prod)
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                return await conn.InsertAsync(prod) > 0;
            }
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                return await conn.GetAllAsync<Products>();
            }
        }

        public async Task<Products> GetByIDAsync(int id)
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                return await conn.GetAsync<Products>(id);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                var entity = await conn.GetAsync<Products>(id);
                return await conn.DeleteAsync(entity);
            }
        }

        public async Task<bool> UpdateAsync(Products prod)
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                return await conn.UpdateAsync(prod);
            }
        }
    }
}
