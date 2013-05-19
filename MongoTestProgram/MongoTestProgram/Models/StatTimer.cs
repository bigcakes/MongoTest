using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using MongoTestProgram.Enums;

namespace MongoTestProgram.Models
{
    public class StatTimer
    {
        public ObjectId Id { get; set; }
        public OperationType operation { get; set; }
        public int recordCount { get; set; }
        public long ticks { get; set; }
        public string modelType { get; set; }
        public int modelPropertyCount { get; set; }
        public string methodName { get; set; }
        private Stopwatch timer { get; set; }

        public StatTimer()
        {
            timer = new Stopwatch();
        }

        public void startTimer()
        {
            timer.Start();
        }

        public void stopTimer()
        {
            timer.Stop();
            ticks = timer.ElapsedTicks;
        }

        public void clearTimer()
        {
            timer = new Stopwatch();
        }

        public override string ToString()
        {
            return string.Format("Operation: {0}  Count: {1}  Type: {2}  Properties: {3}  Seconds: {4}  Method: {5} ", operation.ToString(), recordCount, modelType, modelPropertyCount, TimeSpan.FromTicks(ticks).Seconds, methodName);
        }

    }
}
