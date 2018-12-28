using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wazi.data.core.drivers;

namespace wazi.data.clients.mongo {
    public class MongoClientSettings : ObjectClientSettings {
        public MongoClientSettings() {
            Servers = new List<MongoServerAddress>();
        }

        public List<MongoServerAddress> Servers { get; set; }

        public override string GetConnectionString() {
            return $"mongodb://{string.Join(",", Servers.Select(s => $"{s.Host}:{s.Port}"))}";
        }
    }
}
