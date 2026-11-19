using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModasMunequita.API.Data;
using ModasMunequita.API.DTOs;
using ModasMunequita.API.Models;

namespace ModasMunequita.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrendasController : ControllerBase
{
    private readonly TiendaContext _context;

    public PrendasController(TiendaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Prenda>>> ObtenerPrendas(
        [FromQuery] string? categoria,
        [FromQuery] string? talla)
    {
        var query = _context.Prendas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(categoria))
            query = query.Where(p => p.Categoria.ToLower() == categoria.ToLower());

        if (!string.IsNullOrWhiteSpace(talla))
            query = query.Where(p => p.Talla.ToLower() == talla.ToLower());

        return Ok(await query.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Prenda>> ObtenerPrenda(int id)
    {
        var prenda = await _context.Prendas.FindAsync(id);
        if (prenda == null)
            return NotFound(new { mensaje = "Prenda no encontrada." });

        return Ok(prenda);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Prenda>> CrearPrenda([FromBody] CrearEditarPrendaDTO dto)
    {
        var nuevaPrenda = new Prenda
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Categoria = dto.Categoria,
            Talla = dto.Talla,
            Precio = dto.Precio,
            Stock = dto.Stock,
            ImagenUrl = dto.ImagenUrl
        };

        _context.Prendas.Add(nuevaPrenda);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerPrenda), new { id = nuevaPrenda.Id }, nuevaPrenda);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActualizarPrenda(int id, [FromBody] CrearEditarPrendaDTO dto)
    {
        var prenda = await _context.Prendas.FindAsync(id);
        if (prenda == null)
            return NotFound(new { mensaje = "Prenda no encontrada." });

        prenda.Nombre = dto.Nombre;
        prenda.Descripcion = dto.Descripcion;
        prenda.Categoria = dto.Categoria;
        prenda.Talla = dto.Talla;
        prenda.Precio = dto.Precio;
        prenda.Stock = dto.Stock;
        prenda.ImagenUrl = dto.ImagenUrl;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EliminarPrenda(int id)
    {
        var prenda = await _context.Prendas.FindAsync(id);
        if (prenda == null)
            return NotFound(new { mensaje = "Prenda no encontrada." });

        _context.Prendas.Remove(prenda);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}