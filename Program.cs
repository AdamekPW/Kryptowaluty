using ASP_.NET_nauka.Data;
using Microsoft.EntityFrameworkCore;
using ASP_.NET_nauka.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(
	builder.Configuration.GetConnectionString("DefaultConnection")
	));
builder.Services.AddHostedService<DataUpdater>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(option =>
	{
		option.LoginPath = "/Login";
		option.ExpireTimeSpan = TimeSpan.FromDays(2);
	});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapHub<CurrenciesHub>("/CurrenciesHub");
app.Run();
