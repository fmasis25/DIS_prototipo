var builder = WebApplication.CreateBuilder(args);

// Habilitar cach� distribuido en memoria para las sesiones
builder.Services.AddDistributedMemoryCache();

// Configurar la duraci�n de la sesi�n y las cookies de sesi�n
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duraci�n de la sesi�n (30 minutos)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Habilitar Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Middleware para redirigir a HTTPS
app.UseHttpsRedirection();

// Middleware para servir archivos est�ticos (CSS, JS, im�genes, etc.)
app.UseStaticFiles();

app.UseRouting();

// Middleware para habilitar el uso de sesiones
app.UseSession();

app.UseAuthorization();

// Redireccionar la ra�z a la p�gina de inicio de sesi�n
app.MapGet("/", context =>
{
    context.Response.Redirect("/VieWeb/Login");
    return Task.CompletedTask;
});

// Configurar rutas para Razor Pages
app.MapRazorPages();

// Iniciar la aplicaci�n
app.Run();
