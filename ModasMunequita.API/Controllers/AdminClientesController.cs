using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModasMunequita.API.Data;
using ModasMunequita.API.DTOs;
using ModasMunequita.API.Models;

namespace ModasMunequita.API.Controllers;

[ApiController]
[Route("api/admin/clientes")]
[Authorize(Roles = "Admin")]
public class AdminClientesController : ControllerBase
{
    private readonly TiendaContext _context;
    private readonly UserManager<Usuario> _userManager;

    public AdminClientesController(TiendaContext context, UserManager<Usuario> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteResumenDTO>>> ObtenerClientes(
        [FromQuery] int? diaCobro)
    {
        var usuariosClientes = await _userManager.GetUsersInRoleAsync("User");
        var query = usuariosClientes.AsQueryable();

        if (diaCobro.HasValue)
            query = query.Where(u => u.DiaCobroDita == diaCobro.Value);

        var resultado = query.Select(u => new ClienteResumenDTO
        {
            Id = u.Id,
            NombreCompleto = u.NombreCompleto,
            Email = u.Email ?? string.Empty,
            Telefono = u.PhoneNumber ?? string.Empty,
            DeudaActual = u.DeudaActual,
            DiaCobroDita = u.DiaCobroDita,
            NotasAdmin = u.NotasAdmin,
            EsClienteActivo = u.EsClienteActivo
        }).ToList();

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDetalleDTO>> ObtenerClienteDetalle(string id)
    {
        var usuario = await _context.Users
            .Include(u => u.HistorialPagos)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (usuario == null)
            return NotFound(new { mensaje = "Cliente no encontrado." });

        var detalle = new ClienteDetalleDTO
        {
            Id = usuario.Id,
            NombreCompleto = usuario.NombreCompleto,
            Email = usuario.Email ?? string.Empty,
            Telefono = usuario.PhoneNumber ?? string.Empty,
            DeudaActual = usuario.DeudaActual,
            DiaCobroDita = usuario.DiaCobroDita,
            NotasAdmin = usuario.NotasAdmin,
            EsClienteActivo = usuario.EsClienteActivo,
            HistorialPagos = usuario.HistorialPagos
                .OrderByDescending(p => p.FechaPago)
                .Select(p => new PagoDitaDTO
                {
                    Id = p.Id,
                    Abonado = p.Abonado,
                    FechaPago = p.FechaPago,
                    Concepto = p.Concepto
                }).ToList()
        };

        return Ok(detalle);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarCliente(string id, [FromBody] ActualizarClienteDTO dto)
    {
        var usuario = await _context.Users.FindAsync(id);
        if (usuario == null)
            return NotFound(new { mensaje = "Cliente no encontrado." });

        usuario.NombreCompleto = dto.NombreCompleto;
        usuario.PhoneNumber = dto.Telefono;
        usuario.DeudaActual = dto.DeudaActual;
        usuario.DiaCobroDita = dto.DiaCobroDita;
        usuario.NotasAdmin = dto.NotasAdmin;
        usuario.EsClienteActivo = dto.EsClienteActivo;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/pagos")]
    public async Task<ActionResult<PagoDitaDTO>> RegistrarPago(string id, [FromBody] RegistrarPagoDTO dto)
    {
        var usuario = await _context.Users.FindAsync(id);
        if (usuario == null)
            return NotFound(new { mensaje = "Cliente no encontrado." });

        var nuevoPago = new PagoDita
        {
            UsuarioId = usuario.Id,
            Abonado = dto.Abonado,
            Concepto = dto.Concepto,
            FechaPago = DateTime.UtcNow
        };

        usuario.DeudaActual = Math.Max(0, usuario.DeudaActual - dto.Abonado);

        _context.PagosDita.Add(nuevoPago);
        await _context.SaveChangesAsync();

        return Ok(new PagoDitaDTO
        {
            Id = nuevoPago.Id,
            Abonado = nuevoPago.Abonado,
            FechaPago = nuevoPago.FechaPago,
            Concepto = nuevoPago.Concepto
        });
    }
}