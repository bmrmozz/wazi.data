using wazi.data.core.common.data;
using wazi.data.core.drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wazi.data.core {
    public interface IObjectRepository {
        Guid ID { get; set; }

        Guid RepositoryID { get; set; }

        string RepositoryName { get; set; }

        string Prefix { get; set; }

        string Name { get; set; }

        string FullName { get; }

        string Description { get; set; }

        string DisplayName { get; set; }

        IEnumerable<IContentRepository> ContentRepositories { get; }

        bool IsRepository(Type objectType);

        void Save(IEnumerable<object> items);

        void SaveAsync(IEnumerable<object> items, AsyncCallback callback);

        void Update(IEnumerable<FilterItem> filters, object newvalue);

        void DeleteOne(IEnumerable<FilterItem> filters);

        void DeleteMany(IEnumerable<FilterItem> filters);

        void Remove(IEnumerable<object> items);

        void RemoveItem(object item);

        IEnumerable<object> GetAll();

        IEnumerable<object> Get(IEnumerable<FilterItem> filters);

        void Clear();

        void Create();

        void SetDefaults(IObjectClient client);
    }
}
