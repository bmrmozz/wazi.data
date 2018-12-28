using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using wazi.data.core.drivers;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using wazi.data.core;
using wazi.data.core.common.data;

namespace wazi.data.clients.mongo {
    public class MongoClient : IObjectClient {

        MongoDB.Driver.MongoClient client = null;
        private CancellationToken clienttoken;

        public MongoClient() {
            this.clienttoken.Register(onCancel);
        }

        ~MongoClient() {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
            }
        }

        private void onCancel() {
            //need to implement something here... :--)..
        }

        ClientSettings settings = null;
        public ObjectClientSettings Settings {
            set {
                settings = (ClientSettings)value;
            }
        }

        bool isConnected = false;


        public bool IsConnected {
            get { return this.client != null; }
            set {
                isConnected = value;
            }
        }

        private string defaultInstance = "";

        public string DefaultInstance {
            set {
                this.defaultInstance = value;
            }
            get {
                return this.defaultInstance;
            }
        }

        public void Dispose() {
            Dispose(true);
        }

        public void Connect() {
            if (client == null)
                client = new MongoDB.Driver.MongoClient(this.settings.GetConnectionString());
        }

        public void Disconnect() {
            client = null;
        }

        public void Save<T>(IObjectRepository repo, IEnumerable<T> items, bool autocreate = false) {
            this.Connect();

            if (autocreate && !this.HasCollection(repo.FullName))
                this.CreateCollection(repo.FullName);

            var database = client.GetDatabase(repo.RepositoryName ?? this.defaultInstance);

            var collection = database.GetCollection<T>(repo.FullName);

            //perform the insert of items.
            collection.InsertMany(items);
        }

        public void DeleteOne<T>(IObjectRepository repo, IEnumerable<FilterItem> filters) {
            this.Connect();
            var database = client.GetDatabase(repo.RepositoryName ?? this.defaultInstance);
            var collection = database.GetCollection<T>(repo.FullName);
            var filter = Builders<T>.Filter.And(getfilters<T>(filters));
            collection.DeleteOne(filter);
        }

        public void DeleteMany<T>(IObjectRepository repo, IEnumerable<FilterItem> filters) {
            this.Connect();
            var database = client.GetDatabase(repo.RepositoryName ?? this.defaultInstance);
            var collection = database.GetCollection<T>(repo.FullName);
            var filter = Builders<T>.Filter.And(getfilters<T>(filters));
            collection.DeleteMany(filter);
        }

        public IEnumerable<object> GetAll(IObjectRepository repo) {
            this.Connect();
            var database = client.GetDatabase(repo.RepositoryName ?? this.defaultInstance);
            var collection = database.GetCollection<object>(repo.FullName);
            return collection.FindSync<object>(new BsonDocument()).ToEnumerable();
        }


        public IEnumerable<T> GetAll<T>(IObjectRepository repo) {
            this.Connect();
            var database = client.GetDatabase(repo.RepositoryName ?? this.defaultInstance);
            var collection = database.GetCollection<object>(repo.FullName);
            return collection.FindSync<T>(new BsonDocument()).ToEnumerable();
        }

        public void Update(IObjectRepository repo, object newvalue, IEnumerable<FilterItem> filters) {
            this.Connect();

            var database = client.GetDatabase(repo.RepositoryName ?? this.defaultInstance);
            var collection = database.GetCollection<BsonDocument>(repo.FullName);
            var filter = Builders<BsonDocument>.Filter.And(getfilters(filters));
            var result = collection.ReplaceOne(filter, BsonDocument.Create(newvalue));

            if (result.IsAcknowledged && result.ModifiedCount != 0) {
                //we have a problem here...
            }
        }

        public void Update<T>(IObjectRepository repo, T newvalue, IEnumerable<FilterItem> filters) {
            this.Connect();

            var database = client.GetDatabase(repo.RepositoryName ?? this.defaultInstance);
            var collection = database.GetCollection<BsonDocument>(repo.FullName);
            var filter = Builders<BsonDocument>.Filter.And(getfilters(filters));
            var result = collection.ReplaceOne(filter, newvalue.ToBsonDocument<T>());

            if (result.IsAcknowledged && result.ModifiedCount != 0) {
                //we have a problem here...
            }
        }

