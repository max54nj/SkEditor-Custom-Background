using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Threading;
using SkEditor.API;

namespace CustomBackgroundAddon.Utilities;

public class Background
{
    public static void Register()
    {
        var settings = Settings.Settings.Instance;

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (!BackgroundImage.Enabled()) return;
            BackgroundImage.Load();
            
            if (!settings.KeepEditorBackground)
            {
                var opacity = (int)Settings.Settings.Instance.BackgroundOpacity;
                var hexValue = (int)(opacity / 100.0 * 255);
                var opacityHex = hexValue.ToString("X2");
                var backgroundBrush = new ImmutableSolidColorBrush(Color.Parse($"#{opacityHex}ffffff"));
                var transparentBrush = new ImmutableSolidColorBrush(Color.Parse("#00ffffff"));
                
                StyleOverrides.Apply("EditorBackgroundColor", transparentBrush);
                StyleOverrides.Apply("TabViewSelectedItemBorderBrush", backgroundBrush);
                StyleOverrides.Apply("TabViewItemHeaderBackgroundSelected", backgroundBrush);
                StyleOverrides.Apply("SkEditorBorderBackground", backgroundBrush);
            }
        });
    }

    public static void Unregister()
    {
        BackgroundImage.Remove();
        StyleOverrides.RemoveAll();
    }
    
    public static void Reload()
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            await SkEditorAPI.Addons.DisableAddon(CustomBackgroundAddon.Instance);
            await SkEditorAPI.Addons.EnableAddon(CustomBackgroundAddon.Instance);
            
            // Unregister();
            // Setup();
            // Register();
        });
    }

    public static void Setup()
    {
        BackgroundImage.Setup();
    }
}