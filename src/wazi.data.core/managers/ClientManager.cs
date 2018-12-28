using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wazi.data.clients.mongo;
using wazi.data.core.drivers;
using wazi.data.models;

namespace wazi.data.core.managers
{
    public static class ClientManager {
        public static IObjectClient GetClient<T>(RepositoryConfig repository) {
            if (null == repository) {
                throw new InvalidOperationException($"repository provided is null or invalid, repository settings are required.");
            }

            ObjectClientSettings clientSettings = null;
            switch (repository.Connector) {
                case ConnectorType.Mongo:
                    clientSettings = getMongoSettings(repository);
                    break;
                case ConnectorType.Cassandra:
                    break;
                default:
                    break;
            }

            var client = Activator.CreateInstance(typeof(T)) as IObjectClient;
            client.Settings = clientSettings;
            client.DefaultInstance = repository.Name;
            return client;
        }

        private static ObjectClientSettings getMongoSettings(RepositoryConfig repository) {
            var mongoSettings = new MongoClientSettings() {
                Servers = new List<MongoDB.Driver.MongoServerAddress>()
            };

            var primaryserver = repository.Server;
            mongoSettings.Servers.Add(new MongoDB.Driver.MongoServerAddress(primaryserver.ServerName, primaryserver.PortNo));

            return mongoSettings;
        }
    }
}
