namespace ModasMunequita.API.Models;

public class Prenda
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty; // Ej: Vestidos, Blusas, Pantalones
    public string Talla { get; set; } = string.Empty;     // Ej: S, M, L, XL
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string ImagenUrl { get; set; } = string.Empty; // Para cuando conectemos AWS S3
}