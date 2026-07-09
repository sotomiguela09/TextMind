using Microsoft.Extensions.AI;
using TextMind.Configuration;
using TextMind.Techniques;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Configure<OllamaSettings>(builder.Configuration.GetSection("Ollama"));
builder.Services.Configure<AssistantSettings>(builder.Configuration.GetSection("Assistant"));

var ollamaSettings = builder.Configuration.GetSection("Ollama").Get<OllamaSettings>() ?? new OllamaSettings();

builder.Services.AddSingleton<IChatClient>(_ => new OllamaChatClient(
    new Uri(ollamaSettings.BaseUrl),
    ollamaSettings.Model));

builder.Services.AddScoped<IPromptingTechnique, ZeroShot>();
builder.Services.AddScoped<IPromptingTechnique, FewShot>();
builder.Services.AddScoped<IPromptingTechnique, ChainOfThought>();
builder.Services.AddScoped<IPromptingTechnique, RolePrompting>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
