using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Newtonsoft.Json.Linq;
using SkEditor.API.Settings;
using SkEditor.Views;

namespace CustomBackgroundAddon.Utilities.Settings;

public class GroupSetting
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
        expander.Click += (_, _) => SettingsWindow.NavigateToPage(typeof(TerminalAddonTerminalsSettingsPage));
    }

    public bool IsSelfManaged => true;
}