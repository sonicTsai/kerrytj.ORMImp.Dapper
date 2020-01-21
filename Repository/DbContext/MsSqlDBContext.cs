using Microsoft.Extensions.Options;
using Repository.DbContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.DbContext
{
    public class MsSqlDapperDBContext : DapperDBContext
    {
        public MsSqlDapperDBContext(IOptions<DapperDBContextOptions> optionsAccessor) : base(optionsAccessor)
        {
        }

        protected override IDbConnection CreateConnection(string connectionString)
        {
            IDbConnection conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
