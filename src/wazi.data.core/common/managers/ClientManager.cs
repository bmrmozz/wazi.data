using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wazi.data.core.common.clients.mongo;
using wazi.data.core.drivers;
using wazi.data.models;

namespace wazi.data.core.common.managers {
    public static class ClientManager {
        public static IObjectClient GetClient<T>(RepositoryConfig repository) {
            if (null == repository) {
                throw new InvalidOperationException($"repository provided is null or invalid, repository settings are required.");
            }

            IObjectClient client = null;
            ObjectClientSettings clientSettings = null;
            switch (repository.Connector) {
                case ConnectorType.Mongo:
                    clientSettings = getMongoSettings(repository);
                    client = Activator.CreateInstance(typeof(MongoClient)) as IObjectClient;
                    break;
                case ConnectorType.Cassandra:
                    throw new NotSupportedException("Cassandra client connector not yet supported, please use MongoDB");
                default:
                    break;
            }

            client.Settings = clientSettings;
            client.DefaultInstance = repository.Name;
            return client;
        }

        private static ObjectClientSettings getMongoSettings(RepositoryConfig repository) {
            var mongoSettings = new MongoClientSettings() {
                Servers = new List<MongoDB.Driver.MongoServerAddress>()
            };

            mongoSettings.Servers.Add(new MongoDB.Driver.MongoServerAddress(repository.Server.ServerName, repository.Server.PortNo));
            return mongoSettings;
        }
    }
}