using Microsoft.Extensions.AI;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registramos servicios de OpenAI
builder.Services.AddSingleton<OpenAIClient>(provider =>
    new OpenAIClient(builder.Configuration["OpenAI:ApiKey"]));
builder.Services.AddSingleton<IChatClient>(proveedorServicio =>
{
    var clienteOpenAi = new OpenAIClient(builder.Configuration["OpenAI:ApiKey"]);

    return clienteOpenAi.GetChatClient(builder.Configuration["OpenAI:ChatModel"]).AsIChatClient();
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
