using System.Collections.Generic;
using System.Linq;
using System;

using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using BtlWebApi.Models;
using BtlWebApi.Helpers;

namespace BtlWebApi.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        List<User> Get();
        List<User> GetAll();
        User Get(string id);
        User GetById(string id);
        User Create(User user);
        void Update(string id, User userIn);
        void Remove(User userIn);
        void Remove(string id);
    }

    public class UserService : IUserService
    {

        private readonly IMongoCollection<User> _users;
        private readonly AppSettings _appSettings;

        public UserService(IBtlDatabaseSettings settings, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            Console.WriteLine(settings.ConnectionString);
            Console.WriteLine(settings.DatabaseName);
            Console.WriteLine(settings.UsersCollectionName);
            
            var database = DbConnect(settings.ConnectionString, settings.DatabaseName, TimeSpan.FromSeconds(1));
            _users = database.GetCollection<User>(settings.UsersCollectionName);
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
        public User Authenticate(string username, string password)
        {
            var query =
                _users.AsQueryable<User>()
                .Where(x => x.username == username && x.password == password);

            var user = query.FirstOrDefault();
            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString()),
                    new Claim(ClaimTypes.Role, user.role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }

        public List<User> Get() =>
            _users.Find(user => true).ToList();
        public List<User> GetAll()
        {
            var users = _users.Find(_ => true).ToList();
            return users;
        }
        public User Get(string id) =>
            _users.Find<User>(user => user.id == id).FirstOrDefault();
        public User GetById(string id) =>
            _users.Find<User>(user => user.id == id).FirstOrDefault();

        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public void Update(string id, User userIn) =>
            _users.ReplaceOne(user => user.id == id, userIn);

        public void Remove(User userIn) =>
            _users.DeleteOne(user => user.id == userIn.id);

        public void Remove(string id) =>
            _users.DeleteOne(user => user.id == id);
    }
}
