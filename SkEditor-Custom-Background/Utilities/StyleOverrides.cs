using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SkEditor;

namespace CustomBackgroundAddon.Utilities;

public class StyleOverrides
{
    private static Dictionary<string, object?> _originalResources = new();

    public static void Apply(string[] keys, object? value)
    {
        foreach (var key in keys)
        {
            Apply(key, value);
        }
    }
    public static void Apply(string key, object? value)
    {
        if (Application.Current == null) return;
        
        if (Application.Current.Resources.TryGetResource(key, null, out var originalValue))
        {
            _originalResources.TryAdd(key, originalValue);
        }
        
        Application.Current.Resources.Remove(key);
        Application.Current.Resources.Add(key, value);
    }
    
    public static void RestoreAll()
    {
        if (Application.Current == null) return;

        foreach (var (key, _) in _originalResources)
        {
            Restore(key);
        }
    }

    public static void Restore(string key)
    {
        if (Application.Current == null) return;

        if (_originalResources.TryGetValue(key, out var originalValue))
        {
            Application.Current.Resources.Remove(key);
            Application.Current.Resources.Add(key, originalValue);
            _originalResources.Remove(key);
        }
    }
}