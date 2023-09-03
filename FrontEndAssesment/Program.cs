using FrontEndAssesment.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .Build();

// Bind the configuration to a strongly typed class
UrlSettings urlSetting = new UrlSettings();
configuration.GetSection("UrlSettings").Bind(urlSetting);

// Register MySettings as a service
builder.Services.AddSingleton(urlSetting);

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.Cookie.HttpOnly = true;
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
//        //options.LoginPath = "/Account/Login";
//        options.AccessDeniedPath = "/Home/UserLogged";
//    });

builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options => {

    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    //options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Home/UserLogged";
});

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

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
