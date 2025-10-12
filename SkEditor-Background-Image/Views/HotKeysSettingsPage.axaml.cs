using BackgroundImageAddon.Utilities.Settings;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using SkEditor.Controls;
using SkEditor.Views.Settings;
using SkEditor.Views;

namespace BackgroundImageAddon.Views;

public partial class HotKeysSettingsPage : UserControl
{
    public HotKeysSettingsPage()
    {
        this.InitializeComponent();

        // this.LoadSettings();
        // this.AssignCommands();
    }

    // private void LoadSettings()
    // {
    //     this.WelcomeScreenHotKeyToggleSwitch.IsChecked = Settings.Instance.HotKeysSettings.EnableWelcomeScreenHotKey;
    // }
    //
    // private void AssignCommands()
    // {
    //     this.WelcomeScreenHotKeyToggleSwitch.Command = new RelayCommand(ToggleWelcomeScreenHotKey);
    //
    //
    //     var backButton = this.GetBackButton();
    //     if (backButton != null)
    //         backButton.Command = new RelayCommand(() =>
    //         {
    //             SettingsWindow.NavigateToPage(typeof(CustomAddonSettingsPage));
    //         });
    // }
    //
    // private void ToggleWelcomeScreenHotKey()
    // {
    //     Settings.Instance.HotKeysSettings.EnableWelcomeScreenHotKey =
    //         !Settings.Instance.HotKeysSettings.EnableWelcomeScreenHotKey;
    //     Settings.Instance.Save();
    // }
    //
    // private Button? GetBackButton()
    // {
    //     return this.FindControl<SettingsTitle>("Title")?.GetBackButton();
    // }
}