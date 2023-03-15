using Blazored.LocalStorage;
using CompService.Client.Data;
using CompService.Core.Models;
using CompService.Core.Services;
using IAuthorizationService = CompService.Core.Services.IAuthorizationService;
using CompService.Core.Services.Impl;
using MudBlazor.Services;
using CompService.Core.Repositories;
using CompService.Database.Reposirories;
using CompService.Database.Settings;
using MudBlazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices(options =>
        { 
            options.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
            options.SnackbarConfiguration.ShowTransitionDuration = 100;
        });
builder.Services.Configure<DatabaseConnectionSettings>(
    builder.Configuration.GetSection("MongoConnection"));

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IVerificationRepository, VerificationRepository>();
builder.Services.AddSingleton<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IReferenceRepository<Source>, SourceRepository>();

builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IReferenceService<Source>, SourceService>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<AppState>();
builder.Services.AddSingleton<UserInfoHolder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
