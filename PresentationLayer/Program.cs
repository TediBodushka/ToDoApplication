using DataLayer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ============================
// 1) DbContext
// ============================
builder.Services.AddDbContext<ToDoListDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ============================
// 2) Dependency Injection
// ============================
builder.Services.AddScoped<CategoryContext>();
builder.Services.AddScoped<TaskItemContext>();
// builder.Services.AddScoped<UserContext>(); // ❗ Uncomment ONLY if you have UserContext class

// ============================
// 3) MVC
// ============================
builder.Services.AddControllersWithViews();

// ============================
// 4) Swagger (optional)
// ============================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ============================
// 5) CORS (optional for client → API requests)
// ============================
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// ============================
// DEV Swagger
// ============================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ============================
// Middleware Order - VERY IMPORTANT
// ============================
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthorization(); // ← REQUIRED even without auth!

// ============================
// Routing
// ============================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
