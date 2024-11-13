namespace ApiServer.Api.Users.Models;

/// <summary>
/// Data passed into API for creating/updating user
/// </summary>
/// <param name="Username"></param>
/// <param name="Admin"></param>
/// <param name="Password"></param>
public record UserRequest(string Username, bool Admin, string Password);