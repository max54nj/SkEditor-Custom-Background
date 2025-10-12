using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using SkEditor.API;

namespace BackgroundImageAddon.Utilities;

public static class Resources
{
    public static void ExtractEmbededResource(string resourceName, string outputPath)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) throw new FileNotFoundException(resourceName);

        using var fileStream = File.Create(outputPath);
        stream.CopyTo(fileStream);
    }

    public static Stream StreamEmbededResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) throw new FileNotFoundException(resourceName);
        return stream;
    }

    public static void ExtractLanguages()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var embeddedResources = assembly.GetManifestResourceNames()
                                        .Where(name => name.StartsWith("BackgroundImageAddon.Languages.") &&
                                                       name.EndsWith(".xaml"));
        foreach (var resourceName in embeddedResources)
        {
            var name = resourceName.Replace("BackgroundImageAddon.Languages.", "").Replace(".xaml", "");
            var outputPath = Path.Combine(Settings.Settings.AppDataFolderPath, "Languages", $"{name}.xaml");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
            ExtractEmbededResource(resourceName, outputPath);
        }
    }

    public static void ApplyStyleOverrides()
    {
        if (Application.Current == null) return;
        
        var styleOverrideResources =
            AvaloniaXamlLoader.Load(new
                                        Uri($"avares://{BackgroundImageAddon.Instance.Identifier}/styles/StyleOverrides.axaml")) as ResourceDictionary;
        
        if (styleOverrideResources != null)
        {
            foreach (var key in styleOverrideResources.Keys)
            {
                Application.Current.Resources[key] = styleOverrideResources[key];
            }
        }
    }

    public static void LoadBackgroundImageFromAssets()
    {
        var mainWindow = SkEditorAPI.Windows.GetMainWindow();
        if (mainWindow == null) return;

         var uri = new Uri($"avares://{BackgroundImageAddon.Instance.Identifier}/Assets/background.png");
         var assetLoader = AssetLoader.Open(uri);
         var bitmap = new Bitmap(assetLoader);
         var brush = new ImageBrush(bitmap)
         {
             Stretch = Stretch.UniformToFill, AlignmentX = AlignmentX.Center, AlignmentY = AlignmentY.Center,
         };

         mainWindow.Background = brush;
    }

    public static void LoadBackgroundImageFromAppData()
    {
        var mainWindow = SkEditorAPI.Windows.GetMainWindow();
        if (mainWindow == null) return;
        
        var filePath = Path.Combine(Settings.Settings.AppDataFolderPath, "background.png");
        if (!File.Exists(filePath)) return;

        var bitmap = new Bitmap(filePath);
        var brush = new ImageBrush(bitmap)
        {
            Stretch = Stretch.UniformToFill,
            AlignmentX = AlignmentX.Center,
            AlignmentY = AlignmentY.Center
        };

        mainWindow.Background = brush;
    }
}