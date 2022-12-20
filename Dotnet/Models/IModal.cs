namespace Fenrus.Models;

/// <summary>
/// Modal interface
/// </summary>
public interface IModal
{
    /// <summary>
    /// Gets or sets the unique identifier
    /// </summary>
    Guid Uid { get; set; }
    
    /// <summary>
    /// Gets or sets the name
    /// </summary>
    string Name { get; set; }
}