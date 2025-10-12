using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CustomBackgroundAddon.Utilities;

public class StyleOverrides
{
    private static Dictionary<string, Dictionary<object, object?>?> _originalResources = new();

    public static void Apply(string fileName)
    {
        if (Application.Current == null) return;

        if (AvaloniaXamlLoader.Load(new
                                        Uri($"avares://{CustomBackgroundAddon.Instance.Identifier}/styles/{fileName}.axaml")) is ResourceDictionary styleOverrideResources)
        {
            _originalResources[fileName] = new Dictionary<object, object?>();
            
            foreach (var key in styleOverrideResources.Keys)
            {
                if (Application.Current.Resources.TryGetResource(key, null, out var originalValue))
                {
                    _originalResources[fileName]![key] = originalValue;
                }
            
                Application.Current.Resources.Remove(key);
                Application.Current.Resources.Add(key, styleOverrideResources[key]);
            }
        }
    }
    
    public static void RemoveAll()
    {
        if (Application.Current == null) return;
        
        foreach (var fileName in _originalResources.Keys.ToList())
        {
            Remove(fileName);
        }
    }

    public static void Remove(string fileName)
    {
        if (Application.Current == null) return;
        
        if (!_originalResources.TryGetValue(fileName, out var originalResources) || originalResources == null) return;

        foreach (var (key, originalValue) in originalResources)
        {
            Application.Current.Resources.Remove(key);

            if (originalValue != null)
            {
                Application.Current.Resources.Add(key, originalValue);
            }
        }
    
        _originalResources[fileName] = null;
    }
}