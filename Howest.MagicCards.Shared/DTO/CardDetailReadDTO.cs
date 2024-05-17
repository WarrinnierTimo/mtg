using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO
{
    public record CardDetailReadDTO
    {
        public long Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string ImageUrl { get; init; }
        public string Type { get; init; }
        public string Toughness { get; init; }
        public string Power { get; init; }
    }
}
