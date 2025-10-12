using SkEditor.API;

namespace BackgroundImageAddon.Utilities;

public class Background
{
    public static void Register()
    {
        var settings = Settings.Settings.Instance;
        
        BackgroundImage.Load();
        StyleOverrides.Apply();

        // if (settings.TransparentBackground)
        // {
        //     StyleOverrides.Apply("AppBackground"); // Transparent background for main window
        //     
        //     if (!settings.KeepEditorBackground)
        //     {
        //         StyleOverrides.Apply("EditorBackground"); // Transparent background for editor area
        //     }
        // } else if (BackgroundImage.Exists())
        // {
        //     BackgroundImage.Load();
        //     
        //     if (!settings.KeepEditorBackground)
        //     {
        //         StyleOverrides.Apply("EditorBackground"); // Transparent background for editor area
        //     }
        // }
        
        // if (settings.KeepEditorBackground)
        // {
        //     BackgroundImage.Load();
        //     
        //     StyleOverrides.Apply("EditorBackground");    
        // }
        
        // BackgroundImage.Load();
        
        // StyleOverrides.Apply("AppBackground"); // Transparent background for main window
        
        // StyleOverrides.Apply("EditorBackground"); // Transparent background for editor area
    }

    public static void Unregister()
    {
        // StyleOverrides.RemoveAll();
        
        BackgroundImage.Remove();
        StyleOverrides.Remove();
    }
    
    public static async Task Reload()
    {
        Unregister();
        
        await Task.Delay(500);
        
        Register();
    }
}