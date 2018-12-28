using wazi.data.core.common.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wazi.data.core.drivers {
    public interface IObjectClient : IDisposable {
        string DefaultInstance { set; get; }

        ObjectClientSettings Settings { set; }

        bool IsConnected { get; set; }

        void Connect();

        void Disconnect();

        bool HasDatabase(string name);

        bool HasCollection<T>(string name);

        void CreateCollection(string name);

        void Save<T>(IObjectRepository repo, IEnumerable<T> items, bool autocreate = false);

        void Update(IObjectRepository repo,object newvalue, IEnumerable<FilterItem> filters);

        void Update<T>(IObjectRepository repo, T newvalue, IEnumerable<FilterItem> filters);

        void DeleteOne<T>(IObjectRepository repo, IEnumerable<FilterItem> filters);

        void DeleteMany<T>(IObjectRepository repo, IEnumerable<FilterItem> filters);

        void Transact(Action<IObjectClient> action);

        void StartTransaction(IObjectRepository repo);

        void CommitTransaction(IObjectRepository repo);

        void RollbackTransaction(IObjectRepository repo);

        void CreateDatabase(string name);

        IEnumerable<object> GetAll(IObjectRepository repo);

        IEnumerable<object> Get(IObjectRepository repo, IEnumerable<FilterItem> filters);

        void RegisterClassMapping<Y>();
    }

   
}