        public IEnumerable<T> Get<T>(IObjectRepository repo, IEnumerable<FilterItem> filters) {
            this.Connect();

            var database = client.GetDatabase(repo.RepositoryName ?? this.defaultInstance);
            var collection = database.GetCollection<BsonDocument>(repo.FullName);
            var filter = Builders<BsonDocument>.Filter.And(getfilters(filters));
            return collection.FindSync<T>(filter).ToEnumerable();
        }

        public IEnumerable<object> Get(IObjectRepository repo, IEnumerable<FilterItem> filters) {
            this.Connect();

            var database = client.GetDatabase(repo.RepositoryName ?? this.defaultInstance);
            var collection = database.GetCollection<BsonDocument>(repo.FullName);
            var filter = Builders<BsonDocument>.Filter.And(getfilters(filters));
            return collection.FindSync<object>(filter).ToEnumerable();
        }

        public void StartTransaction(IObjectRepository repo) {

        }

        public void Transact(Action<IObjectClient> action) {
            action(this);
        }

        public void CommitTransaction(IObjectRepository repo) {
        }

        public void RollbackTransaction(IObjectRepository repo) {
        }

        public bool HasDatabase(string name) {
            if (!this.IsConnected)
                this.Connect();

            if (this.IsConnected) {
                var dblist = this.client.ListDatabases(clienttoken);
                while (dblist.MoveNext()) {
                    foreach (BsonDocument database in dblist.Current) {
                        if (database.Contains("name")) {
                            if (database.GetValue("name").AsString == name)
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool HasCollection(string name) {
            this.Connect();

            var database = this.client.GetDatabase(this.defaultInstance);
            var collections = database.ListCollections();

            while (collections.MoveNext()) {
                foreach (BsonDocument collection in collections.Current) {
                    if (collection.Contains("name")) {
                        if (collection.GetValue("name").AsString == name)
                            return true;
                    }
                }
            }

            return false;
        }

        public void CreateCollection(string name) {
            if (this.HasCollection(name))
                return;
            var database = this.client.GetDatabase(this.defaultInstance);
            database.CreateCollection(name);
        }

        public void CreateDatabase(string name) {
            //for mongodb this is not required.
        }

        public void RegisterClassMapping<Y>() {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Y))) {
                BsonClassMap.RegisterClassMap<Y>();
            }
        }

        List<FilterDefinition<BsonDocument>> getfilters(IEnumerable<FilterItem> filters) {
            List<FilterDefinition<BsonDocument>> mongoFilters = new List<FilterDefinition<BsonDocument>>();
            foreach (var thisFilter in filters) {
                switch (thisFilter.Operator) {
                    case FilterOperator.equals:
                        mongoFilters.Add(Builders<BsonDocument>.Filter.Eq(thisFilter.Key, thisFilter.Value));
                        break;
                    case FilterOperator.lessthan:
                        mongoFilters.Add(Builders<BsonDocument>.Filter.Lt(thisFilter.Key, thisFilter.Value));
                        break;
                    case FilterOperator.greatherthan:
                        mongoFilters.Add(Builders<BsonDocument>.Filter.Gt(thisFilter.Key, thisFilter.Value));
                        break;
                    case FilterOperator.contains:
                        mongoFilters.Add(Builders<BsonDocument>.Filter.Text(thisFilter.Key, thisFilter.Value.ToString()));
                        break;
                    case FilterOperator.notequals:
                        break;
                }
            }
            return mongoFilters;
        }

        List<FilterDefinition<T>> getfilters<T>(IEnumerable<FilterItem> filters) {
            List<FilterDefinition<T>> mongoFilters = new List<FilterDefinition<T>>();
            foreach (var thisFilter in filters) {
                switch (thisFilter.Operator) {
                    case FilterOperator.equals:
                        mongoFilters.Add(Builders<T>.Filter.Eq(thisFilter.Key, thisFilter.Value));
                        break;
                    case FilterOperator.lessthan:
                        mongoFilters.Add(Builders<T>.Filter.Lt(thisFilter.Key, thisFilter.Value));
                        break;
                    case FilterOperator.greatherthan:
                        mongoFilters.Add(Builders<T>.Filter.Gt(thisFilter.Key, thisFilter.Value));
                        break;
                    case FilterOperator.contains:
                        mongoFilters.Add(Builders<T>.Filter.Text(thisFilter.Key, thisFilter.Value.ToString()));
                        break;
                    case FilterOperator.notequals:
                        break;
                }
            }
            return mongoFilters;
        }
    }
}
