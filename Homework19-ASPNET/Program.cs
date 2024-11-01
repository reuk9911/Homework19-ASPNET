using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Homework19_ASPNET.Data;
using Homework19_ASPNET.Models;
using Microsoft.AspNetCore.Identity;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.Extensions.Hosting;
using Homework19_ASPNET.Controllers.Api;
using Homework19_ASPNET.Auth;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Homework19_ASPNETContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Homework19_ASPNETContext") ?? throw new InvalidOperationException("Connection string 'Homework19_ASPNETContext' not found.")));

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<Homework19_ASPNETContext>()
                .AddDefaultTokenProviders();
builder.Services.AddHttpClient();

builder.Services.AddAuthorization();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await RoleInitializer.InitializeAsync(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
//CreateDbIfNotExists(app);

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
    pattern: "{controller=Project}/{action=Index}/{id?}");

app.Run();

