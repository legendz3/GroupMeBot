using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace KeBot.Service.Bots
{
    public class BotsRepository<T> : IRepository<T> where T : class
    {
        public BotsRepository()
        {
            var connectionstring = ConfigurationManager.AppSettings.Get("db_url");
            var mongoUrl = new MongoUrl(connectionstring);
            Db = MongoDatabase.Create(mongoUrl);
        }

        protected MongoDatabase Db { get; private set; }
        protected MongoCollection<T> Collection
        {
            get { return Db.GetCollection<T>(typeof(T).Name); }
        }


        public bool Insert(T item)
        {
            var result = Collection.Insert(item, SafeMode.True);
            return result.Ok;
        }

        public bool Update(T item)
        {
            var result = Collection.Save(item, SafeMode.True);
            return result.Ok;
        }

        public bool Update(string id, IDictionary<string, object> updateVal)
        {
            var query = NewIdQueryDoc(id);
            return Update(query, updateVal);
        }

        public bool Update(QueryDocument query, IDictionary<string, object> keyValsToUpdate)
        {
            var update = new UpdateBuilder();
            foreach (var vp in keyValsToUpdate)
                update.Set(vp.Key, BsonValue.Create(vp.Value));

            var result = Collection.Update(query, update, SafeMode.True);

            return result.Ok;
        }

        public bool Delete(string id)
        {
            var query = NewIdQueryDoc(id);
            var results = Collection.Remove(query, SafeMode.True);
            return results.Ok;
        }

        public T FindOne(string id)
        {
            var query = NewIdQueryDoc(id);
            return Collection.FindOne(query);
        }

        public T FindOne(Func<T, bool> criteria)
        {
            return Collection.AsQueryable().FirstOrDefault(criteria);
        }

        public IEnumerable<T> FindAll()
        {
            return Collection.FindAll();
        }

        public IEnumerable<T> FindAll(Func<T, bool> criteria)
        {
            return Collection.AsQueryable().Where(criteria);
        }

        private static QueryDocument NewIdQueryDoc(string id)
        {
            return new QueryDocument("_id", id);
        }
    }

    public interface IRepository<T>
    {
        bool Insert(T item);
        bool Update(T item);
        bool Update(string id, IDictionary<string, object> updateVal);
        bool Delete(string id);
        T FindOne(string id);
        T FindOne(Func<T, bool> criteria);
        IEnumerable<T> FindAll();
        IEnumerable<T> FindAll(Func<T, bool> criteria);
    }
}
