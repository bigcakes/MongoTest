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

namespace MongoTestProgram.Interfaces
{
    public interface IBaseRecord
    {
        ObjectId Id { get; set; }
        DateTime dateAdded { get; set; }
        DateTime lastUpdated { get; set; }
        bool? deleted { get; set; }
    }
}
