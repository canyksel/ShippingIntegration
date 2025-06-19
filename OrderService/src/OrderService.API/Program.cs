using OrderService.Domain.Common.Interfaces;
using OrderService.Infrastructure.Eventing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// MediatR service registration.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IDomainEvent>());
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
