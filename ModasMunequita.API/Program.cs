using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModasMunequita.API.Data;
using ModasMunequita.API.Models;
using Microsoft.OpenApi;
using ModasMunequita.API.Data;
using ModasMunequita.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TiendaContext>(options =>
    options.UseSqlite("Data Source=tienda.db"));

builder.Services.AddIdentityCore<Usuario>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<TiendaContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pega aquí solo tu token JWT (sin la palabra Bearer)"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await DbSeeder.InicializarRolesYAdminAsync(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("PermitirAngular");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TiendaContext>();

    // Esta es la línea que faltaba antes de la 99:
    var conPlaceholder = context.Prendas.Where(p => p.ImagenUrl.Contains("placehold.co")).ToList();
    if (conPlaceholder.Any())
    {
        conPlaceholder.ForEach(p => p.ImagenUrl = string.Empty);
        context.SaveChanges();
    }

    if (context.Prendas.Count() <= 1)
    {
        var prendasEjemplo = new List<Prenda>
        {
            new Prenda { Nombre = "Vestido Midi Floral Rosa", Descripcion = "Vestido fluido con estampado de flores, escote en pico y cintura elástica.", Categoria = "Vestidos", Talla = "M", Precio = 34.95m, Stock = 8, ImagenUrl = "" },
            new Prenda { Nombre = "Blusa Satinada Lazo", Descripcion = "Blusa elegante de tacto satinado con lazada al cuello y manga abullonada.", Categoria = "Blusas", Talla = "S", Precio = 24.50m, Stock = 12, ImagenUrl = "" },
            new Prenda { Nombre = "Pantalón Palazzo Crudo", Descripcion = "Pantalón de tiro alto y pernera ancha con caída ligera, ideal para eventos.", Categoria = "Pantalones", Talla = "L", Precio = 32.00m, Stock = 6, ImagenUrl = "" },
            new Prenda { Nombre = "Falda Plisada Pastel", Descripcion = "Falda midi plisada en tono rosa empolvado con cinturilla cómoda.", Categoria = "Faldas", Talla = "M", Precio = 27.90m, Stock = 10, ImagenUrl = "" },
            new Prenda { Nombre = "Chaqueta Tweed Muñequita", Descripcion = "Chaqueta corta estilo tweed con botones dorados y ribete trenzado.", Categoria = "Chaquetas", Talla = "M", Precio = 45.00m, Stock = 4, ImagenUrl = "" },
            new Prenda { Nombre = "Vestido Cóctel Fucsia", Descripcion = "Vestido entallado por la rodilla con volante lateral y espalda descubierta.", Categoria = "Vestidos", Talla = "S", Precio = 39.95m, Stock = 5, ImagenUrl = "" },
            new Prenda { Nombre = "Top Bordado Suizo", Descripcion = "Top blanco de algodón calado con tirantes anchos y detalle de puntilla.", Categoria = "Blusas", Talla = "XS", Precio = 19.95m, Stock = 15, ImagenUrl = "" },
            new Prenda { Nombre = "Vaquero Mom Fit Celeste", Descripcion = "Jeans de corte clásico y tiro alto en lavado claro con 100% algodón.", Categoria = "Pantalones", Talla = "M", Precio = 29.99m, Stock = 9, ImagenUrl = "" },
            new Prenda { Nombre = "Cárdigan Punto Calado", Descripcion = "Rebeca suave de punto fino con botones nacarados para entretiempo.", Categoria = "Chaquetas", Talla = "L", Precio = 26.50m, Stock = 7, ImagenUrl = "" },
            new Prenda { Nombre = "Falda Mini Vichy", Descripcion = "Falda corta de cuadros vichy rosa y blanco con cremallera invisible.", Categoria = "Faldas", Talla = "S", Precio = 22.00m, Stock = 11, ImagenUrl = "" },
            new Prenda { Nombre = "Mono Largo Ceremonia", Descripcion = "Mono entero con escote cruzado, cinturón a juego y pantalón fluido.", Categoria = "Vestidos", Talla = "XL", Precio = 49.90m, Stock = 3, ImagenUrl = "" },
            new Prenda { Nombre = "Bolso Bandolera Acolchado", Descripcion = "Bolso compacto con cadena dorada y cierre magnético en tono crema.", Categoria = "Complementos", Talla = "Única", Precio = 21.50m, Stock = 14, ImagenUrl = "" }
        };

        context.Prendas.AddRange(prendasEjemplo);
        context.SaveChanges();
    }
}

app.Run();