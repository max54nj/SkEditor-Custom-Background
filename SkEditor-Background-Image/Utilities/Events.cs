using Avalonia.Input;
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
    }

    public static void Unregister()
    {
        var mainWindow = SkEditorAPI.Windows.GetMainWindow();

        if (mainWindow != null)
        {
            mainWindow.KeyDown -= OnMainWindowKeyDown;
        }

        SkEditorAPI.Events.OnLanguageChanged -= OnLanguageChanged;
    }

    private static void OnLanguageChanged(object? sender, LanguageChangedEventArgs args)
    {
        Translation.ChangeLanguage(args.Language).Wait();
    }

    private static void OnMainWindowKeyDown(object? sender, KeyEventArgs args)
    {
        
    }
}