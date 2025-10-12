using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using Newtonsoft.Json.Linq;
using SkEditor.API;
using SkEditor.API.Settings;
using SkEditor.API.Settings.Types;

namespace BackgroundImageAddon.Utilities.Settings;

public class FileSelectSetting : ISettingType
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
        return new Button()
        {
            Content = Translation.Get("SettingsBackgroundImageSelectButtonLabel"),
            Command = new RelayCommand(async () =>
            {
                string? filePath = await SkEditorAPI.Windows.AskForFile(new FilePickerOpenOptions()
                {
                    Title =
                        Translation
                            .Get("SettingsBackgroundImageSelectWindowTitle"),
                    AllowMultiple = false,
                    FileTypeFilter = [
                        new FilePickerFileType("Supported Image Formats")
                        {
                            Patterns = ["*.png", "*.jpg", "*.jpeg"]
                        }
                    ]
                });

                if (filePath is null)
                {
                    return;
                }
                
                File.Copy(filePath, Path.Join(Settings.AppDataFolderPath, "background.png"), true);
                
                Dispatcher.UIThread.Post(async void () => await Background.Reload());
            })
        };
    }

    public void SetupExpander(SettingsExpander expander, Setting setting)
    {
    }

    public bool IsSelfManaged => true;
}