using System.ComponentModel.DataAnnotations;

namespace ModasMunequita.API.DTOs;

public class PagoDitaDTO
{
    public int Id { get; set; }
    public decimal Abonado { get; set; }
    public DateTime FechaPago { get; set; }
    public string Concepto { get; set; } = string.Empty;
}

public class ClienteResumenDTO
{
    public string Id { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public decimal DeudaActual { get; set; }
    public int DiaCobroDita { get; set; }
    public string NotasAdmin { get; set; } = string.Empty;
    public bool EsClienteActivo { get; set; }
}

public class ClienteDetalleDTO : ClienteResumenDTO
{
    public List<PagoDitaDTO> HistorialPagos { get; set; } = new();
}

public class ActualizarClienteDTO
{
    [Required]
    public string NombreCompleto { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;

    [Range(0, 100000)]
    public decimal DeudaActual { get; set; }

    [Range(1, 31)]
    public int DiaCobroDita { get; set; }

    public string NotasAdmin { get; set; } = string.Empty;
    public bool EsClienteActivo { get; set; }
}

public class RegistrarPagoDTO
{
    [Range(0.01, 100000)]
    public decimal Abonado { get; set; }

    [Required]
    public string Concepto { get; set; } = string.Empty;
}