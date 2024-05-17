using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories.IRepositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories.MongoDBRepositories
{
    public class MongoDeckRepository : IDeckRepository
    {
        private readonly IMongoCollection<Deck> _decks;

        public MongoDeckRepository()
        {
            MongoClient client = new MongoClient();
            IMongoDatabase database = client.GetDatabase("mtg_decks");
            _decks = database.GetCollection<Deck>("decks");
        }

        public List<Deck> GetAll()
        {
            return _decks.Find(new BsonDocument()).ToList();
        }

        public Deck GetById(long id)
        {
            return _decks.Find(d => d.Id == id).FirstOrDefault();
        }

        public CardDeck AddCardToDeck(int maxAmount, int deckId, int cardId)
        {
            Deck deck = GetById(deckId);
            List<CardDeck> cardDecks = deck.CardDecks.ToList();

            if (isDeckFull(cardDecks, maxAmount))
            {
                throw new InvalidOperationException("Deck is full");
            }

            CardDeck foundCard = cardDecks.FirstOrDefault(cd => cd.Id == cardId);
            if (foundCard != null)
            {
                // Card already in deck, increase amount
                var filter = Builders<Deck>.Filter.And(
                    Builders<Deck>.Filter.Eq(d => d.Id, deckId),
                    Builders<Deck>.Filter.ElemMatch(d => d.CardDecks, cd => cd.Id == cardId)
                );

                var update = Builders<Deck>.Update.Inc("CardDecks.$.Amount", 1);

                _decks.UpdateOne(filter, update);

                foundCard.Amount++;
            }
            else
            {
                foundCard = new CardDeck { Id = cardId, Amount = 1 };
                // Card not in deck, add card
                var update = Builders<Deck>.Update.Push("CardDecks", foundCard);
                _decks.UpdateOne(d => d.Id == deckId, update);
            }

            return foundCard;
        }

        private bool isDeckFull(List<CardDeck> cardDecks, int maxAmount)
        {
            return cardDecks.Sum(cd => cd.Amount) >= maxAmount;
        }

        public void CreateDeck(Deck deck)
        {
            _decks.InsertOne(deck);
        }

        public CardDeck RemoveCardFromDeck(int deckId, int cardId)
        {
            Deck deck = GetById(deckId);
            CardDeck cardDeck = deck.CardDecks.FirstOrDefault(cd => cd.Id == cardId);

            // Check if card is in deck
            if (cardDeck == null)
            {
                throw new InvalidOperationException("Card not in deck");
            }

            // Check if card amount is greater than 1
            if (cardDeck.Amount > 1)
            {
                // Decrease amount
                var filter = Builders<Deck>.Filter.And(
                                       Builders<Deck>.Filter.Eq(d => d.Id, deckId),
                                                          Builders<Deck>.Filter.ElemMatch(d => d.CardDecks, cd => cd.Id == cardId)
                                                                         );

                var update = Builders<Deck>.Update.Inc("CardDecks.$.Amount", -1);

                _decks.UpdateOne(filter, update);

                cardDeck.Amount--;
            }
            else
            {
                // Remove card
                var update = Builders<Deck>.Update.PullFilter(d => d.CardDecks, cd => cd.Id == cardId);
                _decks.UpdateOne(d => d.Id == deckId, update);
            }

            return cardDeck;
        }

        public int GetNextAvailableId()
        {
            List<Deck> decks = GetAll();
            return decks.Count == 0 ? 1 : decks.Max(d => d.Id) + 1;
        }

        public void ClearDeck(int deckId)
        {
            var update = Builders<Deck>.Update.Set("CardDecks", new List<CardDeck>());
            _decks.UpdateOne(d => d.Id == deckId, update);
        }
    }
}
