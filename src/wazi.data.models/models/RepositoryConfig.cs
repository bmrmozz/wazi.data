using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wazi.data.models {
    public class RepositoryConfig : ConfigurationItem {

        public RepositoryConfig() : base(false) {
        }

        public IEnumerable<RepositoryConfig> Children {
            get;
            set;
        }

        public ConnectorType Connector {
            get;
            set;
        }

        public IEnumerable<RepositoryDataObject> DataObjects {
            get;
            set;
        }

        public string Password {
            get;
            set;
        }

        public RepositoryAddress Server {
            get;
            set;
        }

        public IEnumerable<RepositoryAddress> Servers {
            get;
            set;
        }

        public RepositoryType Type {
            get;
            set;
        }

        public string Username {
            get;
            set;
        }


    }

    public class RepositoryAddress {

        public RepositoryAddress() {
        }

        public int PortNo {
            get;
            set;
        }

        public string ServerName {
            get;
            set;
        }
    }

    public enum RepositoryType {
        master,
        franchise,
        outlet,
        data
    }

    public enum ConnectorType {
        Mongo,
        Cassandra
    }
}