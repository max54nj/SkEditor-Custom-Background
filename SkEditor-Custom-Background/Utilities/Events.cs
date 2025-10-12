using Avalonia.Input;
using SkEditor.API;

namespace CustomBackgroundAddon.Utilities;

public static class Events
{
    public static void Register()
    {
        var mainWindow = SkEditorAPI.Windows.GetMainWindow();

        if (mainWindow != null)
        {
            mainWindow.KeyDown += OnMainWindowKeyDown;
        }

        SkEditorAPI.Events.OnLanguageChanged += OnLanguageChanged;
        SkEditorAPI.Events.OnAddonSettingChanged += OnAddonSettingChanged;
    }

    public static void Unregister()
    {
        var mainWindow = SkEditorAPI.Windows.GetMainWindow();

        if (mainWindow != null)
        {
            mainWindow.KeyDown -= OnMainWindowKeyDown;
        }

        SkEditorAPI.Events.OnLanguageChanged -= OnLanguageChanged;
        SkEditorAPI.Events.OnAddonSettingChanged -= OnAddonSettingChanged;
    }

    private static void OnLanguageChanged(object? sender, LanguageChangedEventArgs args)
    {
        Translation.ChangeLanguage(args.Language).Wait();
    }

    private static void OnMainWindowKeyDown(object? sender, KeyEventArgs args)
    {
        
    }

    private static void OnAddonSettingChanged(object? sender, AddonSettingChangedEventArgs args)
    {
        if (args.Setting.Addon.Identifier != CustomBackgroundAddon.Instance.Identifier) return;
     
        SkEditorAPI.Logs.Info($"Reloading {CustomBackgroundAddon.Instance.Name} Addon due to setting changes...");

        Background.Reload();
    }
}