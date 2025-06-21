using Microsoft.EntityFrameworkCore;
using ShippingService.Infrastructure.Extensions;
using ShippingService.Infrastructure.Messaging;
using ShippingService.Infrastructure.Persistence;
using ShippingService.Application.Extensions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddDbContext<ShippingContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ShippingDb")));

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddShipmentServiceMessaging();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
