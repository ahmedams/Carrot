using System;
using Carrot.Contracts.Repositories;
using Carrot.Entities;
using Carrot.Repositories.Repositories;

namespace Carrot.Repositories
{
    public class RepositoryWrapper : IDisposable
    {
        private readonly Context _context;
        public RepositoryWrapper(Context context)
        {
            _context = context;
        }

        private IAccountRepository _account;
        public IAccountRepository Account => _account ??= new AccountRepository(_context);
        public void Dispose() => GC.SuppressFinalize(this);
    }
}
