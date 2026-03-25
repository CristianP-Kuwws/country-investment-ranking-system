using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Calculations;
using Application.Services;
using Application.Services.Calculations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HorizonFutureVestContext>(opt =>
    opt.UseMySql(connectionString,
        ServerVersion.Create(new Version(9, 2, 0), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql))
);

// ===== Repositorios =====
builder.Services.AddScoped<IBaseRepository<Country>, CountryRepository>();
builder.Services.AddScoped<IBaseRepository<MacroIndicator>, MacroIndicatorRepository>();
builder.Services.AddScoped<IBaseRepository<Indicator>, IndicatorRepository>();
builder.Services.AddScoped<IBaseRepository<ReturnRate>, ReturnRateRepository>();

// ===== Servicios =====
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IMacroIndicatorService, MacroIndicatorService>();
builder.Services.AddScoped<IIndicatorService, IndicatorService>();
builder.Services.AddScoped<IReturnRateService, ReturnRateService>();
builder.Services.AddScoped<IRankingCalculationService, RankingCalculationService>();
builder.Services.AddScoped<ISimulationService, SimulationService>();
builder.Services.AddScoped<ISimulationService, SimulationService>();

// ===== Session =====
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//

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

app.UseSession(); //session


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
