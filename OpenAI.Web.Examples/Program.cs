using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Web.Examples.Secure;

var builder = WebApplication.CreateBuilder(args);

// Forzar carga de User Secrets en Development (asegura que los secretos locales se lean antes de registrar servicios)
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>(optional: true);
}

// Add services to the container.
builder.Services.AddControllersWithViews();

// Bind de la sección "OpenAI" y registro en DI
builder.Services.Configure<OpenAIOptions>(builder.Configuration.GetSection("OpenAI"));

// (Opcional) registrar la instancia concreta para inyectarla directamente
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<OpenAIOptions>>().Value);

// Registramos servicios de OpenAI usando los valores enlazados desde IOptions (incluye user secrets)
builder.Services.AddSingleton<OpenAIClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<OpenAIOptions>>().Value;
    return new OpenAIClient(options.ApiKey);
});

builder.Services.AddSingleton<IChatClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<OpenAIOptions>>().Value;
    var clienteOpenAi = new OpenAIClient(options.ApiKey);
    return clienteOpenAi.GetChatClient(options.ChatModel).AsIChatClient();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=OpenAI}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
