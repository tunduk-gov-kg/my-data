using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyData.Core.Interfaces;
using MyData.Core.Models;

namespace MyData.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class JournalController : ControllerBase
    {
        private readonly IJournalService _journalService;

        public JournalController(IJournalService journalService)
        {
            _journalService = journalService;
        }

        public async Task<List<JournalRecord>> List([FromQuery] [Required] DateTime fromInclusive,
            [FromQuery] [Required] DateTime toInclusive)
        {
            return await _journalService.SearchAsync(fromInclusive, toInclusive);
        }
    }
}