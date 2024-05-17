﻿using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories.IRepositories
{
    public interface ICardRepository
    {
        IQueryable<Card> GetAll();
        Card GetById(int id);
    }
}
