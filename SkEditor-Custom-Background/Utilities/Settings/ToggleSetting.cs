using Avalonia.Controls;
using Newtonsoft.Json.Linq;
using SkEditor.API.Settings.Types;

namespace CustomBackgroundAddon.Utilities.Settings;

public class ToggleSetting : ISettingType
{
    public object Deserialize(JToken value)
    {
        throw new NotImplementedException();
    }

    public JToken Serialize(object value)
    {
        throw new NotImplementedException();
    }

    public Control CreateControl(object raw, Action<object> onChanged)
    {
        ToggleSwitch toggle = new() { IsChecked = Settings.Instance.KeepEditorBackground };
        toggle.IsCheckedChanged += (_, _) =>
        {
            onChanged(toggle.IsChecked ?? false);
            Settings.Instance.KeepEditorBackground = toggle.IsChecked ?? false;
            Settings.Instance.Save();
            
            Background.Reload();
            
        };
        return toggle;
    }

    public bool IsSelfManaged => true;
}