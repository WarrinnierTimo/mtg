using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Howest.MagicCards.DAL.Models
{
    public partial class Deck
    {
        [BsonElement("_id")]
        public Int32 Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("cards")]
        public List<CardDeck> CardDecks { get; set; } = new List<CardDeck>();
    }
}
