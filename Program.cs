using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShortLinkApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллеры
builder.Services.AddControllers();

// Настройка базы данных SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка Identity для аутентификации и авторизации
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = false;                   
    options.Password.RequireLowercase = false;               
    options.Password.RequireUppercase = false;               
    options.Password.RequireNonAlphanumeric = false;         
    options.Password.RequiredLength = 6;                     
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);  
    options.Lockout.MaxFailedAccessAttempts = 5;             
    options.User.RequireUniqueEmail = true;                  
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Конфигурация cookies для Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";               
    options.LogoutPath = "/logout";             
    options.AccessDeniedPath = "/access-denied"; 
    options.ExpireTimeSpan = TimeSpan.FromDays(7);   
    options.SlidingExpiration = true;           
});

// Добавляем поддержку Swagger для документации API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Включение Swagger в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Используем HTTPS и маршрутизацию
app.UseHttpsRedirection();
app.UseRouting();

// Используем аутентификацию и авторизацию перед маршрутами
app.UseAuthentication();
app.UseAuthorization();

// Обработка статических файлов (например, index.html в wwwroot)
app.UseStaticFiles();

// Определяем маршруты для контроллеров API
app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
