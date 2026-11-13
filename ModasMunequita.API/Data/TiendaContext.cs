using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModasMunequita.API.Models;

namespace ModasMunequita.API.Data;

public class TiendaContext : IdentityDbContext<Usuario>
{
    public TiendaContext(DbContextOptions<TiendaContext> options) : base(options) { }

    public DbSet<Prenda> Prendas { get; set; }
    public DbSet<PagoDita> PagosDita { get; set; }
}