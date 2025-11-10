using Application.Interfaces;
using Application.Mappers;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5079");

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllers();

builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddHttpClient<IBookingApiClient, BookingApiClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5001");
});

builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg => { }, typeof(PaymentProfile));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();