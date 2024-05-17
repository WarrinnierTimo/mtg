using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Models
{
    public partial class CardDeck
    {
        [BsonElement("_id")]
        public Int32 Id { get; set; }

        [BsonElement("amount")]
        public Int32 Amount { get; set; }
    }
}
