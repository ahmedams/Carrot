using System.Collections.Generic;
using System.Threading.Tasks;
using Carrot.Contracts.DTOs;
using Carrot.Contracts.DTOs.Common;

namespace Carrot.Contracts.Services
{
    public interface IAccountService
    {
        ServiceResponseModel<List<Account>> GetAccounts();
        Task<ServiceResponseModel<Account>> GetAccount(long id);
        Task<ServiceResponseModel<bool>> UpdateAccount(long id, Account account);
        Task UpdateAccounts(List<User> users);
    }
}
