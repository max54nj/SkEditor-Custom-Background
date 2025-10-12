using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace BackgroundImageAddon.Utilities.Settings;

public partial class Settings : ObservableObject
{
    [ObservableProperty] private bool _keepEditorBackground = false;
    [ObservableProperty] private bool _transparentBackground = false;
    
    public static string AppDataFolderPath { get; } =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SkEditor", "Addons",
                     BackgroundImageAddon.Instance.Identifier);

    public static string SettingsFilePath { get; } = Path.Combine(AppDataFolderPath, "settings.json");

    public static Settings Instance { get; private set; } = new();

    public static Settings Load()
    {
        if (!File.Exists(SettingsFilePath) || string.IsNullOrWhiteSpace(File.ReadAllText(SettingsFilePath)))
            return LoadDefaultSettings();

        try
        {
            var newSettings =
                JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsFilePath)) ?? new Settings();

            Instance = newSettings;

            return newSettings;
        }
        catch (JsonException)
        {
            return LoadDefaultSettings();
        }
    }

    public static Settings LoadDefaultSettings()
    {
        if (!Directory.Exists(AppDataFolderPath)) Directory.CreateDirectory(Path.GetDirectoryName(AppDataFolderPath)!);

        var defaultSettings = new Settings();

        var jsonContent = JsonConvert.SerializeObject(defaultSettings, Formatting.Indented);
        File.WriteAllText(SettingsFilePath, jsonContent);

        return Instance = defaultSettings;
    }

    public void Save()
    {
        var jsonContent = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(SettingsFilePath, jsonContent);
    }
}