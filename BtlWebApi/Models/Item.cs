using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BtlWebApi.Models
{
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string title { get; set; }
        public int disporder { get; set; }
        public string boardcode { get; set; }
        public string projectcode { get; set; }
        public string prioritycode { get; set; }
        public string sizecode { get; set; }
        public string statuscode { get; set; }
        public string createdbyuser { get; set; }
        public DateTime createdtimestamp { get; set; }
        public string assignedtouser { get; set; }
        public DateTime? assignedtimestamp { get; set; }
        public string closedbyuser { get; set; }
        public DateTime? closedtimestamp { get; set; }
        public string description { get; set; }
        public string comments { get; set; }
        public DateTime? duedate { get; set; }
    }
}
