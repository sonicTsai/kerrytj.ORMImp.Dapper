﻿using Dapper;
using Repository.Models;
using Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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

        private string updateSql = @"UPDATE [dbo].[Employees]
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

        public async Task<bool> UpdateAsync(Employees emp)
        {
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                //使用Transaction
                using (IDbTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        var data = await conn.ExecuteAsync(updateSql, emp, transaction: tran) > 0;
                        tran.Commit();
                        return data;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<bool> UpdateUsingTransactionScopeAsync(Employees emp)
        {
            bool data = false;
            //1.使用Transaction Scope，Default TransactionScopeOption = Required
            //2.TransactionScope在非同步中使用會出現 A TransactionScope must be disposed on the same thread that it was created 錯誤
            //需使用 TransactionScopeAsyncFlowOption.Enabled 選項，讓Thread可以進行切換其他Thread
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                using (IDbConnection conn = DBConfig.GetSqlConnection())
                {
                    //ConfigureAwait(false)
                    //ASP.NET Core 基本上不用去擔心和 UI 互動是不是會發生 deadlock 的問題，
                    //ASP.NET Core 已經沒有 AspNetSynchronizationContext 的設計了，
                    //所以任何的非同步呼叫基本上都可以一直採用 ConfigureAwait(false) 來幫你擠出那麼點效能出來．
                    //https://dotblogs.com.tw/aspnetshare/2018/06/03/configureawaiter
                    data = await conn.ExecuteAsync(updateSql, emp).ConfigureAwait(false) > 0;
                }
                scope.Complete();
            }
            return data;
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

        public async Task<IEnumerable<Products>> GetAllProductWithCategory()
        {
            //多表對應
            using (IDbConnection conn = DBConfig.GetSqlConnection())
            {
                var selectAllProductWithCategorySQL = @"select * from Products p 
    inner join Categories c on c.CategoryID = p.CategoryId
    Order by p.ProductID";
                var allProductWithCategory = await conn.QueryAsync<Products, Category, Products>
                    (selectAllProductWithCategorySQL, (prod, cg) => { prod.Category = cg; return prod; }, splitOn: "CategoryId");
                return allProductWithCategory;
            }
        }

    }
}
