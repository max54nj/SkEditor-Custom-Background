using System.Reflection;
using BackgroundImageAddon.Utilities;
using Avalonia.Platform;
using Avalonia.Svg.Skia;
using FluentAvalonia.UI.Controls;
using BackgroundImageAddon.Utilities.Settings;
using FluentIcons.Avalonia.Fluent;
using FluentIcons.Common;
using SkEditor.API.Settings;
using SkEditor.API.Settings.Types;

namespace BackgroundImageAddon;

using SkEditor.API;

public class BackgroundImageAddon : IAddon
{
    public static BackgroundImageAddon Instance { get; private set; } = null!;

    private ImageIconSource? _iconSource;

    public string Name => "Background Image";

    public string Identifier => "de.max54nj.background-image";

    public string Version =>
        Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ??
        throw new NullReferenceException("Version is null");

    public string? Description => "Set a custom background image for the editor.";

    public Version GetMinimalSkEditorVersion() => new(2, 9, 3);

    public IconSource GetAddonIcon()
    {
        if (this._iconSource != null) return this._iconSource;

        var stream = AssetLoader.Open(new Uri("avares://de.max54nj.background-image/Assets/Terminal.svg"));
        return this._iconSource =
                   new ImageIconSource { Source = new SvgImage { Source = SvgSource.LoadFromStream(stream) } };
    }

    public void OnEnable()
    {
    }

    public async Task OnEnableAsync()
    {
        SkEditorAPI.Logs.Info("Enabling Background Image Addon...");

        Instance = this;

        Settings.Load();

        Resources.ExtractLanguages();

        await Translation.ChangeLanguage(SkEditorAPI.Core.GetAppConfig().Language);

        Utilities.Events.Register();
        
        Resources.ApplyStyleOverrides();
        
        Resources.LoadBackgroundImageFromAssets();

        SkEditorAPI.Logs.Info("Successfully enabled Background Image Addon!");
    }

    public void OnDisable()
    {
        Utilities.Events.Unregister();
    }

    public List<Setting> GetSettings()
    {
        List<Setting> settings =
        [
            new(Instance, Translation.Get("SettingsBackgroundImageLabel"), "BackgroundImage",
                true, new FileSelectSetting(), Translation.Get("SettingsBackgroundImageDescription"),
                new FluentIconSource { Icon = Icon.Image }),
        ];

        return settings;
    }
}