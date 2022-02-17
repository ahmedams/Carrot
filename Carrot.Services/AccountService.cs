using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carrot.Contracts.DTOs;
using Carrot.Contracts.DTOs.Common;
using Carrot.Contracts.Services;
using Carrot.Repositories;

namespace Carrot.Services
{
    public class AccountService : IAccountService
    {
        private readonly RepositoryWrapper _wrapper;
        private readonly IOktaService _oktaService;

        public AccountService(RepositoryWrapper wrapper,IOktaService oktaService)
        {
            _wrapper = wrapper;
            _oktaService = oktaService;
        }

        public ServiceResponseModel<List<Account>> GetAccounts()
        {
            var returnedData = new ServiceResponseModel<List<Account>>();
            try
            {
                returnedData.Data = _wrapper.Account.FindAll().Select(s => new Account()
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    UserId = s.Id
                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                returnedData.Error.ErrorCode = 500;
                returnedData.Error.Errors.Add(e.Message);
            }

            return returnedData;
        }

        public async Task<ServiceResponseModel<Account>> GetAccount(long id)
        {
            var returnedData = new ServiceResponseModel<Account>();
            try
            {
                var dbObject = await _wrapper.Account.GetFirstAsync(g => g.Id == id);
                if (dbObject != null)
                    returnedData.Data = new Account()
                    {
                        FirstName = dbObject.FirstName,
                        LastName = dbObject.LastName,
                        UserId = dbObject.Id
                    };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                returnedData.Error.ErrorCode = 500;
                returnedData.Error.Errors.Add(e.Message);
            }

            return returnedData;
        }

        public async Task<ServiceResponseModel<bool>> UpdateAccount(long id, Account account)
        {
            var returnedData = new ServiceResponseModel<bool>();
            try
            {
                var dbAccount = await _wrapper.Account.GetFirstAsync(x => x.Id == id);
                if (dbAccount != null)
                {
                    dbAccount.FirstName = account.FirstName;
                    dbAccount.LastName = account.LastName;
                    _wrapper.Account.Update(dbAccount);

                    //Update at Okta
                    await _oktaService.UpdateUser(new User()
                    {
                        Id = dbAccount.OktaUserId,
                        Profile = new Profile()
                        {
                            Family_name = account.LastName,
                            Given_name = account.FirstName
                        }
                    });

                    returnedData.Data = await _wrapper.Account.SaveAsync() > 0;
                }
                else
                {
                    returnedData.Data = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                returnedData.Error.ErrorCode = 500;
                returnedData.Error.Errors.Add(e.Message);
            }

            return returnedData;
        }

        public async Task UpdateAccounts(List<User> users)
        {
            try
            {
                foreach (var user in users)
                {
                    var dbAccount = await _wrapper.Account.GetFirstAsync(x => x.OktaUserId == user.Id);
                    if (dbAccount != null)
                    {
                        dbAccount.FirstName = user.Profile?.Given_name;
                        dbAccount.LastName = user.Profile?.Family_name;
                        _wrapper.Account.Update(dbAccount);
                    }
                    else
                    {
                        await _wrapper.Account.CreateAsync(new Entities.Account()
                        {
                            FirstName = user.Profile?.Given_name,
                            LastName = user.Profile?.Family_name,
                            OktaUserId = user.Id
                        });
                    }
                    await _wrapper.Account.SaveAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
