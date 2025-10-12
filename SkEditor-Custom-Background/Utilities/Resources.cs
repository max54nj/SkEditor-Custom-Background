using System.Reflection;

namespace CustomBackgroundAddon.Utilities;

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
                                        .Where(name => name.StartsWith("CustomBackgroundAddon.Languages.") &&
                                                       name.EndsWith(".xaml"));
        foreach (var resourceName in embeddedResources)
        {
            var name = resourceName.Replace("CustomBackgroundAddon.Languages.", "").Replace(".xaml", "");
            var outputPath = Path.Combine(Settings.Settings.AppDataFolderPath, "Languages", $"{name}.xaml");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
            ExtractEmbededResource(resourceName, outputPath);
        }
    }
    
    

    
}