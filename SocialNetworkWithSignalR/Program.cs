using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetworkWithSignalR.Entities;
using SocialNetworkWithSignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});


builder.Services.AddDbContext<CustomIdentityDbContext>(options =>
{
    options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ChatNewDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
});

builder.Services.AddIdentity<CustomIdentityUser,CustomIdentityRole>()
    .AddEntityFrameworkStores<CustomIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSignalR();

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("Default", "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapHub<ChatHub>("/chathub");
});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
