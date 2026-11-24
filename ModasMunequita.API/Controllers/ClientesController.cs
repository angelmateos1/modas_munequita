using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModasMunequita.API.Data;
using ModasMunequita.API.DTOs;
using ModasMunequita.API.Models;

namespace ModasMunequita.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientesController : ControllerBase
{
    private readonly TiendaContext _context;
    private readonly UserManager<Usuario> _userManager;

    public ClientesController(TiendaContext context, UserManager<Usuario> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("mi-dita")]
    public async Task<IActionResult> ObtenerMiDita()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var usuario = await _context.Users
            .Include(u => u.HistorialPagos)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (usuario == null)
            return NotFound(new { mensaje = "Clienta no encontrada." });

        return Ok(new
        {
            usuario.Id,
            usuario.NombreCompleto,
            Email = usuario.Email ?? string.Empty,
            Telefono = usuario.PhoneNumber ?? string.Empty,
            usuario.DeudaActual,
            usuario.DiaCobroDita,
            usuario.EsClienteActivo,
            HistorialPagos = usuario.HistorialPagos
                .OrderByDescending(p => p.FechaPago)
                .Select(p => new PagoDitaDTO
                {
                    Id = p.Id,
                    Abonado = p.Abonado,
                    FechaPago = p.FechaPago,
                    Concepto = p.Concepto
                }).ToList()
        });
    }

    [HttpGet("perfil")]
    public async Task<IActionResult> ObtenerPerfil()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var usuario = await _userManager.FindByIdAsync(userId);
        if (usuario == null)
            return NotFound(new { mensaje = "Usuario no encontrado." });

        return Ok(new
        {
            usuario.Id,
            usuario.NombreCompleto,
            Email = usuario.Email ?? string.Empty,
            Telefono = usuario.PhoneNumber ?? string.Empty
        });
    }

    [HttpPut("perfil")]
    public async Task<IActionResult> ActualizarPerfil([FromBody] ActualizarPerfilDTO dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var usuario = await _userManager.FindByIdAsync(userId);
        if (usuario == null)
            return NotFound(new { mensaje = "Usuario no encontrado." });

        if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != usuario.Email)
        {
            var existente = await _userManager.FindByEmailAsync(dto.Email);
            if (existente != null && existente.Id != usuario.Id)
                return BadRequest(new { mensaje = "Ese correo electrónico ya está registrado." });

            usuario.Email = dto.Email;
            usuario.UserName = dto.Email;
        }

        usuario.NombreCompleto = dto.NombreCompleto;
        usuario.PhoneNumber = dto.Telefono;

        var resultado = await _userManager.UpdateAsync(usuario);
        if (!resultado.Succeeded)
            return BadRequest(new { mensaje = "No se pudieron actualizar los datos." });

        if (!string.IsNullOrWhiteSpace(dto.NuevaPassword))
        {
            if (string.IsNullOrWhiteSpace(dto.PasswordActual))
                return BadRequest(new { mensaje = "Debes escribir tu contraseña actual para cambiarla." });

            var cambioPass = await _userManager.ChangePasswordAsync(usuario, dto.PasswordActual, dto.NuevaPassword);
            if (!cambioPass.Succeeded)
                return BadRequest(new { mensaje = "La contraseña actual es incorrecta o la nueva no cumple los requisitos (mín. 6 caracteres, mayúscula, número y símbolo)." });
        }

        return Ok(new
        {
            usuario.Id,
            usuario.NombreCompleto,
            Email = usuario.Email ?? string.Empty,
            Telefono = usuario.PhoneNumber ?? string.Empty
        });
    }
}

public class ActualizarPerfilDTO
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? PasswordActual { get; set; }
    public string? NuevaPassword { get; set; }
}