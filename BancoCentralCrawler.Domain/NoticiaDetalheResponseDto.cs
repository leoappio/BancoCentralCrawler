﻿namespace BancoCentralCrawler.Domain;

public class NoticiaDetalheResponseDto
{
    public string Id { get; set; }
    public string Titulo { get; set; }
    public DateTime Data { get; set; }
    public string Descricao { get; set; }
    public string Conteudo { get; set; }
    public Uri UrlOriginal { get; set; }
    
    public NoticiaDetalheResponseDto()
    {

    }

    public NoticiaDetalheResponseDto(string id,
        string titulo,
        DateTime data,
        string descricao,
        string conteudo,
        Uri urlOriginal)
    {
        Id = id;
        Titulo = titulo;
        Data = data;
        Descricao = descricao;
        Conteudo = conteudo;
        UrlOriginal = urlOriginal;
    }
}