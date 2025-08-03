using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthFitnessAPI.Context;
using HealthFitnessAPI.ScheduledJobs;
using HealthFitnessAPI.Services;
using HealthFitnessAPI.Services.Init;
using HealthFitnessAPI.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<HealthFitnessDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()).AddFluentValidationAutoValidation();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork<HealthFitnessDbContext>>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<IUserAchievementService, UserAchievementService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserInitService, UserInitService>();

builder.Services.AddQuartz(options =>
{
    var jobKey = new JobKey("DeleteExpiredRefreshTokensJob");
    options.AddJob<DeleteExpiredRefreshTokensJob>(opts => opts
        .WithIdentity(jobKey)
        .WithDescription("Says Hello every 10 seconds"));

    options.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("DeleteExpiredRefreshTokensJob-trigger")
        .WithCronSchedule("0 0 * * * ?")
        .WithDescription("Runs every hour, every day"));
});

builder.Services.AddQuartzHostedService(options =>
    options.WaitForJobsToComplete = true);

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{

    var ctx = scope.ServiceProvider.GetRequiredService<HealthFitnessDbContext>();
    ctx.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature =
            context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

        var error = new
        {
            Message = "An unexpected error occurred.",
            Detail = exceptionHandlerPathFeature?.Error.Message
        };

        await context.Response.WriteAsJsonAsync(error);
    });
});

using (var scope = app.Services.CreateScope())
{
    var userInitService = scope.ServiceProvider.GetRequiredService<IUserInitService>();
    await userInitService.InitUsers();
}

app.MapControllers();

app.Run();
