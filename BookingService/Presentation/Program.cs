using System.Text;
using Application.Interfaces;
using Application.Interfaces.Auth;
using Application.Interfaces.Cache;
using Application.Interfaces.UnitOfWork;
using Application.Mappers;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presentation.Middlewares;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5001");

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
var redis = ConnectionMultiplexer.Connect(redisConnectionString);
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

builder.Services.AddSingleton<ICacheService, RedisCacheService>();

builder.Services.AddControllers();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IVenueService, VenueService>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg => { }, typeof(EventProfile), typeof(TicketProfile), typeof(UserProfile), typeof(VenueProfile));
//
// builder.Services.AddHttpClient<IUserApiClientService, UserApiService>(client =>
// {
//     client.BaseAddress = new Uri("http://localhost:5031");
// });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<TicketAvailabilityMiddleware>();

app.MapControllers();

app.Run();