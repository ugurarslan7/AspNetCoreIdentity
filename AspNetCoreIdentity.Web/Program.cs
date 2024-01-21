using AspNetCoreIdentity.Web.ClaimsProvider;
using AspNetCoreIdentity.Web.Extensions;
using AspNetCoreIdentity.Web.Models;
using AspNetCoreIdentity.Web.OptionsModels;
using AspNetCoreIdentity.Web.PermissionsRoot;
using AspNetCoreIdentity.Web.Requirements;
using AspNetCoreIdentity.Web.Seeds;
using AspNetCoreIdentity.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using static AspNetCoreIdentity.Web.Requirements.ViolenceRequirement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
   {
       options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConectionString"));
   });

//User Picture eriþmek istiyorum.Herhangi bir controllerda veya classta wwwroot ta bulunan klasore IFileProvider ile eriþmek için.
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EMailSettings"));

builder.Services.AddIdentityWithExtension();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();
builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BursaPolicy", policy =>
    {
        policy.RequireClaim("city", "Bursa");
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new ExchangeExpireRequirement());
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ViolencePolicy", policy =>
    {
        policy.AddRequirements(new ViolenceRequirement() { ThresholdAge = 18 });
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OrderPermissionRead", policy =>
    {
        policy.RequireClaim("permission", Permission.Order.Read);
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OrderPermissionDelete", policy =>
    {
        policy.RequireClaim("permission", Permission.Order.Delete);
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StockPermissionDelete", policy =>
    {
        policy.RequireClaim("permission", Permission.Order.Delete);
    });
});

builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookieBuilder = new CookieBuilder();
    cookieBuilder.Name = "UserAppCookie";
    opt.LoginPath = new PathString("/Home/SignIn");
    opt.LogoutPath = new PathString("/Member/Logout");
    opt.AccessDeniedPath = new PathString("/Member/AccessDenied");
    opt.Cookie = cookieBuilder;
    opt.ExpireTimeSpan = TimeSpan.FromDays(10); // Cookie nin browserda tutulacagý gün
    opt.SlidingExpiration = true;  //Kullanýcý 10 gün içinde herhangi bir zamanda sisteme girdiðinde cookienin devam etmesi

});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

    await PermissionSeed.Seed(roleManager);
}

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
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
