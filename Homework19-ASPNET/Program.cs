using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Homework19_ASPNET.Data;
using Homework19_ASPNET.Models;
using Microsoft.AspNetCore.Identity;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.Extensions.Hosting;
using Homework19_ASPNET.Controllers.Api;
using Homework19_ASPNET.Auth;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.OAuth;
using Homework19_ASPNET.Controllers.Api.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Homework19_ASPNETContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Homework19_ASPNETContext") ?? throw new InvalidOperationException("Connection string 'Homework19_ASPNETContext' not found.")));

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<Homework19_ASPNETContext>()
                .AddDefaultTokenProviders();

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<JwtService>();
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddAuth(builder.Configuration);

var app = builder.Build();

app.Use(async (context, next) =>
{
    if (context.Request.Cookies.TryGetValue("token", out string? token))
        context.Request.Headers.Authorization = $"Bearer {token}";
    await next();
});


//CreateDbIfNotExists(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Project}/{action=Index}/{id?}");

app.Run();


