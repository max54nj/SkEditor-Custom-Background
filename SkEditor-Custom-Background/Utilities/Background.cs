using Avalonia.Threading;
using SkEditor.API;

namespace CustomBackgroundAddon.Utilities;

public class Background
{
    public static void Register()
    {
        var settings = Settings.Settings.Instance;

        if (!BackgroundImage.Enabled()) return;
        BackgroundImage.Load();

        if (!settings.KeepEditorBackground)
        {
            StyleOverrides.Apply("EditorBackground");
        }
        
        
    }

    public static void Unregister()
    {
        StyleOverrides.RemoveAll();
        
        BackgroundImage.Remove();
    }
    
    public static void Reload()
    {
        Dispatcher.UIThread.Post(async () =>
        {
            await SkEditorAPI.Addons.DisableAddon(CustomBackgroundAddon.Instance);
            await SkEditorAPI.Addons.EnableAddon(CustomBackgroundAddon.Instance);
        });
    }

    public static void Setup()
    {
        BackgroundImage.Setup();
    }
}