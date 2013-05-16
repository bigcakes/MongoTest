using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoTestProgram.Extensions
{
    public static class LinqExtensions
    {
        public static object GetProperty<T>(this T obj, string name) where T : class
        {
            Type t = typeof(T);
            return t.GetProperty(name).GetValue(obj, null);
        }
    }
}
