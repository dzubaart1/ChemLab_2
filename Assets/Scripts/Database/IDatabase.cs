using System;
using System.Collections.Generic;

namespace Database
{
    public interface IDatabase<T> where T : ILabSerializable
    {
        public void Add(T item);
        public void RemoveAll();
        public List<T> ReadAll();
        public List<T> ReadWhere(Func<T, bool> filter);
        public string GetFilePath();
    }
}