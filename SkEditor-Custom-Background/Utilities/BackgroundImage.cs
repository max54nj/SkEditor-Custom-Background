using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using SkEditor.API;
using SkEditor.Utilities.Styling;

namespace CustomBackgroundAddon.Utilities;

public class BackgroundImage
{
    private static IBrush? _originalBackground;

    public static void Load()
    {
        var mainWindow = SkEditorAPI.Windows.GetMainWindow();
        if (mainWindow == null) return;

        var filePath = Path.Combine(Settings.Settings.Instance.CurrentBackgroundPath);
        if (!File.Exists(filePath)) return;

        if (_originalBackground == null)
        {
            _originalBackground = mainWindow.Background;
        }

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
            await ThemeEditor.ReloadCurrentTheme();
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