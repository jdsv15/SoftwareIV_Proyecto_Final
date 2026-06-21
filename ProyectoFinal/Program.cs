using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Data;
using ProyectoFinal.Models;
using ProyectoFinal.Repository;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la conexión a SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configuración de Identity usando ApplicationUser
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUnidadTrabajo, UnidadTrabajo>();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Seguridad Autenticacion
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// Ruteo para las Áreas
app.MapAreaControllerRoute(
    name: "areaAdministracion",
    areaName: "Administracion",
    pattern: "Administracion/{controller=Admin}/{action=Especialidades}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Ruteo por defecto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicializacion y Semillero de Datos (Seeding)
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Roles
    string[] roles = { "Administrador", "Medico", "Paciente" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Crear usuario Administrador por defecto si no existe
    var adminEmail = "admin@gmail.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        var nuevoAdmin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            Nombre = "Admin", 
            Cedula = "1",               
            Rol = "Administrador",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(nuevoAdmin, "Admin1234!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(nuevoAdmin, "Administrador");
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(adminUser, "Administrador"))
        {
            await userManager.AddToRoleAsync(adminUser, "Administrador");
        }
    }
}

app.Run();