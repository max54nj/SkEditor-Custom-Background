using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SkEditor.API;

namespace BackgroundImageAddon.Utilities;

public class StyleOverrides
{
    private static Dictionary<string, Dictionary<object, object?>?> _originalResources = new();

    public static void Apply(string fileName)
    {
        if (Application.Current == null) return;

        var styleOverrideResources =
            AvaloniaXamlLoader.Load(new
                                        Uri($"avares://{BackgroundImageAddon.Instance.Identifier}/styles/{fileName}.axaml")) as ResourceDictionary;

        if (styleOverrideResources != null)
        {
            _originalResources[fileName] = new Dictionary<object, object?>();
            
            foreach (var key in styleOverrideResources.Keys)
            {
                if (Application.Current.Resources.TryGetResource(key, null, out var originalValue))
                {
                    _originalResources[fileName]![key] = originalValue;
                }
            
                if (Application.Current.Resources.ContainsKey(key))
                {
                    Application.Current.Resources.Remove(key);
                }
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
        
        if (!_originalResources.TryGetValue(fileName, out Dictionary<object, object?>? originalResources) || originalResources == null) return;

        foreach (var (key, originalValue) in originalResources)
        {
            SkEditorAPI.Logs.Info("Restoring resource: " + key);
            if (Application.Current.Resources.ContainsKey(key))
            {
                Application.Current.Resources.Remove(key);
            }
        
            if (originalValue != null)
            {
                Application.Current.Resources.Add(key, originalValue);
            }
        }
    
        _originalResources[fileName] = null;
    }
}