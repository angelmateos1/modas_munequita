using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModasMunequita.API.Data;
using ModasMunequita.API.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Conectar Base de Datos SQLite
builder.Services.AddDbContext<TiendaContext>(options =>
    options.UseSqlite("Data Source=tienda.db"));

// 2. Configurar Identity (Usuarios y Roles: Admin / User)
builder.Services.AddIdentityCore<Usuario>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<TiendaContext>();

// 3. Habilitar Controladores y Swagger (documentación interactiva)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();