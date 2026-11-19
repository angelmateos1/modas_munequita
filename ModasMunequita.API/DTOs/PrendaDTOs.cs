using System.ComponentModel.DataAnnotations;

namespace ModasMunequita.API.DTOs;

public class CrearEditarPrendaDTO
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    [Required]
    public string Categoria { get; set; } = string.Empty;

    [Required]
    public string Talla { get; set; } = string.Empty;

    [Range(0.01, 10000)]
    public decimal Precio { get; set; }

    [Range(0, 10000)]
    public int Stock { get; set; }

    public string ImagenUrl { get; set; } = string.Empty;
}