using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ModasMunequita.API.DTOs;
using ModasMunequita.API.Models;

namespace ModasMunequita.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<Usuario> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<Usuario> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Registrar([FromBody] RegistroDTO dto)
    {
        var usuarioExiste = await _userManager.FindByEmailAsync(dto.Email);
        if (usuarioExiste != null)
            return BadRequest(new { mensaje = "Este correo ya está registrado." });

        var nuevoUsuario = new Usuario
        {
            UserName = dto.Email,
            Email = dto.Email,
            NombreCompleto = dto.NombreCompleto,
            PhoneNumber = dto.Telefono,
            DiaCobroDita = dto.DiaCobroDita,
            DeudaActual = 0,
            EsClienteActivo = true
        };

        var resultado = await _userManager.CreateAsync(nuevoUsuario, dto.Password);
        if (!resultado.Succeeded)
            return BadRequest(resultado.Errors);

        await _userManager.AddToRoleAsync(nuevoUsuario, "User");

        var token = GenerarTokenJWT(nuevoUsuario, "User");

        return Ok(new AuthRespuestaDTO
        {
            Token = token,
            Email = nuevoUsuario.Email!,
            NombreCompleto = nuevoUsuario.NombreCompleto,
            Rol = "User"
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var usuario = await _userManager.FindByEmailAsync(dto.Email);
        if (usuario == null || !await _userManager.CheckPasswordAsync(usuario, dto.Password))
            return Unauthorized(new { mensaje = "Correo o contraseña incorrectos." });

        var roles = await _userManager.GetRolesAsync(usuario);
        var rolPrincipal = roles.FirstOrDefault() ?? "User";

        var token = GenerarTokenJWT(usuario, rolPrincipal);

        return Ok(new AuthRespuestaDTO
        {
            Token = token,
            Email = usuario.Email!,
            NombreCompleto = usuario.NombreCompleto,
            Rol = rolPrincipal
        });
    }

    private string GenerarTokenJWT(Usuario usuario, string rol)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id),
            new Claim(ClaimTypes.Email, usuario.Email!),
            new Claim(ClaimTypes.Name, usuario.NombreCompleto),
            new Claim(ClaimTypes.Role, rol)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}