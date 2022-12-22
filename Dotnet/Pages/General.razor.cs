using Fenrus.Models.UiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fenrus.Pages;

/// <summary>
/// General user settings page
/// </summary>
[Authorize]
public partial class General : UserPage
{
    GeneralSettingsModel Model { get; set; } = new();

    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    private string Background { get; set; }

    private string InitialImage;

    protected override async Task PostGotUser()
    {
        this.Model = new()
        {
            Theme = Settings.Theme,
            AccentColor = Settings.AccentColor,
            GroupTitles = Settings.ShowGroupTitles,
            LinkTarget = Settings.LinkTarget,
            ShowIndicators = Settings.ShowStatusIndicators
        };
        
        if (string.IsNullOrEmpty(Settings.BackgroundImage) == false)
        {
            if (Settings.BackgroundImage.StartsWith("db:/image/"))
            {
                // db image
                InitialImage = "/fimage/" + Settings.BackgroundImage["db:/image/".Length..];
            }
            else
            {
                InitialImage = Settings.BackgroundImage;
            }
        }
    }

    private async Task Save()
    {
        if (Background == string.Empty)
        {
            if(string.IsNullOrEmpty(Settings.BackgroundImage) == false)
                ImageHelper.DeleteImage(Settings.BackgroundImage);
            Settings.BackgroundImage = string.Empty;
        }
        else
        {
            string bkg = ImageHelper.SaveImageFromBase64(Background);
            if (string.IsNullOrEmpty(bkg) == false)
            {
                if (string.IsNullOrEmpty(Settings.BackgroundImage) == false)
                    ImageHelper.DeleteImage(Settings.BackgroundImage);
                Settings.BackgroundImage = bkg;
            }
        }

        Settings.AccentColor = Model.AccentColor;
        Settings.LinkTarget = Model.LinkTarget;
        Settings.Theme = Model.Theme;
        Settings.ShowGroupTitles = Model.GroupTitles;
        Settings.ShowStatusIndicators = Model.ShowIndicators;
        new Services.UserSettingsService().Save(Settings);
        App.UpdateAccentColor(Settings.AccentColor);
        ToastService.ShowSuccess("Settings Saved");
    }
}