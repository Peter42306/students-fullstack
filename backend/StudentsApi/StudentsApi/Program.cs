using Microsoft.EntityFrameworkCore;
using StudentsApi.Data;
using StudentsApi.Services.Photos;
using StudentsApi.Services.Storage;
using StudentsApi.Services.Students;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.Configure<FileStorageOptions>(
    builder.Configuration.GetSection("FileStorage"));

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentPhotoService, StudentPhotoService>();
builder.Services.AddScoped<IFileStorage, LocalFileStorage>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
