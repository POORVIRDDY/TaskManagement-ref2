using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskManagement.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TaskManagementContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("dbconn")));
builder.Services.AddScoped<NotificationService>();
//builder.Services.AddScoped<User>();
//builder.Services.AddScoped<MasterUserStory>();
builder.Services.AddScoped<TaskManagementContext>();
// Add services to the container
builder.Services.AddRazorPages();

// Enable session with a 30-minute timeout
builder.Services.AddSession(options =>
{

    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shared/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable session middleware
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();