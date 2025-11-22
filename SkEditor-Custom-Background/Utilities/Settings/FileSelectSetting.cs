using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using Newtonsoft.Json.Linq;
using SkEditor.API;
using SkEditor.API.Settings;
using SkEditor.API.Settings.Types;
using SkEditor.Views.Settings;
using Symbol = FluentIcons.Common.Symbol;
using SymbolIcon = FluentIcons.Avalonia.SymbolIcon;

namespace CustomBackgroundAddon.Utilities.Settings;

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
        var stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };

        var comboBox = new ComboBox { PlaceholderText = "Select" };
        comboBox.Items.Add(new ComboBoxItem()
        {
            Content = "None",
            Tag = "none"
        });
        
        var files = Directory.GetFiles(Settings.BackgroundFolderPath);

        foreach (var file in files)
        {
            var comboBoxItem = new ComboBoxItem
            {
                Content = Path.GetFileName(file),
                Tag = Uri.UnescapeDataString(file)
            };
                
            comboBox.Items.Add(comboBoxItem);
        }

        comboBox.SelectedItem = files.Contains(Settings.Instance.CurrentBackgroundPath) ? Settings.Instance.CurrentBackgroundPath : "none";
        

        comboBox.SelectionChanged += (_, _) =>
        {
            var selectedPath = (comboBox.SelectedItem as ComboBoxItem)?.Tag as string;
            if (selectedPath == "none") selectedPath = null;
            
            Settings.Instance.CurrentBackgroundPath = selectedPath ?? "";
            Settings.Instance.Save();
            CustomAddonSettingsPage.Load(CustomBackgroundAddon.Instance);
            
            Background.Reload();
        };

        var button = new Button { Content = new SymbolIcon() { Symbol = Symbol.Add }, Command = new RelayCommand(async () =>
         {
             var filePath = await SkEditorAPI.Windows.AskForFile(new FilePickerOpenOptions()
             {
                 Title =
                     Translation
                         .Get("SettingsBackgroundImageSelectWindowTitle"),
                 AllowMultiple = false,
                 FileTypeFilter = [
                     new FilePickerFileType("Supported Image Formats")
                     {
                         Patterns = ["*.png", "*.jpg", "*.jpeg", "*.webp", "*.gif", "*.ico", "*.bmp"]
                     }
                 ]
             });

             if (filePath is null)
             {
                 return;
             }
                
             filePath = Uri.UnescapeDataString(filePath);
                
             File.Copy(filePath, Path.Join(Settings.BackgroundFolderPath, Path.GetFileName(filePath)), false);

             Settings.Instance.CurrentBackgroundPath = filePath;
             Settings.Instance.Save();
             CustomAddonSettingsPage.Load(CustomBackgroundAddon.Instance);
             Background.Reload();
         })
        };

        stackPanel.Children.Add(comboBox);
        stackPanel.Children.Add(button);

        return stackPanel;
    }

    public void SetupExpander(SettingsExpander expander, Setting setting)
    {
    }

    public bool IsSelfManaged => true;
}