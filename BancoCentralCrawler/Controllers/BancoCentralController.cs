using BancoCentralCrawler.Domain;
using BancoCentralCrawler.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BancoCentralCrawler.Controllers;

[ApiController]
[Route("[controller]")]
public class BancoCentralController : ControllerBase
{
    private readonly IBancoCentralService _bancoCentralService;
    public BancoCentralController(IBancoCentralService bancoCentralService)
    {
        _bancoCentralService = bancoCentralService;
    }

    [HttpGet("GetNoticiasPorAno/{ano}")]
    public async Task<ActionResult<IEnumerable<NoticiaDetalheResponseDto>>> GetNewsByYearAsync([FromRoute] int ano)
    {
        var result = await _bancoCentralService.GetNewsByYearAsync(ano);

        return Ok(result);
    }
        
    [HttpGet("GetTodasAsNoticias")]
    public async Task<ActionResult<IEnumerable<NoticiaDetalheResponseDto>>> GetNoticiasAsync()
    {
        return Ok();
    }
        
    [HttpGet("GetNotasPorAno/{ano}")]
    public async Task<ActionResult<IEnumerable<NoticiaDetalheResponseDto>>> GetNotasPorAnoAsync([FromRoute] int ano)
    {
        return Ok();
    }
        
    [HttpGet("GetTodasAsNotas")]
    public async Task<ActionResult<IEnumerable<NoticiaDetalheResponseDto>>> GetNotasAsync()
    {
        return Ok();
    }
        
    [HttpGet("GetTodasAsNotasENoticias")]
    public async Task<ActionResult<IEnumerable<NoticiaDetalheResponseDto>>> GetTodasAsync()
    {
        return Ok();
    }
}