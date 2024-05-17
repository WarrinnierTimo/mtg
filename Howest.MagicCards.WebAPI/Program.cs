using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Repositories.IRepositories;
using Howest.MagicCards.DAL.Repositories.SqlRepositories;
using Howest.MagicCards.Shared.Mappings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICardRepository, SqlCardRepository>();
builder.Services.AddScoped<IRarityRepository, SqlRarityRepository>();
builder.Services.AddScoped<ISetRepository, SqlSetRepository>();
builder.Services.AddScoped<IArtistRepository, SqlArtistsRepository>();

builder.Services.AddAutoMapper(new Type[] { typeof(CardsProfile), typeof(RaritiesProfile), typeof(ArtistsProfile), typeof(CardsDetailsProfile) });


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
