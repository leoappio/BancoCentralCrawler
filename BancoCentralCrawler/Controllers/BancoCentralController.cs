using BancoCentralCrawler.Domain;
using BancoCentralCrawler.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    public async Task<ActionResult<IEnumerable<ExtractedDataDto>>> GetNewsByYearAsync([FromRoute] int ano)
    {
        var result = await _bancoCentralService.GetNewsByYearAsync(ano);

        return Ok(result);
    }

    [HttpGet("GetTodasAsNoticias")]
    public async Task<ActionResult<IEnumerable<ExtractedDataDto>>> GetAllNewsAsync()
    {
        var result = await _bancoCentralService.GetAllNewsAsync();

        return Ok(result);
    }

    [HttpGet("GetNotasPorAno/{ano}")]
    public async Task<ActionResult<IEnumerable<ExtractedDataDto>>> GetPressReleasesByYearAsync(
        [FromRoute] int ano)
    {
        var result = await _bancoCentralService.GetPressReleasesByYearAsync(ano);

        return Ok(result);
    }

    [HttpGet("GetTodasAsNotas")]
    public async Task<ActionResult<IEnumerable<ExtractedDataDto>>> GetAllPressReleasesAsync()
    {
        var result = await _bancoCentralService.GetAllPressReleasesAsync();

        return Ok(result);
    }

    [HttpGet("GetTodasAsNotasENoticias")]
    public async Task<ActionResult<IEnumerable<ExtractedDataDto>>> GetAllNewsAndPressReleasesAsync()
    {
        var result = await _bancoCentralService.GetAllNewsAndPressReleasesAsync();

        var totalItens = result.Count;
        var stringResult = JsonConvert.SerializeObject(result);

        return Ok(result);
    }
}