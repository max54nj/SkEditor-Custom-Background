using Avalonia.Input;
using Avalonia.Threading;
using SkEditor.API;

namespace BackgroundImageAddon.Utilities;

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
        if (args.Setting.Addon.Identifier != BackgroundImageAddon.Instance.Identifier) return;
     
        SkEditorAPI.Logs.Info("Reloading background due to setting change...");
        
        Dispatcher.UIThread.Post(async void () => await Background.Reload());
    }
}