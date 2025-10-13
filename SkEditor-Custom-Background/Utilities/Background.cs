using Avalonia.Media;
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
            StyleOverrides.Apply(
                ["EditorBackgroundColor", "TabViewSelectedItemBorderBrush", "TabViewItemHeaderBackgroundSelected"], 
                new SolidColorBrush(Color.Parse("#10ffffff"))
            );
        }
    }

    public static void Unregister()
    {
        StyleOverrides.RestoreAll();
        
        BackgroundImage.Remove();
    }

    public static void Setup()
    {
        BackgroundImage.Setup();
    }
}