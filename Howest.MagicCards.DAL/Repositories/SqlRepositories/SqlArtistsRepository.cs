using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories.SqlRepositories
{
    public class SqlArtistsRepository : IArtistRepository
    {
        private readonly DBContext.DBContext _db;

        public SqlArtistsRepository(DBContext.DBContext db)
        {
            _db = db;
        }

        public IQueryable<Artist> GetAll()
        {
            IQueryable<Artist> artists = _db.Artists.Select(a => a);
            return artists;
        }
    }
}
