using Avalonia.Media;
using Avalonia.Media.Imaging;
using SkEditor.API;

namespace BackgroundImageAddon.Utilities;

public class BackgroundImage
{
    private static IBrush? _originalBackground;

    public static void Load()
    {
        var mainWindow = SkEditorAPI.Windows.GetMainWindow();
        if (mainWindow == null) return;

        var filePath = Path.Combine(Settings.Settings.AppDataFolderPath, "background.png");
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

        mainWindow.Background = _originalBackground;
        _originalBackground = null;
    }

    public static bool Exists()
    {
        return File.Exists(Path.Combine(Settings.Settings.AppDataFolderPath, "background.png"));
    }
}