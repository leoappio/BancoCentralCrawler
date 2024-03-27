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
    public async Task<ActionResult<IEnumerable<NoticiaDetalheResponseDto>>> GetAllNewsAsync()
    {
        var result = await _bancoCentralService.GetAllNewsAsync();

        return Ok(result);
    }

    [HttpGet("GetNotasPorAno/{ano}")]
    public async Task<ActionResult<IEnumerable<NoticiaDetalheResponseDto>>> GetPressReleasesByYearAsync(
        [FromRoute] int ano)
    {
        var result = await _bancoCentralService.GetPressReleasesByYearAsync(ano);

        return Ok(result);
    }

    [HttpGet("GetTodasAsNotas")]
    public async Task<ActionResult<IEnumerable<NoticiaDetalheResponseDto>>> GetAllPressReleasesAsync()
    {
        var result = await _bancoCentralService.GetAllPressReleasesAsync();

        return Ok(result);
    }

    [HttpGet("GetTodasAsNotasENoticias")]
    public async Task<ActionResult<IEnumerable<NoticiaDetalheResponseDto>>> GetAllNewsAndPressReleasesAsync()
    {
        var result = await _bancoCentralService.GetAllNewsAndPressReleasesAsync();

        return Ok(result);
    }
}