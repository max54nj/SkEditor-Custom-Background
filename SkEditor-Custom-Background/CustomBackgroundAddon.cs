using System.Reflection;
using CustomBackgroundAddon.Utilities;
using Avalonia.Platform;
using Avalonia.Svg.Skia;
using FluentAvalonia.UI.Controls;
using CustomBackgroundAddon.Utilities.Settings;
using FluentIcons.Avalonia.Fluent;
using FluentIcons.Common;
using SkEditor.API.Settings;
using ToggleSetting = CustomBackgroundAddon.Utilities.Settings.ToggleSetting;

namespace CustomBackgroundAddon;

using SkEditor.API;

public class CustomBackgroundAddon : IAddon
{
    public static CustomBackgroundAddon Instance { get; private set; } = null!;

    private ImageIconSource? _iconSource;

    public string Name => "Custom Background";

    public string Identifier => "de.max54nj.customBackground";

    public string Version =>
        Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ??
        throw new NullReferenceException("Version is null");

    public string? Description => "Custom background images for SkEditor!";

    public Version GetMinimalSkEditorVersion() => new(2, 9, 3);

    public IconSource GetAddonIcon()
    {
        if (this._iconSource != null) return this._iconSource;

        var stream = AssetLoader.Open(new Uri("avares://de.max54nj.customBackground/Assets/Icon.svg"));
        return this._iconSource =
                   new ImageIconSource { Source = new SvgImage { Source = SvgSource.LoadFromStream(stream) } };
    }

    public void OnEnable()
    {
    }

    public async Task OnEnableAsync()
    {
        SkEditorAPI.Logs.Info($"Enabling {Name} Addon...");

        Instance = this;

        Settings.Load();
        
        Utilities.Events.Register();

        Resources.ExtractLanguages();

        await Translation.ChangeLanguage(SkEditorAPI.Core.GetAppConfig().Language);

        Background.Setup();
        Background.Register();

        SkEditorAPI.Logs.Info($"Successfully enabled {Name} Addon!");
    }

    public void OnDisable()
    {
        Background.Unregister();
        Utilities.Events.Unregister();
    }

    public List<Setting> GetSettings()
    {
        List<Setting> settings =
        [
            new(Instance, Translation.Get("SettingsBackgroundImageLabel"), "BackgroundImage",
                null!, new FileSelectSetting(), Translation.Get("SettingsBackgroundImageDescription"),
                new FluentIconSource { Icon = Icon.Image }),
            new(Instance, Translation.Get("SettingsKeepEditorBackgroundLabel"), "KeepEditorBackground",
                false, new ToggleSetting(),
                Translation.Get("SettingsKeepEditorBackgroundDescription"),
                new FluentIconSource { Icon = Icon.ColorBackground }),
            
            // new(Instance, Translation.Get("SettingsTransparentBackgroundLabel"), "TransparentBackground",
            //     false, new ToggleSetting(),
            //     Translation.Get("SettingsTransparentBackgroundDescription"),
            //     new FluentIconSource { Icon = Icon.Eye })
        ];

        return settings;
    }
}