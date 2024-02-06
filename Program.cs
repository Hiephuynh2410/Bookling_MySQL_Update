using System.Reflection;
using System.Text;
using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Scrutor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages()
            .AddRazorRuntimeCompilation();

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("BookingCatToc");

builder.Services.AddDbContext<DlctContext>(options => {
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


//register Services
builder.Services.AddServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "adminArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapGet("/", context =>
{
    context.Response.Redirect("/Admin/Home/Index");
    return Task.CompletedTask;
});
app.Run();
