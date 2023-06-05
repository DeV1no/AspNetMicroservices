
using Basket.Api.GrpcServices;
using Basket.Api.Mapper;
using Basket.Api.Repositories;
using Discount.Grpc.Protos;
using Grpc.Net.Client.Configuration;
using MassTransit;
using static Discount.Grpc.Protos.DiscountProtoService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
    (o=>o.Address=new Uri("http://localhost:5003"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(BasketProfile));

// builder.Services.AddGrpcClient<DiscountProtoServiceClient>(o => { 
//     o.Address = new Uri("https://localhost:5003");
//     // o.ChannelOptionsActions.Add(options =>
//     // {
//     //     options.ServiceConfig = new ServiceConfig {MethodConfigs = {defaultMethodConfig}};
//     // });
// });


builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((ctx, rcfg) =>
    {
        rcfg.Host("amqp://guest:guest@localhost:5672");
    });
});
//builder.Services.AddMassTransitHostedService();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(opt =>
    opt.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString")
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();