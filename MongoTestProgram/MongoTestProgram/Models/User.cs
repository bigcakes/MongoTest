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
using MongoTestProgram.Interfaces;

namespace MongoTestProgram.Models
{
    public class User : IBaseRecord
    {
        public ObjectId Id { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime dateAdded { get; set; }
        public DateTime lastUpdated { get; set; }
        public bool? deleted { get; set; }

        public User()
        {
            
        }
    }
}
