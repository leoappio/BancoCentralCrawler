using System.Net;
using System.Text;

namespace BancoCentralCrawler.Services.Helpers;

public static class WebHelper
{
    private const string Agent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 Edg/91.0.864.70";
    
    public static async Task<string> GetAsync(string url, Encoding encoding)
    {
        while (true)
        {
            var httpHandler = new HttpClientHandler();

            var httpClient = new HttpClient(httpHandler) { Timeout = new TimeSpan(0, 5, 0) };

            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(Agent);

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) throw new ApplicationException("Erro ao obter conteúdo do site.");

            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                url = response?.Headers?.Location?.ToString() ?? string.Empty;
                continue;
            }

            using var stream = new StreamReader(await response.Content.ReadAsStreamAsync(), encoding);
            return await stream.ReadToEndAsync();
        }
    }
}