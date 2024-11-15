using System.ComponentModel.DataAnnotations;

namespace ApiServer.ViewModels;

/// <summary>
/// View model for Login form
/// </summary>
public class LoginViewModel
{
    /// <summary>
    /// Username for attempted login
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Password for user
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
    
    /// <summary>
    /// URL to be returned after successful login
    /// </summary>
    public string? ReturnUrl { get; set; }
}