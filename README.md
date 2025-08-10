# ECommerceWeb (ASP.NET Core 8) — Login/Signup + Customer Panel

A starter e‑commerce web app built with **.NET 8 / ASP.NET Core MVC + Identity**.  
It ships with email + social login (Google/Facebook), a customer dashboard (profile, addresses, orders, wallet/rewards, favourites, occasions, invoices), and a simple admin area.

> Built to run fast locally with **SQLite**. Swap to SQL Server/Azure SQL by changing one line.

---

## ✨ Features

- **Authentication**
  - Email/password via ASP.NET Core Identity
  - Social login: **Google**, **Facebook**
  - OTP-ready phone field (placeholder hooks in profile)
- **Customer Panel**
  - Profile (editable), contact info
  - **Addresses** with fields for Google Maps/Places autocomplete
  - **Orders** (list, detail, repeat order, leave feedback placeholder)
  - **Wallet/Rewards** (credit/debit/bonus transactions & balance)
  - **Favourites (Wishlist)**
  - **Occasions** (birthday/anniversary calendar)
  - **Invoices** (list & link to PDF placeholder)
- **Admin**
  - Minimal admin dashboard (users & orders counts)
  - Ready for role-based access (`Admin` role)
- **Responsive UI** with Bootstrap (swap to Tailwind if you prefer)

---

## 🧱 Tech stack

- **.NET 8**, ASP.NET Core MVC + **Identity UI**
- **Entity Framework Core 8** (default: **SQLite**; easy switch to SQL Server/Azure SQL)
- Bootstrap 5 for styling
- OAuth 2.0 (Google/Facebook)

---

## 📦 Project structure (high level)

```
ECommerceWeb/
  Controllers/
    AdminController.cs
    FavoritesController.cs
    HomeController.cs
    InvoicesController.cs
    OccasionsController.cs
    OrdersController.cs
    ProfileController.cs
    WalletController.cs
  Data/
    ApplicationDbContext.cs
  Models/
    Address.cs
    ApplicationUser.cs
    FavoriteItem.cs
    Invoice.cs
    Occasion.cs
    Order.cs
    WalletTransaction.cs
  Views/ ... (Razor views, Identity UI via MapRazorPages)
  Program.cs
  appsettings.json
ECommerceWeb.sln
```

---

## 🚀 Getting started

### Prerequisites
- **Visual Studio 2022 (17.8+)** with .NET 8 workload, or
- **.NET 8 SDK** + your editor of choice

### 1) Clone/open
- Extract the solution and open `ECommerceWeb.sln` in Visual Studio  
  or `cd` to the project and run:
  ```bash
  dotnet restore
  ```

### 2) Configure secrets
For local dev, keep secrets out of source using **User Secrets**:

```bash
# from the ECommerceWeb project directory
dotnet user-secrets init

# Google OAuth
dotnet user-secrets set "Authentication:Google:ClientId" "<YOUR_GOOGLE_CLIENT_ID>"
dotnet user-secrets set "Authentication:Google:ClientSecret" "<YOUR_GOOGLE_CLIENT_SECRET>"

# Facebook OAuth
dotnet user-secrets set "Authentication:Facebook:AppId" "<YOUR_FB_APP_ID>"
dotnet user-secrets set "Authentication:Facebook:AppSecret" "<YOUR_FB_APP_SECRET>"
```

> You can also set these in `appsettings.json` (not recommended for real projects).

### 3) Database
Default is **SQLite** using `app.db` file.

Create & apply the initial migration:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> If EF tooling isn’t installed, install it:
> ```bash
> dotnet tool install --global dotnet-ef
> ```

### 4) Run
- **Visual Studio**: `F5` on the HTTPS profile  
- **CLI**: `dotnet run`

Browse the site (VS will open it automatically). Identity UI endpoints are under `/Identity/Account/...` and the main pages are MVC controllers/views.

---

## 🔐 Social login setup (quick)

### Google
1. Go to **Google Cloud Console → Google Auth Platform**
2. Create OAuth client (**Web application**).
3. **Authorized redirect URIs** (must match exactly):
   - `https://localhost:5001/signin-google`  
   - (use your actual VS HTTPS port; add your production URL later e.g. `https://yourdomain.com/signin-google`)
4. Copy **Client ID/Secret** to User Secrets (see above).

