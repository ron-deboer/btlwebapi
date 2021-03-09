using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BtlWebApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string boardcode { get; set; }
        public string token { get; set; }
    }
}