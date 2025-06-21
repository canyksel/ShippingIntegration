using Microsoft.EntityFrameworkCore;
using ShippingService.Application.Extensions;
using ShippingService.Infrastructure.Extensions;
using ShippingService.Infrastructure.Messaging;
using ShippingService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddDbContext<ShippingContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ShippingDb")));

builder.Services.RegisterRedisService(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddShipmentServiceMessaging();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ShippingContext>();
    context.Database.Migrate();
}
app.UseCors("AllowAll");
app.Run();
