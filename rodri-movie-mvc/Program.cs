using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using rodri_movie_mvc.Data;
using rodri_movie_mvc.Models;
using rodri_movie_mvc.Service;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//incluir contexto
builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MovieDbContext")));

//add identity core
builder.Services.AddIdentityCore<Usuario>(options =>
{
    // Opciones de contraseña básicas
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;

    // Opciones de usuario
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MovieDbContext>()
    .AddSignInManager();

//Manejo de la cookie. Lo ponemos en default, pero hay que ponerlo.
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
})
.AddIdentityCookies();


// Registrar autenticación por cookie para el esquema de Identity
builder.Services.ConfigureApplicationCookie (options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
        options.LoginPath = "/Usuario/Login";
        options.AccessDeniedPath = "/Usuario/AccessDenied";
        // Se pueden ajustar otras opciones de cookie aquí
    });


builder.Services.AddScoped<ImagenStorage>();
builder.Services.Configure<FormOptions>(o => { o.MultipartBoundaryLengthLimit = 2 * 1024 * 1024; });

var app = builder.Build();

//Invocar la ejecucion del DbSeeder con un using scoped
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MovieDbContext>();
        var userManager = services.GetRequiredService<UserManager<Usuario>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DbSeeder.Seed(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Autenticación debe ir antes de autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
