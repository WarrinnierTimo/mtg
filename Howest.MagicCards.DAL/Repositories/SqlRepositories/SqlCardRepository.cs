using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories.SqlRepositories
{
    public class SqlCardRepository : ICardRepository
    {
        private readonly DBContext.DBContext _db;

        public SqlCardRepository(DBContext.DBContext db)
        {
            _db = db;
        }

        public IQueryable<Card> GetAll()
        {
            IQueryable<Card> cards = _db.Cards.Select(c => c);
            return cards;
        }

        public Card GetById(int id)
        {
            Card card = _db.Cards.SingleOrDefault(c => c.Id == id);
            return card;
        }
    }
}
