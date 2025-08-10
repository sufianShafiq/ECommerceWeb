using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerceWeb.Data;
using ECommerceWeb.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configure database connection. SQLite is used for simplicity and portability.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");// ?? "Data Source=app.db";
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlite(connectionString));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity with default UI. Account confirmation is disabled for ease of testing.
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();

// Add MVC controllers and views.
builder.Services.AddControllersWithViews();

// Register external login providers. Client IDs and secrets are pulled from configuration
// with fallback placeholders so the app still builds if values aren't provided.
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "GOOGLE_CLIENT_ID";
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "GOOGLE_CLIENT_SECRET";
    })
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"] ?? "FACEBOOK_APP_ID";
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"] ?? "FACEBOOK_APP_SECRET";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// Map Razor Pages for Identity UI (login, register, etc.)
app.MapRazorPages();

app.Run();