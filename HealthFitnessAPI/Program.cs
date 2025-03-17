using FluentValidation;
using FluentValidation.AspNetCore;
using HealthFitnessAPI.Context;
using HealthFitnessAPI.Services;
using HealthFitnessAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<HealthFitnessDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()).AddFluentValidationAutoValidation();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork<HealthFitnessDbContext>>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<IUserAchievementService, UserAchievementService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
