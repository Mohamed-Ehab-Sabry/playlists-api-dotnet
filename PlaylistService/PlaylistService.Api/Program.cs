using Microsoft.EntityFrameworkCore;
using PlaylistService.Application.Interfaces;
using PlaylistService.Infrastructure.Data;
using PlaylistService.Infrastructure.Repositories;
using PlaylistService.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();

// Register the SQLite Datbase Context
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the Repository
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();

// Register the Application Service
builder.Services.AddScoped<IPlaylistAppService, PlaylistAppService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Seeding the database
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await DbSeeder.SeedAsync(context);


}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
