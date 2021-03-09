using System.Collections.Generic;
using System.Linq;
using System;

using MongoDB.Bson;
using MongoDB.Driver;
using BtlWebApi.Models;

namespace BtlWebApi.Services
{
    public interface ICodeService
    {
        Code GetByTypeAndCodeKey(string codetype, string codekey);
        List<Code> Get();
        List<Code> GetAll();
        Code Get(string id);
        Code GetById(string id);
        Code Create(Code code);
        void Update(string id, Code codeIn);
        void Remove(Code codeIn);
        void Remove(string id);
    }

    public class CodeService : ICodeService
    {

        private readonly IMongoCollection<Code> _codes;

        public CodeService(IBtlDatabaseSettings settings)
        {
            //Console.WriteLine(settings.ConnectionString);
            //Console.WriteLine(settings.DatabaseName);
            //Console.WriteLine(settings.CodesCollectionName);

            var database = DbConnect(settings.ConnectionString, settings.DatabaseName, TimeSpan.FromSeconds(1));

            _codes = database.GetCollection<Code>(settings.CodesCollectionName);

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
        public Code GetByTypeAndCodeKey(string codetype, string codekey)
        {
            return _codes.Find<Code>(code => code.codetype == codetype && code.codekey == codekey).FirstOrDefault();
        }
        public List<Code> Get() =>
            _codes.Find(code => true).ToList();
        public List<Code> GetAll()
        {
            var codes = _codes.Find(_ => true).ToList();
            return codes;
        }
        public Code Get(string id) =>
            _codes.Find<Code>(code => code.id == id).FirstOrDefault();
        public Code GetById(string id) =>
            _codes.Find<Code>(code => code.id == id).FirstOrDefault();

        public Code Create(Code code)
        {
            _codes.InsertOne(code);
            return code;
        }

        public void Update(string id, Code codeIn) =>
            _codes.ReplaceOne(code => code.id == id, codeIn);

        public void Remove(Code codeIn) =>
            _codes.DeleteOne(code => code.id == codeIn.id);

        public void Remove(string id) =>
            _codes.DeleteOne(code => code.id == id);
    }
}
