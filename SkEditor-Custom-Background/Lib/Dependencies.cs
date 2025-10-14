using SkEditor.API;

namespace CustomBackgroundAddon.Lib;

public class Dependencies
{
    public static void CheckForBookbinder()
    {
        var bookbinder = SkEditorAPI.Addons.GetAddons().FirstOrDefault(x => x.Identifier == "de.max54nj.bookbinder");
        if (bookbinder == null)
        {
            throw new Exception("Bookbinder addon is required for Custom Background addon to work. Please install and enable Bookbinder.");
        }

        var bookBinderState = SkEditorAPI.Addons.GetAddonState(bookbinder);
        if (bookBinderState != IAddons.AddonState.Enabled)
        {
            throw new Exception("Bookbinder addon is required for Custom Background addon to work. Please enable Bookbinder.");
        }
    }
}