// See https://aka.ms/new-console-template for more information

using GenshinVoices;

var fullLanguageNamesArray = new Dictionary<string, string>()
{
    { "ja", "Japanese" }
};

if (args.Length != 1)
{
    Console.WriteLine("Usage: GenshinVoices.exe LANGUAGE");
    return;
}

var language = args[0];
var crawler = new Crawler();
await crawler.Request("wiki/Character/List#Playable_Characters");
var trs = crawler.GetNodes("(//table)[1]/tbody/tr");

if (trs != null)
{
    var first = true;
    
    foreach (var tr in trs)
    {
        if (first)
        {
            first = false;
            continue;
        }

        var name = tr.SelectSingleNode("(.//td)[2]")?.SelectSingleNode("a")?.InnerText;
        Console.WriteLine($"Processing {name}");
        await crawler.Request($"wiki/{name}/Voice-Overs" + (language != "en" ? "/" + fullLanguageNamesArray[language] : ""));

        try
        {
            var xpath = string.Empty;

            switch (name)
            {
                case "Traveler":
                    break;
                case "Cyno":
                    xpath = "//a[contains(@title, 'Cyno Hello - The Present.ogg')]";
                    break;
                case "Sethos":
                    xpath = "//a[contains(@title, 'VO Sethos Hello - Lumine.ogg')]";
                    break;
                default:
                    xpath = "//a[contains(@title, '" + name + " Hello.ogg')]";
                    break;
            }

            if (string.IsNullOrEmpty(xpath))
            {
                Console.WriteLine($"Skipped {name}");
                continue;
            }

            var audio = crawler.GetNode(xpath)?.ParentNode
                .SelectSingleNode("//audio");
            await Helper.DownloadVoice(language, name, audio.GetAttributeValue("src", ""));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine($"Cannot process {name}: {ex.Message}");
        }
    }
}