using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CustomBackgroundAddon.Utilities;

public class StyleOverrides
{
    private static Dictionary<string, Dictionary<object, object?>?> _originalResources = new();
    private static Dictionary<object, object?> _originalManualResources = new();

    public static void Init()
    {
        if (Application.Current?.Resources == null)
        {
            return;
        }

        foreach (KeyValuePair<object, object?> keyValuePair in Application.Current.Resources)
        {
            _originalManualResources[keyValuePair.Key] = keyValuePair.Value;
        }
    }

    public static void ApplyFile(string fileName)
    {
        if (Application.Current == null) return;

        if (AvaloniaXamlLoader.Load(new Uri($"avares://{CustomBackgroundAddon.Instance.Identifier}/styles/{fileName}.axaml"))
            is not ResourceDictionary styleOverrideResources) return;
        
        _originalResources[fileName] = new Dictionary<object, object?>();
            
        foreach (var key in styleOverrideResources.Keys)
        {
            if (Application.Current.Resources.TryGetResource(key, null, out var originalValue))
            {
                _originalResources[fileName]![key] = originalValue;
            }
            
            Application.Current.Resources[key] = styleOverrideResources[key];
        }
    }

    public static void Apply(object key, object value)
    {
        if (Application.Current == null) return;
        
        Application.Current.Resources[key] = value;
    }
    
    public static void RemoveAll()
    {
        if (Application.Current == null) return;
        
        foreach (var fileName in _originalResources.Keys.ToList())
        {
            RemoveFile(fileName);
        }
        
        foreach (var key in _originalManualResources.Keys.ToList())
        {
            Remove(key);
        }
    }

    public static void RemoveFile(string fileName)
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
    
    public static void Remove(object key)
    {
        if (Application.Current == null) return;
        
        if (!_originalManualResources.TryGetValue(key, out var originalValue)) return;

        if (originalValue != null)
        {
            Application.Current.Resources[key] = originalValue;
        }
        else
        {
            Application.Current.Resources.Remove(key);
        }
        
        _originalManualResources.Remove(key);
    }
}