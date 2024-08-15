namespace GenshinVoices;

public class Helper
{
    public static async Task DownloadVoice(string language, string character, string url)
    {
        var client = new HttpClient();
        await using var s = await client.GetStreamAsync(url);
        await using var fs = new FileStream(GetCharacterFilename(language, character), FileMode.OpenOrCreate);
        await s.CopyToAsync(fs);
    }

    private static string GetCharacterFilename(string language, string character)
    {
        return "voices/" + language + "/vo-" + language + "-" + character.Replace(" ", "-").ToLower() + ".ogg";
    }
}