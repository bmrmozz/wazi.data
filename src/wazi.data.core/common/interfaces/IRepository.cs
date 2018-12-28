using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using wazi.data.core.drivers;
using wazi.data.models;

namespace wazi.data.core {
    public interface IRepository {
        Guid ID { get; set; }

        string Name { get; set; }

        string DisplayName { get; set; }

        string Description { get; set; }

        bool IsConfigured { get; set; }

        ConcurrentDictionary<Type, object> ObjectRepositories { get; }

        void AddObjectRepository<T>(IObjectRepository repository);

        void CanSaveItem<T>(T itemType);

        void Create();

        void Delete<T>();

        IObjectClient GetClient();

        IObjectRepository GetObjectRepository<T>();

        RepositoryConfig GetConfig();

        void Setup(RepositoryConfig config = null);

        void SetConfig(RepositoryConfig config);
    }
}
