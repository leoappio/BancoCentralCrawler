using BancoCentralCrawler.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BancoCentralCrawler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BancoCentralController : ControllerBase
    {
        private readonly IBancoCentralService _bancoCentralService;
        public BancoCentralController(IBancoCentralService bancoCentralService)
        {
            _bancoCentralService = bancoCentralService;
        }

        [HttpGet(Name = "GetNoticiasPorAno/{ano}")]
        public async Task<IEnumerable<>> GetNoticiasPorAnoAsync([FromRoute] int ano)
        {

        }
        
        [HttpGet(Name = "GetTodasAsNoticias")]
        public async Task<IEnumerable<>> GetNoticiasAsync()
        {

        }
        
        [HttpGet(Name = "GetNotasPorAno/{ano}")]
        public async Task<IEnumerable<>> GetNotasPorAnoAsync([FromRoute] int ano)
        {

        }
        
        [HttpGet(Name = "GetTodasAsNotas")]
        public async Task<IEnumerable<>> GetNotasAsync()
        {

        }
        
        [HttpGet(Name = "GetTodasAsNotasENoticias")]
        public async Task<IEnumerable<>> GetTodasAsync()
        {

        }
    }
}
