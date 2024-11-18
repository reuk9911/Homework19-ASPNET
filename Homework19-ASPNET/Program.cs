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
//builder.Services.AddHttpClient();

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<JwtService>();
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddAuth(builder.Configuration);




var app = builder.Build();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Project}/{action=Index}/{id?}");

app.Run();

////*******////

//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<Homework19_ASPNETContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("Homework19_ASPNETContext") ?? throw new InvalidOperationException("Connection string 'Homework19_ASPNETContext' not found.")));

//// Add services to the container.

//builder.Services.AddControllersWithViews();
//builder.Services.AddIdentity<User, IdentityRole>()
//                .AddEntityFrameworkStores<Homework19_ASPNETContext>()
//                .AddDefaultTokenProviders();
//builder.Services.AddHttpClient();

//builder.Services.AddAuthorization();

///*//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
////    .AddCookie(options =>
////    {
////        options.LoginPath = "/Login";
////        options.AccessDeniedPath = "/accessdenied"; // ДОБАВИТЬ VIEW!!!!!!!!!!!!!!!!
////    });*/

//builder.Services.AddAuthorization();
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            // указывает, будет ли валидироваться издатель при валидации токена
//            ValidateIssuer = true,
//            // строка, представляющая издателя
//            ValidIssuer = AuthOptions.ISSUER,
//            // будет ли валидироваться потребитель токена
//            ValidateAudience = true,
//            // установка потребителя токена
//            ValidAudience = AuthOptions.AUDIENCE,
//            // будет ли валидироваться время существования
//            ValidateLifetime = true,
//            // установка ключа безопасности
//            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
//            // валидация ключа безопасности
//            ValidateIssuerSigningKey = true,
//        };
//    });


//var app = builder.Build();

////using (var scope = app.Services.CreateScope())
////{ // добавляем первого админа в БД
////    var services = scope.ServiceProvider;
////    try
////    {
////        var userManager = services.GetRequiredService<UserManager<User>>();
////        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
////        await RoleInitializer.InitializeAsync(userManager, rolesManager);
////    }
////    catch (Exception ex)
////    {
////        var logger = services.GetRequiredService<ILogger<Program>>();
////        logger.LogError(ex, "An error occurred while seeding the database.");
////    }
////}
////CreateDbIfNotExists(app);

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Project}/{action=Index}/{id?}");

//app.Run();

