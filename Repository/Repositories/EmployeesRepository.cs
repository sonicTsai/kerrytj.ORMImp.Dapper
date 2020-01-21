using Dapper;
using Repository.Models;
using Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class EmployeesRepository : IEmployeesRepository
    {
        public async Task<bool> AddAsync(Employees emp)
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                string sql = @"INSERT INTO [dbo].[Employees]
           ([LastName]
           ,[FirstName]
           ,[Title]
           ,[TitleOfCourtesy]
           ,[BirthDate]
           ,[HireDate]
           ,[Address]
           ,[City]
           ,[Region]
           ,[PostalCode]
           ,[Country]
           ,[HomePhone]
           ,[Extension]
           ,[Photo]
           ,[Notes]
           ,[ReportsTo]
           ,[PhotoPath])
     VALUES
           (@LastName,
           @FirstName,
           @Title, 
           @TitleOfCourtesy, 
          @BirthDate, 
           @HireDate, 
           @Address, 
           @City, 
           @Region, 
           @PostalCode, 
           @Country, 
           @HomePhone, 
           @Extension, 
           @Photo, 
           @Notes, 
           @ReportsTo, 
           @PhotoPath)";

                return await conn.ExecuteAsync(sql, emp) > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                string sql = @"delete from Employees where EmployeeID = @EmployeeID";
                return await conn.ExecuteAsync(sql, new { EmployeeID = id }) > 0;
            }
        }

        public async Task<IEnumerable<Employees>> GetAllAsync()
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                string sql = @"SELECT [EmployeeID]
      ,[LastName]
      ,[FirstName]
      ,[Title]
      ,[TitleOfCourtesy]
      ,[BirthDate]
      ,[HireDate]
      ,[Address]
      ,[City]
      ,[Region]
      ,[PostalCode]
      ,[Country]
      ,[HomePhone]
      ,[Extension]
      ,[Photo]
      ,[Notes]
      ,[ReportsTo]
      ,[PhotoPath]
  FROM [dbo].[Employees]";
                return await conn.QueryAsync<Employees>(sql);
            }
        }

        public async Task<Employees> GetByIDAsync(int id)
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                string sql = @"SELECT [EmployeeID]
      ,[LastName]
      ,[FirstName]
      ,[Title]
      ,[TitleOfCourtesy]
      ,[BirthDate]
      ,[HireDate]
      ,[Address]
      ,[City]
      ,[Region]
      ,[PostalCode]
      ,[Country]
      ,[HomePhone]
      ,[Extension]
      ,[Photo]
      ,[Notes]
      ,[ReportsTo]
      ,[PhotoPath]
  FROM [dbo].[Employees]
where EmployeeID = @EmployeeID";
                return await conn.QueryFirstOrDefaultAsync<Employees>(sql, new { EmployeeID = id });
            }
        }

        public async Task<bool> UpdateAsync(Employees emp)
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                conn.Open();
                //使用Transaction
                using (IDbTransaction tran = conn.BeginTransaction())
                {
                    string sql = @"UPDATE [dbo].[Employees]
   SET [LastName] = @LastName,
      [FirstName] = @FirstName,
      [Title] = @Title, 
      [TitleOfCourtesy] = @TitleOfCourtesy, 
      [BirthDate] = @BirthDate, 
      [HireDate] = @HireDate, 
      [Address] = @Address, 
      [City] = @City, 
      [Region] = @Region, 
      [PostalCode] = @PostalCode, 
      [Country] = @Country, 
      [HomePhone] = @HomePhone, 
      [Extension] = @Extension, 
      [Photo] = @Photo, 
      [Notes] = @Notes, 
      [ReportsTo] = @ReportsTo, 
      [PhotoPath] = @PhotoPath
    WHERE EmployeeID = @EmployeeID";
                    var data = await conn.ExecuteAsync(sql, emp, transaction: tran) > 0;
                    tran.Commit();
                    return data;
                }
            }
        }

        public async Task<int> GetEmployeeSalesByCountry()
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                var ci = new System.Globalization.CultureInfo("en-US", true);
                DateTime beginningDate = DateTime.Parse("1996/01/01", ci);
                DateTime endingDate = DateTime.Parse("1998/12/31", ci);
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Beginning_Date", beginningDate, DbType.DateTime, ParameterDirection.Input);
                parameters.Add("@Ending_Date", endingDate, DbType.DateTime, ParameterDirection.Input);
                parameters.Add("@return_value", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                await conn.ExecuteAsync("Employee Sales by Country", parameters, commandType: CommandType.StoredProcedure);
                int result = parameters.Get<int>("@return_value");
                return result;
            }
        }

        public  async Task<IEnumerable<Product>> GetAllProductWithCategory() {
            //多表對應
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                var selectAllProductWithCategorySQL = @"select * from Products p 
    inner join Categories c on c.CategoryID = p.CategoryId
    Order by p.ProductID";
                var allProductWithCategory = await conn.QueryAsync<Product, Category, Product>
                    (selectAllProductWithCategorySQL, (prod, cg) => { prod.Category = cg; return prod; }, splitOn: "CategoryId");
                return allProductWithCategory;
            }
        }

    }
}
