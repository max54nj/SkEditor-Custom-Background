using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace BackgroundImageAddon.Utilities;

public class Translation
{
    public static readonly Translation Instance = new Translation();

    private static readonly Dictionary<string, ResourceDictionary> Translations = [];

    public static string? Language;

    public static string LanguagesFolder { get; } = Path.Combine(Settings.Settings.AppDataFolderPath, "Languages");

    public static string Get(string key, params string?[] parameters)
    {
        object? translation = null;
        Application.Current?.TryGetResource(key, ThemeVariant.Default, out translation);
        var translationString = translation?.ToString() ?? key;
        translationString = translationString.Replace("\\n", Environment.NewLine);

        for (var i = 0; i < parameters.Length; i++)
            translationString = translationString.Replace($"{{{i}}}", parameters[i]);

        return translationString;
    }

    public static async Task ChangeLanguage(string language)
    {
        if (Language == language) return;
        foreach (var translation in Translations.Where(translation => translation.Key != "English"))
        {
            Application.Current?.Resources.MergedDictionaries.Remove(translation.Value);
            Translations.Remove(translation.Key);
        }

        Uri languageXaml = new(Path.Combine(LanguagesFolder, $"{language}.xaml"));

        if (!File.Exists(languageXaml.OriginalString))
        {
            await ChangeLanguage("English");
            return;
        }

        await using FileStream languageStream = new(languageXaml.OriginalString, FileMode.Open, FileAccess.Read,
                                                    FileShare.ReadWrite, 16384, FileOptions.Asynchronous);

        if (AvaloniaRuntimeXamlLoader.Load(languageStream) is ResourceDictionary dictionary)
        {
            Application.Current?.Resources.MergedDictionaries.Add(dictionary);
            Translations.TryAdd(language, dictionary);
        }

        Language = language;
    }
}