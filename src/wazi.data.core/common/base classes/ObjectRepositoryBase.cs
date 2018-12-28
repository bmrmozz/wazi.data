
using wazi.data.models;
using wazi.data.core.common.data;
using wazi.data.core.drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wazi.data.core {
    public abstract class ObjectRepositoryBase<T> : IObjectRepository {
        public ObjectRepositoryBase(IRepository repository) {
            this.Repository = repository;
        }

        public ObjectRepositoryBase() {

        }

        #region Properties
        IRepository repository = null;

        public IRepository Repository {
            get {
                return this.repository;
            }
            private set {
                this.repository = value;
            }
        }

        public IEnumerable<IContentRepository> ContentRepositories {
            get;
        }

        public string Description {
            get;
            set;
        }

        public string DisplayName {
            get;
            set;
        }

        public Guid ID {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }

        public Guid RepositoryID {
            get;
            set;
        }

        public string RepositoryName {
            get;
            set;
        }

        public string Prefix {
            get; set;
        }

        public string FullName {
            get {
                return string.IsNullOrEmpty(this.Prefix)
                   ? this.Name : $"{this.Prefix}_{this.Name}";
            }
        }
        #endregion

        public void Clear() {

        }

        public void Setup(IRepository repository) {
            this.Repository = repository;
        }

        public virtual void Create() {
            if (this.repository != null) {
                using (var client = this.repository.GetClient()) {
                    if (!client.HasCollection(this.FullName)) {
                        client.CreateCollection(this.FullName);
                        this.SetDefaults(client);
                    }
                }
            }
        }

        public virtual void SetDefaults(IObjectClient client) {
        }

        public virtual bool IsRepository(Type objectType) {
            return objectType == typeof(T);
        }

        public virtual void Remove(IEnumerable<object> items) {
            throw new NotImplementedException();
        }

        public virtual void RemoveItem(object item) {
            throw new NotImplementedException();
        }

        public virtual void Save(IEnumerable<T> items) {
            using (var client = this.repository.GetClient()) {
                client.Save<T>(this, items, true);
            }
        }

        public virtual void SaveItem(T item) {
            if (this.repository == null)
                return;

            using (var client = this.repository.GetClient()) {
                client.Save<T>(this, new List<T> { item }, true);
            }
        }

        public void Save(IEnumerable<object> items) {
            if (this.repository == null)
                throw new ArgumentNullException("MasterRepository", new Exception("Master repository is null or not provided."));

            using (var client = this.repository.GetClient()) {
                client.Save(this, new List<object> { items }, true);
            }
        }

        public virtual void SaveAsync(IEnumerable<object> items, AsyncCallback callback) {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<object> GetAll() {
            if (this.repository == null)
                return null;

            using (var client = this.repository.GetClient()) {
                if (!client.HasCollection(this.FullName))
                    return null;

                //now we need to get all the items using the client.
                return client.GetAll(this);

            }
        }

        public virtual IEnumerable<object> Get(IEnumerable<FilterItem> filters) {
            if (this.repository == null)
                return null;

            using (var client = this.repository.GetClient()) {
                if (!client.HasCollection(this.FullName))
                    return null;

                //now we need to get all the items using the client.
                return client.Get(this, filters);
            }
        }

        public void Update(IEnumerable<FilterItem> filters, object newvalue) {
            using (var client = this.repository.GetClient()) {
                if (client.HasCollection(this.FullName)) {
                    client.Update<T>(this, (T)newvalue, filters);
                }
                else {
                    throw new ArgumentNullException("Collection is invalid or does not exist.");
                }
            }
        }

        public void RegisterClassMapping() {
            using (var client = this.repository.GetClient()) {
                client.RegisterClassMapping<T>();
            }
        }

        public void DeleteOne(IEnumerable<FilterItem> filters) {
            using (var client = this.repository.GetClient()) {
                client.DeleteOne<T>(this, filters);
            }
        }

        public void DeleteMany(IEnumerable<FilterItem> filters) {
            using (var client = this.repository.GetClient()) {
                client.DeleteMany<T>(this, filters);
            }
        }
    }
}
