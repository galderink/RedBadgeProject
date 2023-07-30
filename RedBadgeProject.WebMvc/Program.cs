using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedBadgeProject.Data;
using RedBadgeProject.Data.Entities;
using RedBadgeProject.Services;
using RedBadgeProject.Services.Class;
using RedBadgeProject.Services.Messages;
using RedBadgeProject.Services.User;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddDefaultIdentity<UserEntity>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.ConfigureApplicationCookie(options => 
{
    options.LoginPath = "/Account/Login";
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessagesService, MessagesService>();
builder.Services.AddScoped<IUserClassService, UserClassService>();
builder.Services.AddScoped<IAvailableClassService, AvailableClassService>();
builder.Services.AddScoped<IAccountStatusService, AccountStatusService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error occurred while applying migrations: {ex.Message}");
    }
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "accountstatus",
        pattern: "AccountStatus/AccountStatusDetail",
        defaults: new { controller = "AccountStatus", action = "AccountStatusDetail" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}");
});

app.Run();
