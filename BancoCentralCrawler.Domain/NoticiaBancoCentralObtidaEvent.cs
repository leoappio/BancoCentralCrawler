namespace BancoCentralCrawler.Domain;

public class NoticiaBancoCentralObtidaEvent
{
    public int Id { get; set; }

    public DateTime DataModificacao { get; set; }

    public Uri UrlOriginal { get; set; }

    public string Titulo { get; set; }

    public DateTime? DataDivulgacao { get; set; }

    public NoticiaBancoCentralObtidaEvent(
        int id,
        DateTime dataModificacao,
        Uri urlOriginal,
        string titulo,
        DateTime? dataDivulgacao)
    {
        Id = id;
        DataModificacao = dataModificacao;
        UrlOriginal = urlOriginal;
        Titulo = titulo;
        DataDivulgacao = dataDivulgacao;
    }
}