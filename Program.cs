using eFilingAPI.Repositories;
using eFilingAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// configure DataContext
var connectionString = builder.Configuration.GetConnectionString("DataContext");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

// configure Application Services
builder.Services.AddScoped<PostService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // initialize database
    using var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
    using var db = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.EnsureCreated();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
