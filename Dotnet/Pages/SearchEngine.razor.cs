using Microsoft.AspNetCore.Components;

namespace Fenrus.Pages;

/// <summary>
/// Search engine page
/// </summary>
public partial class SearchEngine: UserPage
{
    
    Models.SearchEngine Model { get; set; } = new();

    [Parameter]
    public string UidString
    {
        get => Uid.ToString();
        set
        {
            if (Guid.TryParse(value, out Guid guid))
                this.Uid = guid;
            else
            {
                // do a redirect
            }
        }
    }

    /// <summary>
    /// Gets or sets the UID of the group
    /// </summary>
    public Guid Uid { get; set; }

    void Save()
    {
    }

    void Cancel()
    {
        this.Router.NavigateTo("/settings/search-engines");
    }
}