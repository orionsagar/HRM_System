using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Persistence.DAL
{
    public interface ITransactable : IDisposable
    {
        Task<ITransactable> BeginNewTransationAsync();
        Task FinishTransactionAsync();
    }

    public class Transactable : ITransactable
    {
        private readonly IApplicationDbContext db;
        private IDbContextTransaction transaction;

        public Transactable(IApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<ITransactable> BeginNewTransationAsync()
        {
            transaction = await db.Database.BeginTransactionAsync();

            return this;
        }

        public async Task FinishTransactionAsync()
        {
            try
            {
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                transaction?.Dispose();
                transaction = null;
            }
        }
    }
}
