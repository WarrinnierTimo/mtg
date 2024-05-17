using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories.SqlRepositories
{
    public class SqlSetRepository : ISetRepository
    {
        private readonly DBContext.DBContext _db;

        public SqlSetRepository(DBContext.DBContext db)
        {
            _db = db;
        }

        public IQueryable<Set> GetAll()
        {
            // get all sets from the database
            return _db.Sets;
        }
    }
}
