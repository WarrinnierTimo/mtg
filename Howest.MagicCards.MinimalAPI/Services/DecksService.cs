using Howest.MagicCards.DAL.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Howest.MagicCards.MinimalAPI.Controllers
{
    public class DecksService
    {
        private readonly IMongoCollection<Deck> _decks;

        public DecksService(
                IOptions<DecksDatabaseSettings> decksDBSettings)
        {
            var mongoClient = new MongoClient(
                decksDBSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                decksDBSettings.Value.DatabaseName);

            _decks = mongoDatabase.GetCollection<Deck>(
                decksDBSettings.Value.DecksCollectionName);
        }

        public async Task<List<Deck>> GetAsync() =>
        await _decks.Find(_ => true).ToListAsync();

        public async Task<Deck> GetAsync(long id) =>
            await _decks.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Deck newDeck) =>
            await _decks.InsertOneAsync(newDeck);

        public async Task UpdateAsync(long id, Deck updateDeck) =>
            await _decks.ReplaceOneAsync(x => x.Id == id, updateDeck);

        public async Task RemoveAsync(long id) =>
            await _decks.DeleteOneAsync(x => x.Id == id);
    }
}
