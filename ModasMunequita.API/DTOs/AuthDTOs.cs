using System.ComponentModel.DataAnnotations;

namespace ModasMunequita.API.DTOs;

public class RegistroDTO
{
    [Required]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;
    public int DiaCobroDita { get; set; } = 1;
}

public class LoginDTO
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class AuthRespuestaDTO
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}