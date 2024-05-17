using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class CardExtensions
    {
        public static IQueryable<Card> ToFilteredList(this IQueryable<Card> cards, string name, string text, string type, string setCode, string rarityCode, int artistId, string sortOrder)
        {
            var filteredCards = cards
                    .Where(c => (string.IsNullOrEmpty(name) || c.Name.Contains(name))
                                   && (string.IsNullOrEmpty(text) || c.Text.Contains(text))
                                   && (string.IsNullOrEmpty(type) || c.Type.Contains(type))
                                   && (setCode == null || c.SetCode == setCode)
                                   && (rarityCode == null || c.RarityCode == rarityCode)
                                   && (artistId == 0 || c.ArtistId == artistId));

            switch (sortOrder?.ToLower())
            {
                case "asc":
                    return filteredCards.OrderBy(c => c.Name);
                case "desc":
                    return filteredCards.OrderByDescending(c => c.Name);
                default:
                    return filteredCards; // If sortOrder is not "asc" or "desc", return the list as is.
            }
        }
    }
}
