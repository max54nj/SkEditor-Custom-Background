using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.Immutable;
using Avalonia.Threading;
using SkEditor.API;
using SkEditor.Utilities.Styling;

namespace CustomBackgroundAddon.Utilities;

public static class BackgroundImage
{
    private static IBrush? _originalBackground;

    public static void Load()
    {
        var mainWindow = SkEditorAPI.Windows.GetMainWindow();
        if (mainWindow == null)
        {
            SkEditorAPI.Logs.Warning("MainWindow is null, cannot load background image.");
            return;
        }

        var filePath = Path.Combine(Settings.Settings.Instance.CurrentBackgroundPath);
        if (!File.Exists(filePath))
        {
            SkEditorAPI.Logs.Warning("File not found: " + filePath);
            return;
        }

        _originalBackground ??= mainWindow.Background;

        var bitmap = new Bitmap(filePath);
        var brush = new ImageBrush(bitmap)
        {
            Stretch = Stretch.UniformToFill,
            AlignmentX = AlignmentX.Center,
            AlignmentY = AlignmentY.Center
        };

        mainWindow.Background = brush;
    }

    public static void Remove()
    {
        var mainWindow = SkEditorAPI.Windows.GetMainWindow();
        if (mainWindow == null) return;
        Dispatcher.UIThread.Post(async () =>
        {
            // await ThemeEditor.ReloadCurrentTheme();
            mainWindow.Background = ThemeEditor.CurrentTheme.BackgroundColor;
        });
        _originalBackground = null;
    }

    public static bool Enabled()
    {
        return Settings.Settings.Instance.CurrentBackgroundPath != "";
    }

    public static void Setup()
    {
        var directoryExists = Directory.Exists(Settings.Settings.BackgroundFolderPath);
        if (!directoryExists) Directory.CreateDirectory(Settings.Settings.BackgroundFolderPath);
    }
}