using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wazi.data.core.drivers;
using wazi.data.models;
using wazi.data.core.common;

namespace wazi.data.core.master {
    public class MasterRepository : RepositoryBase, IMasterRepository {
        public MasterRepository() : base(null) {

        }

        public MasterRepository(RepositoryConfig repository) :
            base(repository) {

            this.Name = repository.Name;
            this.ID = ID;
            this.Description = "rcosMaster contains all golden copies of products, roles etc. and links to all franchises.";
            this.Setup(repository);
        }

        public override void Setup(RepositoryConfig repository = null) {
            if (repository != null) {
                this.repository = repository;
                this.Name = repository.Name;
                this.ID = ID;
                this.Description = "rcosMaster contains all golden copies of products, roles etc. and links to all franchises.";
            }

            //general repositories
            this.AddObjectRepository<ConfigRepo>(new ConfigRepo(this));
            this.AddObjectRepository<ObjectRepo>(new ObjectRepo(this));
        }
    }
}
