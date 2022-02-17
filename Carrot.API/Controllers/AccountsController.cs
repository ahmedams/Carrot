using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carrot.Contracts.DTOs;
using Carrot.Contracts.Services;

namespace Carrot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _account;
        private readonly IOktaService _tokenService;

        public AccountsController(IAccountService account, IOktaService tokenService)
        {
            _account = account;
            _tokenService = tokenService;
        }

        // GET: api/<AccountsController>
        [HttpGet]
        public async Task<ActionResult<List<Account>>> Get()
        {
            var result = _account.GetAccounts();
            return StatusCode(result.IsSuccessful ? 200 : result.Error.ErrorCode, result);
        }

        // GET api/<AccountsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> Get(int id)
        {
            var result = await _account.GetAccount(id);
            return StatusCode(result.IsSuccessful ? 200 : result.Error.ErrorCode, result);
        }

        // POST api/<AccountsController>
        [HttpPost]
        public async Task<ActionResult> Put([FromBody] Account account)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await _account.UpdateAccount(account.UserId, account);
            return StatusCode(result.IsSuccessful ? 200 : result.Error.ErrorCode, result);
        }

    }
}
