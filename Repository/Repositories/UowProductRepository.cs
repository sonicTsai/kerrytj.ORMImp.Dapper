using Repository.DbContext;
using Repository.Models;
using Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class UowProductRepository : IProductRepository
    {
        private readonly DapperDBContext _context;

        public UowProductRepository(DapperDBContext context)
        {
            this._context = context;
        }

        public async Task<bool> AddAsync(Product prod)
        {
            string sql = @"INSERT INTO Product 
                        (ProductName
                        ,QuantityPerUnit
                        ,UnitPrice
                        ,CategoryID)
                    VALUES
                        (@ProductName
                        ,@QuantityPerUnit
                        ,@UnitPrice
                        ,@CategoryID)";
            return await _context.ExecuteAsync(sql, prod) > 0;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetByIDAsync(int id)
        {
            string sql = @"SELECT ProductID
                            ,ProductName
                            ,QuantityPerUnit
                            ,UnitPrice 
                            ,CategoryID
                        FROM Product
                        WHERE ProductID = @Id";
            return await _context.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
        }

        public Task<bool> UpdateAsync(Product prod)
        {
            throw new NotImplementedException();
        }
    }
}
