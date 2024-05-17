using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories.IRepositories
{
    public interface IDeckRepository
    {
        List<Deck> GetAll();
        Deck GetById(long id);
        CardDeck AddCardToDeck(int maxAmount, int deckId, int cardId);
        CardDeck RemoveCardFromDeck(int deckId, int cardId);
        void CreateDeck(Deck deck);
        int GetNextAvailableId();
        void ClearDeck(int deckId);
    }
}
