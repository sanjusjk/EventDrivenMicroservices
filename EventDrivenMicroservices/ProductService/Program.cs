using MongoDB.Driver;
using ProductService.MongoDb;
using ProductService.RabbitMQ;
using ProductService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDBSettings"));
// Add ProductRepository and other services
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<RabbitMqConnection>();
builder.Services.AddSingleton<ProductPublisher>();

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