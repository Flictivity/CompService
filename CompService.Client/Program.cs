using Blazored.LocalStorage;
using CompService.Client.Data;
using CompService.Core.Models;
using CompService.Core.Services;
using IAuthorizationService = CompService.Core.Services.IAuthorizationService;
using CompService.Core.Services.Impl;
using MudBlazor.Services;
using CompService.Core.Repositories;
using CompService.Core.Settings;
using CompService.Database.Repositories;
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
builder.Services.AddSingleton<IFacilityRepository, FacilityRepository>();
builder.Services.AddSingleton<ISparePartRepository, SparePartRepository>();
builder.Services.AddSingleton<IReferenceRepository<Source>, SourceRepository>();
builder.Services.AddSingleton<IReferenceRepository<Defect>, DefectRepository>();
builder.Services.AddSingleton<IReferenceRepository<Brand>, BrandRepository>();
builder.Services.AddSingleton<IReferenceRepository<Appearance>, AppearanceRepository>();
builder.Services.AddSingleton<IReferenceRepository<DeviceType>, DeviceTypeRepository>();
builder.Services.AddSingleton<IReferenceRepository<SparePartCategory>, SparePartCategoryRepository>();
builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<IDevicePlaceRepository, DevicePlaceRepository>();

builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IReferenceService<Source>, SourceService>();
builder.Services.AddScoped<IReferenceService<Defect>, DefectService>();
builder.Services.AddScoped<IReferenceService<Brand>, BrandService>();
builder.Services.AddScoped<IReferenceService<Appearance>, AppearanceService>();
builder.Services.AddScoped<IReferenceService<DeviceType>, DeviceTypeService>();
builder.Services.AddScoped<IReferenceService<SparePartCategory>, SparePartCategoryService>();
builder.Services.AddScoped<IFacilityService, FacilityService>();
builder.Services.AddScoped<ISparePartService, SparePartService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IDevicePlaceService, DevicePlaceService>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<AppState>();
builder.Services.AddScoped<UserInfoHolder>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

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
