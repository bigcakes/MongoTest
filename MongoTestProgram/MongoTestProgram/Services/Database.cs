using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Reflection;
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


        //**************** TODO:Convert all methods to be generic methods
        //**************** TODO:Thread the database calls

        public static List<User> GetUsers(List<ObjectId> ids)
        {
            var foundList = new List<User>();

            // "entities" is the name of the collection
            var collection = database.GetCollection<User>("users");

            var timer = new StatTimer();

            timer.startTimer();

            //var query = Query<User>.EQ(e => e.Id, id);
            foreach (var id in ids)
            {
                var query = Query<User>.EQ(e => e.Id, id);
                var entity = collection.FindOne(query);

                foundList.Add(entity);
            }

            timer.stopTimer();
            timer.recordCount = foundList.Count();
            timer.operation = Enums.OperationType.Select;

            RecordTimer(timer, typeof(User));

            return foundList;
        }

        public static List<User> GetAllUsers()
        {
            var foundList = new List<User>();

            // "entities" is the name of the collection
            var collection = database.GetCollection<User>("users");

            var timer = new StatTimer();

            timer.startTimer();

            foundList.AddRange(collection.FindAll());

            timer.stopTimer();
            timer.recordCount = foundList.Count();
            timer.operation = Enums.OperationType.Select;

            RecordTimer(timer, typeof(User));

            return foundList;
        }

        public static List<ObjectId> InsertUsers(List<User> users)
        {
            var returnedList = new List<ObjectId>();

            // "entities" is the name of the collection
            var collection = database.GetCollection<User>("users");

            var timer = new StatTimer();

            timer.startTimer();

            foreach (var user in users)
            {
                collection.Insert(user);
                returnedList.Add(user.Id); // Insert will set the Id if necessary (as it was in this example)
            }

            timer.stopTimer();
            timer.recordCount = users.Count();
            timer.operation = Enums.OperationType.Insert;

            var recorded = RecordTimer(timer, typeof(User));

            return returnedList;
        }

        public static List<User> UpdateUsers(List<User> users)
        {
            var returnedList = new List<User>();

            var collection = database.GetCollection<User>("users");

            var timer = new StatTimer();

            timer.startTimer();

            foreach(var user in users)
            {
                var currentUser = collection.AsQueryable().Single(u => u.Id == user.Id);

                currentUser.username = user.username;
                currentUser.firstName = user.firstName;
                currentUser.lastName = user.lastName;

                collection.Save(currentUser);
            }

            timer.stopTimer();
            timer.recordCount = users.Count();
            timer.operation = Enums.OperationType.Update;

            RecordTimer(timer, typeof(User));


            return returnedList;
        }

        public static bool RemoveUsers(List<ObjectId> ids)
        {
            var success = true;

            var collection = database.GetCollection<User>("users");

            var timer = new StatTimer();

            timer.startTimer();

            foreach (var id in ids)
            {
                var query = Query<User>.EQ(e => e.Id, id);
                collection.Remove(query);
            }

            timer.stopTimer();
            timer.recordCount = ids.Count();
            timer.operation = Enums.OperationType.Delete;

            RecordTimer(timer, typeof(User));

            return success;
        }

        public static List<User> SearchUsers(string username = "", string firstName = "", string lastName = "")
        {
            var returnedList = new List<User>();

            var collection = database.GetCollection<User>("users");

            //var stuff = collection.AsQueryable().Where(e => e.GetType().GetProperty(searchBy).GetValue(e) == searchUsing);
            //var stuff = collection.AsQueryable().Where(e => e.GetProperty(searchBy) == searchUsing);
            //var query = Query<User>.EQ(e => e.GetType().GetProperty(searchBy).GetValue(e), searchUsing);
            //var entities = collection.Find(query);

            //returnedList.AddRange(entities);
            var timer = new StatTimer();

            timer.startTimer();

            var stuff = collection.AsQueryable().Where(e => (e.username.Contains(username))).Where(e => (e.firstName.Contains(firstName))).Where(e => (e.lastName.Contains(lastName)));

            returnedList.AddRange(stuff);

            timer.stopTimer();
            timer.recordCount = stuff.Count();
            timer.operation = Enums.OperationType.Select;

            RecordTimer(timer, typeof(User));

            return returnedList;
        }

        /// <summary>
        /// Need to start and stop timer before this method and add the record count to the passed model.
        /// </summary>
        /// <param name="timer">The timer object that has the required information</param>
        /// <param name="model">The model that is being used</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool RecordTimer(StatTimer timer, Type type)
        {
            var success = true;
            var props = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            timer.modelPropertyCount = props.Count();
            timer.modelType = type.Name; //.GetType().ToString();
            timer.methodName = new StackFrame(1, true).GetMethod().Name;

            InsertTimer(timer);

            return success;
        }

        public static List<ObjectId> InsertTimer(StatTimer timer)
        {
            var returnedList = new List<ObjectId>();

            // "entities" is the name of the collection
            var collection = database.GetCollection<StatTimer>("timers");

            collection.Insert(timer);
            returnedList.Add(timer.Id);

            return returnedList;
        }

        public static List<StatTimer> GetAllTimers()
        {
            var foundList = new List<StatTimer>();

            // "entities" is the name of the collection
            var collection = database.GetCollection<StatTimer>("timers");

            foundList.AddRange(collection.FindAll());

            return foundList;
        }

    }
}
