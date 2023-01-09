using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Threading;

namespace DatabaseUI.Database.Abstract
{
    public abstract class DBSession : IDisposable
    {
        private readonly Semaphore sem = new Semaphore(1, 1);
        public void Lock() => sem.WaitOne();
        public void Unlock() => sem.Release();

        public abstract Task<DbDataReader> ExecuteReaderAsync(string command);
        public abstract Task<int> ExecuteNonQueryAsync(string command);
        public abstract Task<object> ExecuteScalarAsync(string command);

        public virtual void Dispose() { }
    }
}
