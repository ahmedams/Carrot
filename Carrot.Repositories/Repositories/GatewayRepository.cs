using Carrot.Contracts.Repositories;
using Carrot.Entities;
using Carrot.Repositories.Common;

namespace Carrot.Repositories.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(Context context) : base(context)
        {
            Context = context;
        }
    }
}
