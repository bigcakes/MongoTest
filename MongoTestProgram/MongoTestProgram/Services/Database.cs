using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using MongoTestProgram.Models;
using MongoTestProgram.Extensions;

namespace MongoTestProgram.Services
{
    public static class Database
    {
        public static string connectionString = "mongodb://localhost";
        public static MongoClient client = new MongoClient(connectionString);
        public static MongoServer server = client.GetServer();
        public static MongoDatabase database = server.GetDatabase("test"); // "test" is the name of the database

        public static List<User> GetUsers(List<ObjectId> ids)
        {
            var foundList = new List<User>();

            // "entities" is the name of the collection
            var collection = database.GetCollection<User>("users");

            //var query = Query<User>.EQ(e => e.Id, id);
            foreach (var id in ids)
            {
                var query = Query<User>.EQ(e => e.Id, id);
                var entity = collection.FindOne(query);

                foundList.Add(entity);
            }

            return foundList;
        }

        public static List<User> GetAllUsers()
        {
            var foundList = new List<User>();

            // "entities" is the name of the collection
            var collection = database.GetCollection<User>("users");

            foundList.AddRange(collection.FindAll());

            return foundList;
        }

        public static List<ObjectId> InsertUsers(List<User> users)
        {
            var returnedList = new List<ObjectId>();

            // "entities" is the name of the collection
            var collection = database.GetCollection<User>("users");

            foreach (var user in users)
            {
                collection.Insert(user);
                returnedList.Add(user.Id); // Insert will set the Id if necessary (as it was in this example)
            }

            return returnedList;
        }

        public static List<User> UpdateUsers(List<User> users)
        {
            throw new NotImplementedException();
            var returnedList = new List<User>();

            var collection = database.GetCollection<User>("users");



            return returnedList;
        }

        public static bool RemoveUsers(List<ObjectId> ids)
        {
            var success = true;

            var collection = database.GetCollection<User>("users");

            foreach (var id in ids)
            {
                var query = Query<User>.EQ(e => e.Id, id);
                collection.Remove(query);
            }

            return success;
        }

        public static List<User> SearchUsers(string searchBy, string searchUsing)
        {
            var returnedList = new List<User>();

            var collection = database.GetCollection<User>("users");

            //var stuff = collection.AsQueryable().Where(e => e.GetType().GetProperty(searchBy).GetValue(e) == searchUsing);
            //var stuff = collection.AsQueryable().Where(e => e.GetProperty(searchBy) == searchUsing);
            //var query = Query<User>.EQ(e => e.GetType().GetProperty(searchBy).GetValue(e), searchUsing);
            //var entities = collection.Find(query);

            //returnedList.AddRange(entities);

            var stuff = collection.AsQueryable().Where(e => searchBy != "username" || (e.username == searchUsing)).Where(e => searchBy != "firstName" || (e.firstName == searchUsing)).Where(e => searchBy != "lastName" || (e.lastName == searchUsing));

            returnedList.AddRange(stuff);

            return returnedList;
        }

    }
}
