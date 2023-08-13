using Casgem.RapidAPI.Hotel.DAL;
using Casgem.RapidAPI.Hotel.DAL.DapperConfiguration.Abstract;
using Casgem.RapidAPI.Hotel.DAL.DapperConfiguration.Concrete;
using Casgem.RapidAPI.Hotel.Services;
using Casgem.RapidAPI.Hotel.Settings;
using Casgem.RapidAPI.Hotel.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<HotelContext>(options => {
    options.UseSqlServer("Server=DESKTOP-13123BI; Initial Catalog=CasgemHotelRapidAPIDB; Integrated Security=true;");
});
builder.Services.AddScoped<HotelDataFetchUtil>();

//Register dapper in scope    
builder.Services.AddScoped<IDapper, DapperImpl>();

//redis
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));
builder.Services.AddSingleton<RedisService>(sp =>
{
    var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
    var redis = new RedisService(redisSettings.Host, redisSettings.Port);
    redis.Connect();
    return redis;
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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
