using HtmlAgilityPack;

namespace GenshinVoices;

public class Crawler
{
    private const string RootUrl = "https://genshin-impact.fandom.com";

    private readonly HttpClient _httpClient = new(new HttpClientHandler());
    private HtmlDocument? _htmlDocument;

    public async Task Request(string url)
    {
        var raw = await _httpClient.GetStringAsync(RootUrl + "/" + url);
        
        _htmlDocument = new HtmlDocument();
        _htmlDocument.LoadHtml(raw);
    }

    public HtmlNode? GetNode(string xpath)
    {
        return _htmlDocument?.DocumentNode.SelectSingleNode(xpath);
    }

    public HtmlNodeCollection? GetNodes(string xpath)
    {
        return _htmlDocument?.DocumentNode.SelectNodes(xpath);
    }
}