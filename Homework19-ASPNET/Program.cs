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
using Microsoft.Extensions.Primitives;


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

// Middleware to manually extract and validate JWT from Authorization header.
//app.Use(async (context, next) =>
//{
//    // Extract the token from the Authorization header.
//    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
//    if (token != null)
//    {
//        try
//        {
//            // Extract the client ID from the JWT token.
//            var clientId = GetClientIdFromToken(token);

//            // Find client configuration using the extracted client ID.
//            var clientConfig = ClientStore.Clients.FirstOrDefault(c => c.ClientId == clientId);

//            // Set up token validation parameters based on the client configuration.
//            var validationParameters = new TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidateLifetime = true,
//                ValidateIssuerSigningKey = true,
//                ValidIssuer = clientConfig.Issuer,
//                ValidAudience = clientConfig.Audience,
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clientConfig.SecretKey))
//            };

//            // Validate the token and retrieve the principal (user identity).
//            var handler = new JwtSecurityTokenHandler();
//            var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);

//            // Attach the user identity to the current HTTP context if validation is successful.
//            context.User = principal;
//        }
//        catch (SecurityTokenExpiredException)
//        {
//            // Handle expired token by returning a specific unauthorized response.
//            await ReturnUnauthorizedResponse(context, "Token is expired");
//            return;
//        }
//        catch (SecurityTokenValidationException)
//        {
//            // Handle invalid token by returning a specific unauthorized response.
//            await ReturnUnauthorizedResponse(context, "Token is invalid");
//            return;
//        }
//        catch (Exception ex)
//        {
//            // Handle other exceptions that might occur during JWT validation.
//            await ReturnUnauthorizedResponse(context, "Authorization failed: " + ex.Message);
//            return;
//        }
//    }

//    // Proceed to the next middleware in the pipeline.
//    await next();
//});

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


