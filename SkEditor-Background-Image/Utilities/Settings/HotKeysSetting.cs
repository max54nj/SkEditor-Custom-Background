using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Newtonsoft.Json.Linq;
using SkEditor.API.Settings;
using SkEditor.API.Settings.Types;
using BackgroundImageAddon.Views;
using SkEditor.Views;

namespace BackgroundImageAddon.Utilities.Settings;

public class HotKeysSetting : ISettingType
{
    public object Deserialize(JToken value)
    {
        throw new NotImplementedException();
    }

    public JToken Serialize(object value)
    {
        throw new NotImplementedException();
    }

    public Control CreateControl(object value, Action<object> onChanged)
    {
        return new Control();
    }

    public void SetupExpander(SettingsExpander expander, Setting setting)
    {
        expander.IsClickEnabled = true;
        expander.Click += (_, _) => SettingsWindow.NavigateToPage(typeof(HotKeysSettingsPage));
    }

    public bool IsSelfManaged => true;
}

public class HotKeySettings
{
    public bool EnableWelcomeScreenHotKey { get; set; } = true;
}