using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace CustomBackgroundAddon.Utilities.Settings;

public partial class Settings : ObservableObject
{
    [ObservableProperty] private bool _keepEditorBackground = false;
    [ObservableProperty] private string _currentBackgroundPath = "";
    [ObservableProperty] private double _backgroundOpacity = 5.0;
    [ObservableProperty] private double _backgroundBlur = 0.0;
    
    public static string AppDataFolderPath { get; } =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SkEditor", "Addons",
                     CustomBackgroundAddon.Instance.Identifier);

    public static string SettingsFilePath { get; } = Path.Combine(AppDataFolderPath, "settings.json");
    
    public static string BackgroundFolderPath { get; } = Path.Combine(AppDataFolderPath, "backgrounds");

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