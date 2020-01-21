using Repository.DbContext;
using Repository.Models;
using Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class UowProductsRepository : IProductsRepository
    {
        private readonly DapperDBContext _context;

        public UowProductsRepository(DapperDBContext context)
        {
            this._context = context;
        }

        public async Task<bool> AddAsync(Products prod)
        {
            string sql = @"INSERT INTO Products 
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

        public async Task<Products> GetByIDAsync(int id)
        {
            string sql = @"SELECT ProductID
                            ,ProductName
                            ,QuantityPerUnit
                            ,UnitPrice 
                            ,CategoryID
                        FROM Products
                        WHERE ProductID = @ProductID";
            return await _context.QueryFirstOrDefaultAsync<Products>(sql, new { ProductID = id });
        }

    }
}
