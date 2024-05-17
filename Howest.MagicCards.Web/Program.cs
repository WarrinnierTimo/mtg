using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Repositories.IRepositories;
using Howest.MagicCards.DAL.Repositories.SqlRepositories;
using Howest.MagicCards.Shared.Mappings;
using Howest.MagicCards.Web.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ICardRepository, SqlCardRepository>();
builder.Services.AddScoped<IRarityRepository, SqlRarityRepository>();
builder.Services.AddScoped<ISetRepository, SqlSetRepository>();
builder.Services.AddScoped<IArtistRepository, SqlArtistsRepository>();

builder.Services.AddDbContext<DBContext>
    (options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(new Type[] { typeof(CardsProfile), typeof(RaritiesProfile), typeof(ArtistsProfile), typeof(CardsDetailsProfile) });

builder.Services.AddHttpClient("CardsAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5257/api/");
});

builder.Services.AddHttpClient("DecksAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7079/api/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
