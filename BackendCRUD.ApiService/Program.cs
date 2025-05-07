using Microsoft.EntityFrameworkCore;
using BackendCRUD.ApiService.Data;
using BackendCRUD.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddScoped<EmailService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),sqlOptions => sqlOptions.EnableRetryOnFailure()));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .WithOrigins(
                    "https://front-crud-ince.vercel.app",
                    "http://localhost:3000"
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});


var app = builder.Build();


app.UseExceptionHandler();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();


app.Run();
