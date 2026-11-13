using Microsoft.AspNetCore.Identity;

namespace ModasMunequita.API.Models;

public class Usuario : IdentityUser
{
    public string NombreCompleto { get; set; } = string.Empty;
    public decimal DeudaActual { get; set; } = 0;
    public int DiaCobroDita { get; set; } = 1; // Día del mes (1 al 31) en que se le cobra
    public string NotasAdmin { get; set; } = string.Empty; // Notas internas sobre el cliente
    public bool EsClienteActivo { get; set; } = true;

    // Relación: Un usuario tiene muchos pagos registrados en su historial
    public ICollection<PagoDita> HistorialPagos { get; set; } = new List<PagoDita>();
}