using DataLayer.Data;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Exceptions;
using Pezeshkafzar_v2.Services;
using static Pezeshkafzar_v2.Areas.Admin.Controllers.ProductsController;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var configuration = builder.Configuration;
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDefaultIdentity<Users>(options =>
//    options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDBContext>();



builder.Services.AddIdentity<Users, IdentityRole<Guid>>(options =>
{
    options.User.RequireUniqueEmail = false;
}).AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/login");

builder.Services.AddRazorPages(options =>
{
    options.Conventions
        .AddPageApplicationModelConvention("/StreamedSingleFileUploadDb",
            model =>
            {
                model.Filters.Add(
                    new GenerateAntiforgeryTokenCookieAttribute());
                model.Filters.Add(
                    new DisableFormValueModelBindingAttribute());
            });
    options.Conventions
        .AddPageApplicationModelConvention("/StreamedSingleFileUploadPhysical",
            model =>
            {
                model.Filters.Add(
                    new GenerateAntiforgeryTokenCookieAttribute());
                model.Filters.Add(
                    new DisableFormValueModelBindingAttribute());
            });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
});

RegisterServices.Handle(builder);

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

app.UseAuthentication();

app.UseAuthorization();

//app.ConfigureCustomExceptionMiddleware();

//app.MapAreaControllerRoute(
//    name: "admin",
//    areaName: "admin",
//    pattern: "admin/{controller=Products}/{action=Index}/{id?}"
//    );

//app.MapControllerRoute(
//    name: "admin",
//    pattern: "admin/{controller=Products}/{action=Index}/{id?}"
//    );

//app.MapControllerRoute(
//    name: "admin",
//    pattern: "{area:exists}/{controller=Products}/{action=Index}/{id?}");
app.MapAreaControllerRoute(
    name: "admin",
    areaName: "admin",
    pattern: "admin/{controller=products}/{action=Index}/{id?}");

app.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");

//app.MapControllerRoute(
//    name: "admin",
//    pattern: "admin/{controller}/{action}"
//);


app.MapRazorPages();

app.UseSession();

app.Run();
