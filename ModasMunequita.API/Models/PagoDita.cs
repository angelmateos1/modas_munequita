namespace ModasMunequita.API.Models;

public class PagoDita
{
    public int Id { get; set; }
    public string UsuarioId { get; set; } = string.Empty;
    public Usuario? Usuario { get; set; }

    public decimal Abonado { get; set; }
    public DateTime FechaPago { get; set; } = DateTime.UtcNow;
    public string Concepto { get; set; } = string.Empty; // Ej: "Abono quincena mayo" o "Compra vestido rojo a dita"
}