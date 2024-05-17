using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories.SqlRepositories
{
    public class SqlRarityRepository : IRarityRepository
    {
        private readonly DBContext.DBContext _db;

        public SqlRarityRepository(DBContext.DBContext db)
        {
            _db = db;
        }

        public IQueryable<Rarity> GetAll()
        {
            // get all rarities
            return _db.Rarities;
        }
    }
}
