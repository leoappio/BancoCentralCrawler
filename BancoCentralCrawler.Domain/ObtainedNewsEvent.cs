namespace BancoCentralCrawler.Domain;

public class ObtainedNewsEvent
{
    public int Id { get; set; }

    public Uri UrlOriginal { get; set; }

    public string Titulo { get; set; }

    public ObtainedNewsEvent(
        int id,
        Uri urlOriginal,
        string titulo)
    {
        Id = id;
        UrlOriginal = urlOriginal;
        Titulo = titulo;
    }
}