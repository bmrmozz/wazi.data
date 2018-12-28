using wazi.data.core.drivers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using wazi.data.models;

namespace wazi.data.core {
    public class RepositoryBase : IRepository {
        protected RepositoryConfig repository = null;

        public RepositoryBase(RepositoryConfig repository) {
            this.repository = repository;
        }

        public string Description {
            get; set;
        }

        public string DisplayName {
            get; set;
        }

        public Guid ID {
            get; set;
        }

        public string Name {
            get; set;
        }

        ConcurrentDictionary<Type, object> objectRepositories = new ConcurrentDictionary<Type, object>();
        [JsonIgnore()]
        public ConcurrentDictionary<Type, object> ObjectRepositories {
            get {
                return objectRepositories;
            }
        }

        ConcurrentBag<IRepository> subrepositories = new ConcurrentBag<IRepository>();
        [JsonIgnore()]
        public ConcurrentBag<IRepository> SubRepositories {
            get {
                return subrepositories;
            }
        }

        public bool IsConfigured {
            get; set;
        }

        public virtual IObjectClient GetClient() {
            // return ClientManager.GetClient<mongo.client.MongoClient>(this.repository);
            return null;
        }

        public virtual IObjectRepository GetObjectRepository<T>() {
            return this.objectRepositories.FirstOrDefault(repo => (repo.Value as IObjectRepository).IsRepository(typeof(T))).Value as IObjectRepository;
        }

        public virtual void AddObjectRepository<T>(IObjectRepository objectrepo) {
            objectrepo.RepositoryID = this.ID;
            objectrepo.RepositoryName = this.Name;
            objectRepositories.AddOrUpdate(typeof(T), objectrepo, addrepo);
        }

        public virtual void AddRepository(IRepository repository, string name) {
            var newconfig = new RepositoryConfig {
                Connector = this.repository.Connector,
                Name = name,
                Server = this.repository.Server,
                Servers = this.repository.Servers,
                Type = RepositoryType.data,
                DisplayName = Name,
                State = ConfigState.active
            };

            repository.SetConfig(newconfig);
            repository.Name = newconfig.Name;
            repository.DisplayName = newconfig.DisplayName;
            repository.ID = Guid.NewGuid();
            repository.Setup(newconfig);
            this.subrepositories.Add(repository);
        }

        public virtual void Setup(RepositoryConfig config = null) {
            //do nothing..
        }

        private object addrepo(Type type, object repository) {
            return repository;
        }

        public virtual void CanSaveItem<T>(T itemType) {
            throw new NotImplementedException();
        }

        public virtual void Create() {
            using (var client = this.GetClient()) {
                //lets first see if database exists...
                var createDatabase = !client.HasDatabase(this.Name);

                if (createDatabase)
                    client.CreateDatabase(this.Name);

                foreach (KeyValuePair<Type, object> item in this.objectRepositories) {
                    var thisType = item.Key.GetType();
                    var repo = (IObjectRepository)item.Value;
                    repo.Create();
                }
            }


            foreach (IRepository subrepository in this.SubRepositories) {
                //force all sub repositories to also create themselves...
                subrepository.Create();
            }
        }

        public virtual RepositoryConfig GetConfig() {
            return this.repository;
        }

        public virtual void SetConfig(RepositoryConfig config) {
            this.repository = config;
        }

        public virtual void Delete<T>() {
            object objectRepo = null;
            this.ObjectRepositories.TryRemove(typeof(T), out objectRepo);
        }
    }
}
