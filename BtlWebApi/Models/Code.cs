using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BtlWebApi.Models
{
    public class Code
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string codetype { get; set; }
        public string codekey { get; set; }
        public string description { get; set; }
    }
}