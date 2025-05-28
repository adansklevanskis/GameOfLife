using FluentValidation;
using GameOfLife.Configuration;
using GameOfLife.Data;
using GameOfLife.Middleware;
using GameOfLife.Models;
using GameOfLife.Notifications;
using GameOfLife.Serialization;
using GameOfLife.Services;
using GameOfLife.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GameSettings>(builder.Configuration.GetSection("GameSettings"));



// Configure EF Core
builder.Services.AddDbContext<GameOfLifeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IGameBoardRepository, GameBoardRepository>();
builder.Services.AddScoped<IGameOfLifeProcessor, GameOfLifeProcessor>();
builder.Services.AddScoped<Notifier>();

builder.Services.AddScoped<IValidator<GameBoard>, GameBoardValidator>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new IntArrayJsonConverter());
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();
