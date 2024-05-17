using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories.IRepositories;
using Howest.MagicCards.DAL.Repositories.MongoDBRepositories;
using Howest.MagicCards.MinimalAPI.Controllers;
using Howest.MagicCards.MinimalAPI.Mappings;
using Howest.MagicCards.Shared.Mappings;
using MongoDB.Driver;

var commonPrefix = "/api";

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddSingleton<DecksService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMongoClient>(new MongoClient(config.GetConnectionString("MongoDB")));
builder.Services.AddAutoMapper(typeof(CardsProfile));
builder.Services.AddScoped<IDeckRepository, MongoDeckRepository>();

var app = builder.Build();
string urlPrefix = config.GetSection("ApiPrefix").Value ?? commonPrefix;
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapDeckEndpoints(urlPrefix, app.Services.GetRequiredService<IMapper>(), config);

app.Run();

