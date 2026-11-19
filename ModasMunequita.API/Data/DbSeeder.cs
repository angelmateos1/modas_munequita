using Microsoft.AspNetCore.Identity;
using ModasMunequita.API.Models;

namespace ModasMunequita.API.Data;

public static class DbSeeder
{
    public static async Task InicializarRolesYAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();

        string[] roles = ["Admin", "User"];
        foreach (var rol in roles)
        {
            if (!await roleManager.RoleExistsAsync(rol))
            {
                await roleManager.CreateAsync(new IdentityRole(rol));
            }
        }

        var adminEmail = "admin@modasmunequita.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var nuevoAdmin = new Usuario
            {
                UserName = adminEmail,
                Email = adminEmail,
                NombreCompleto = "Administradora Modas Muñequita",
                EsClienteActivo = true
            };

            var resultado = await userManager.CreateAsync(nuevoAdmin, "Admin123!");
            if (resultado.Succeeded)
            {
                await userManager.AddToRoleAsync(nuevoAdmin, "Admin");
            }
        }
    }
}