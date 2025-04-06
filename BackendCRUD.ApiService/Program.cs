using Microsoft.EntityFrameworkCore;
using BackendCRUD.ApiService.Data;
using BackendCRUD.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddScoped<EmailService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();


app.UseExceptionHandler();
app.UseCors("AllowAll");

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();