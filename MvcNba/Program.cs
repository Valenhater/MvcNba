using Microsoft.EntityFrameworkCore;
using MvcNba.Data;
using MvcNba.Helpers;
using MvcNba.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetConnectionString("SqlNba");

builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<HelperPathProvider>();
builder.Services.AddSingleton<HelperTools>();
builder.Services.AddSingleton<HelperCryptography>();

builder.Services.AddTransient<RepositoryNba>();
builder.Services.AddTransient<RepositoryJugadores>();
builder.Services.AddTransient<RepositoryEntradas>();
builder.Services.AddTransient<RepositoryUsuarios>();
builder.Services.AddDbContext<NbaContext>(options => options.UseSqlServer(connectionString));

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

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Partidos}/{action=PartidosJugados}/{id?}");

app.Run();
