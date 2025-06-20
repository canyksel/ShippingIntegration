using Microsoft.EntityFrameworkCore;
using OrderService.Application.Extensions;
using OrderService.Application.Orders.Events;
using OrderService.Infrastructure.Extensions;
using OrderService.Infrastructure.Messaging;
using OrderService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddDbContext<OrderContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrderDb")));

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();


builder.Services.AddOrderServiceMessaging();
builder.Services.AddScoped<IEventPublisher, EventPublisher>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<OrderContext>();
    await SeedData.SeedAsync(context);
}

app.Run();