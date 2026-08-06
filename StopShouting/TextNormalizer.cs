using System.Text.RegularExpressions;
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace StopShouting;

public static class TextNormalizer
{
    private static readonly Dictionary<string, string> BusinessTerms =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["adobe"] = "Adobe",
            ["after effects"] = "After Effects",
            ["athena"] = "Athena",
            ["azure"] = "Azure",
            ["bonafide"] = "Bonafide",
            ["canva"] = "Canva",
            ["drx"] = "DRX",
            ["excel"] = "Excel",
            ["figma"] = "Figma",
            ["illustrator"] = "Illustrator",
            ["indesign"] = "InDesign",
            ["lightroom"] = "Lightroom",
            ["m365"] = "M365",
            ["microsoft"] = "Microsoft",
            ["onedrive"] = "OneDrive",
            ["outlook"] = "Outlook",
            ["photoshop"] = "Photoshop",
            ["power apps"] = "PowerApps",
            ["power automate"] = "Power Automate",
            ["power bi"] = "Power BI",
            ["power fx"] = "Power Fx",
            ["power pages"] = "Power Pages",
            ["power shell"] = "PowerShell",
            ["power virtual agents"] = "Power Virtual Agents",
            ["premiere pro"] = "Premiere Pro",
            ["rx4route"] = "RX4Route",
            ["sharepoint"] = "SharePoint",
            ["sketch"] = "Sketch",
            ["teams"] = "Teams",
            ["word"] = "Word",
        };

    private static readonly Dictionary<string, string> Names =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [" alexander"] = " Alexander",
            [" andrew"] = " Andrew",
            [" anthony"] = " Anthony",
            [" ava"] = " Ava",
            [" ben"] = " Ben",
            [" charles"] = " Charles",
            [" christa"] = " Christa",
            [" christi"] = " Christi",
            [" christopher"] = " Christopher",
            [" daniel"] = " Daniel",
            [" danielle"] = " Danielle",
            [" david"] = " David",
            [" emily"] = " Emily",
            [" ethan"] = " Ethan",
            [" isabella"] = " Isabella",
            [" james"] = " James",
            [" john"] = " John",
            [" jordan"] = " Jordan",
            [" jose"] = " Jose",
            [" joseph"] = " Joseph",
            [" joshua"] = " Joshua",
            [" kristi"] = " Kristi",
            [" matt "] = " Matt ",
            [" matthew"] = " Matthew",
            [" mia"] = " Mia",
            [" michael"] = " Michael",
            [" mikayla"] = " Mikayla",
            [" nicholas"] = " Nicholas",
            [" olivia"] = " Olivia",
            [" rachel"] = " Rachel",
            [" robert"] = " Robert",
            [" samantha"] = " Samantha",
            [" sarah"] = " Sarah",
            [" sophia"] = " Sophia",
            [" steve"] = " Steve",
            [" thomas"] = " Thomas",
            [" tom"] = " Tom",
            [" william"] = " William",
            [" will"] = " Will"

        };
    private static readonly Dictionary<string, string> Contractions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["cant"] = "can't",
            ["couldnt"] = "couldn't",
            ["didnt"] = "didn't",
            ["doesnt"] = "doesn't",
            ["dont"] = "don't",
            ["hadnt"] = "hadn't",
            ["hasnt"] = "hasn't",
            ["havent"] = "haven't",
            ["isnt"] = "isn't",
            ["mustnt"] = "mustn't",
            ["shouldnt"] = "shouldn't",
            ["wasnt"] = "wasn't",
            ["werent"] = "weren't",
            ["wont"] = "won't",
            ["wouldnt"] = "wouldn't",

            ["arent"] = "aren't",
            ["aint"] = "ain't",

            ["im"] = "I'm",
            ["ive"] = "I've",
            ["id"] = "I'd",
            ["ill"] = "I'll",

            ["theyre"] = "they're",
            ["theyve"] = "they've",
            ["theyd"] = "they'd",
            ["theyll"] = "they'll",

            ["youre"] = "you're",
            ["youve"] = "you've",
            ["youd"] = "you'd",
            ["youll"] = "you'll",

            ["were"] = "we're",
            ["weve"] = "we've",
            ["wed"] = "we'd",

            ["thats"] = "that's",
            ["theres"] = "there's",
            ["heres"] = "here's",
            ["whats"] = "what's",
            ["whos"] = "who's",
            ["wheres"] = "where's",
            ["whens"] = "when's",
            ["hows"] = "how's"
        };

    public static string Deshout(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var result = NormalizeWhitespace(input);

        result = result.ToLowerInvariant();

        result = ConvertToSentenceCase(result);
        
        result = ApplyDictionary(result, Contractions);

        result = ApplyDictionary(result, BusinessTerms);

        result = ApplyDictionary(result, Names);


        return result;
    }

    private static string NormalizeWhitespace(string input)
    {
        var lines = input.Split(
            ["\r\n", "\n"],
            StringSplitOptions.None);

        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = Regex.Replace(lines[i].Trim(), @"\s+", " ");
        }

        var result = string.Join(Environment.NewLine, lines);

        result = Regex.Replace(
            result,
            @"(\r?\n){3,}",
            Environment.NewLine + Environment.NewLine);

        return result;
    }

    private static string ConvertToSentenceCase(string input)
    {
        var chars = input.ToCharArray();

        var capitalizeNext = true;

        for (int i = 0; i < chars.Length; i++)
        {
            if (capitalizeNext && char.IsLetter(chars[i]))
            {
                chars[i] = char.ToUpper(chars[i]);
                capitalizeNext = false;
            }

            if (chars[i] == '.' ||
                chars[i] == '!' ||
                chars[i] == '?')
            {
                capitalizeNext = true;
            }
        }

        return new string(chars);
    }

    private static string ApplyDictionary(
        string input,
        Dictionary<string, string> replacements)
    {
        return replacements.Aggregate(input, 
            (current, item) 
                => Regex.Replace(current, $@"\b{Regex.Escape(item.Key)}\b",
                    item.Value, RegexOptions.
                        IgnoreCase));
    }
}
