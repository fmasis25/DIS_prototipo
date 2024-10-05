var builder = WebApplication.CreateBuilder(args);

// Habilitar caché distribuido en memoria para las sesiones
builder.Services.AddDistributedMemoryCache();

// Configurar la duración de la sesión y las cookies de sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duración de la sesión (30 minutos)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Habilitar Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Middleware para redirigir a HTTPS
app.UseHttpsRedirection();

// Middleware para servir archivos estáticos (CSS, JS, imágenes, etc.)
app.UseStaticFiles();

app.UseRouting();

// Middleware para habilitar el uso de sesiones
app.UseSession();

app.UseAuthorization();

// Redireccionar la raíz a la página de inicio de sesión
app.MapGet("/", context =>
{
    context.Response.Redirect("/VieWeb/Login");
    return Task.CompletedTask;
});

// Configurar rutas para Razor Pages
app.MapRazorPages();

// Iniciar la aplicación
app.Run();
