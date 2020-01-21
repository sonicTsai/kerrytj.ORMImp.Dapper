using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.DbContext
{
    public interface IContext : IDisposable
    {
        bool IsTransactionStarted { get; }

        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}
