using EventBus.Messages.Comon;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ordering.API.EventBusConsumer;
using Ordering.API.Mapper;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(OrderingProfile));

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<BasketCheckoutConsumer>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<BasketCheckoutConsumer>();
    cfg.UsingRabbitMq((ctx, rcfg) =>
    {
       // rcfg.Host("amqp://guest:guest@localhost:5672");
        rcfg.Host("amqp://guest:guest@localhost:5672");
        rcfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderContext>();
    db.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();