### Facebook
1. Go to **Facebook for Developers → My Apps → Create App (Consumer)**.
2. Add **Facebook Login** product → **Web**.
3. **Valid OAuth Redirect URIs**:
   - `https://localhost:5001/signin-facebook`
   - (match your VS port; add prod later e.g. `https://yourdomain.com/signin-facebook`)
4. Copy **App ID/Secret** to User Secrets.  
5. In Dev Mode, add yourself under **App roles → Testers**.

> **Gotcha:** “URL blocked / redirect mismatch” happens when the redirect URI doesn’t match the port/path exactly. Use `/signin-google` and `/signin-facebook` respectively.

---

## ⚙️ Key configuration snippets

### Program.cs (auth, Identity, EF)
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    })
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
        // Optional:
        // options.Scope.Add("email");
        // options.Fields.Add("email");
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
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
app.MapRazorPages(); // Identity UI

app.Run();
```

### Switching to SQL Server/Azure SQL
- Change the connection + provider:
```csharp
// using Microsoft.EntityFrameworkCore;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```
- Update `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=ECommerceWeb;User Id=xxx;Password=yyy;TrustServerCertificate=True;"
}
```
- Run migrations again:
```bash
dotnet ef migrations add InitSqlServer -c ApplicationDbContext
dotnet ef database update -c ApplicationDbContext
```

---

## 👥 Admin role (optional seed)

To protect the Admin area with roles, create the role and assign a user once:

```csharp
// Place after 'var app = builder.Build();' and before 'app.Run();'
using (var scope = app.Services.CreateScope())
{
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if (!await roleMgr.RoleExistsAsync("Admin"))
        await roleMgr.CreateAsync(new IdentityRole("Admin"));

    var admin = await userMgr.FindByEmailAsync("admin@local.test");
    if (admin == null)
    {
        admin = new ApplicationUser { UserName = "admin@local.test", Email = "admin@local.test", EmailConfirmed = true };
        await userMgr.CreateAsync(admin, "Admin#12345");
    }
    if (!await userMgr.IsInRoleAsync(admin, "Admin"))
        await userMgr.AddToRoleAsync(admin, "Admin");
}
```
Then decorate controllers with `[Authorize(Roles = "Admin")]` as needed.

---

## 🧭 Useful routes

- `/` — Home
- `/Profile` — View/update profile
- `/Orders` — My orders
- `/Wallet` — Balance & transactions
- `/Favorites` — Wishlist
- `/Occasions` — Events list/create
- `/Invoices` — Invoices list
- `/Admin/Dashboard` — Admin overview
- Identity UI: `/Identity/Account/Login`, `/Identity/Account/Register`

---

## 🛠️ Troubleshooting

- **CS1061 for AddDefaultIdentity/AddGoogle/UseMigrationsEndPoint**  
  → Ensure these packages exist (v8.*):
  - `Microsoft.AspNetCore.Identity.UI`
  - `Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore`
  - `Microsoft.AspNetCore.Authentication.Google`
  - `Microsoft.AspNetCore.Authentication.Facebook`
  - `Microsoft.EntityFrameworkCore.Sqlite` (or SqlServer)
  → Then **Restore** and rebuild.

- **OAuth redirect mismatch**  
  → Your Google/Facebook redirect must match exactly (scheme + host + port + path).  
  Google: `/signin-google` • Facebook: `/signin-facebook`

- **No email from Facebook**  
  → Add `options.Scope.Add("email"); options.Fields.Add("email");` and test with an account that has a confirmed email.

- **Migrations error**  
  → Ensure EF tools installed: `dotnet tool install --global dotnet-ef`

---

## 📦 Deploying to Azure (quick path)

1. Create **Azure App Service** (Windows or Linux) + **Azure SQL** (or use Azure Database for PostgreSQL if you swap providers).
2. Update your connection string in **Configuration → Application settings** (`DefaultConnection`) and **Authentication** secrets as **App Settings**.
3. Publish from VS (**Right‑click project → Publish → Azure App Service**) or use GitHub Actions/Azure DevOps.

---

## 🗺️ Roadmap / TODO
- Replace Bootstrap with Tailwind + shadcn/ui (optional)
- Add phone OTP verification & email confirmation flows
- Implement address autocomplete with Google Places API
- Generate invoice PDFs
- Full admin CRUD for orders, wallet, occasions, favourites
- Unit/integration tests

---

## License
MIT (or your preferred license).

---

**Questions?** Ping me and I’ll tailor this to your hosting stack (IIS, Azure App Service, Docker) and help you go live fast.
