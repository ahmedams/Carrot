using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carrot.Contracts.DTOs;
using Carrot.Contracts.DTOs.Common;

namespace Carrot.Contracts.Services
{
    public interface IOktaService
    {
        Task<List<User>> GetUsersAsync();
        Task<ServiceResponseModel<bool>> UpdateUser(User user);
    }
}
