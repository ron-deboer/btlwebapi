using System.Collections.Generic;
using System.Linq;
using System;

using MongoDB.Bson;
using MongoDB.Driver;
using BtlWebApi.Models;

namespace BtlWebApi.Services
{
    public interface IItemService
    {
        List<Item> Get();
        List<Item> GetAll();
        Item Get(string id);
        Item GetById(string id);
        Item Create(Item item);
        void Update(string id, Item itemIn);
        void Remove(Item itemIn);
        void Remove(string id);
    }

    public class ItemService : IItemService
    {

        private readonly IMongoCollection<Item> _items;

        public ItemService(IBtlDatabaseSettings settings)
        {
            //Console.WriteLine(settings.ConnectionString);
            //Console.WriteLine(settings.DatabaseName);
            //Console.WriteLine(settings.ItemsCollectionName);

            var database = DbConnect(settings.ConnectionString, settings.DatabaseName, TimeSpan.FromSeconds(1)); 

            _items = database.GetCollection<Item>(settings.ItemsCollectionName);

        }

        private IMongoDatabase DbConnect(string connectionString, string dbName, TimeSpan timeout)
        {
            var url = MongoUrl.Create(connectionString);
            var client = new MongoClient(url);
            var db = client.GetDatabase(dbName);
            var pingTask = db.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));
            pingTask.Wait(timeout);
            if (pingTask.IsCompleted)
                Console.WriteLine("Connected to: {0}.", connectionString);
            else
                throw new TimeoutException(string.Format("Failed to connect to: {0}.", connectionString));

            return db;
        }

        public List<Item> Get() =>
            _items.Find(item => true).ToList();
        public List<Item> GetAll()
        {
            var items = _items.Find(_ => true).ToList();
            return items;
        }
        public Item Get(string id) =>
            _items.Find<Item>(item => item.id == id).FirstOrDefault();
        public Item GetById(string id) =>
            _items.Find<Item>(item => item.id == id).FirstOrDefault();

        public Item Create(Item item)
        {
            _items.InsertOne(item);
            return item;
        }

        public void Update(string id, Item itemIn) =>
            _items.ReplaceOne(item => item.id == id, itemIn);

        public void Remove(Item itemIn) =>
            _items.DeleteOne(item => item.id == itemIn.id);

        public void Remove(string id) =>
            _items.DeleteOne(item => item.id == id);
    }
}
