using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Filters
{
    public class CardFilter : PaginationFilter
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string SetCode { get; set; }
        public string RarityCode { get; set; }
        public int ArtistId { get; set; }
        public string SortOrder { get; set; } = "asc";
    }
}
