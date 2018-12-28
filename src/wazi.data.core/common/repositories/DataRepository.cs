using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wazi.data.models;

namespace wazi.data.core.common {
    public class DataRepository : RepositoryBase {
        public DataRepository() : base(null) {
        }

        public DataRepository(RepositoryConfig repository) :
            base(repository) {

            this.Name = repository.Name;
            this.ID = ID;
            this.Description = "Repository responsible for all adhoc data objects";
            this.Setup(repository);
        }

        public override void Setup(RepositoryConfig repository = null) {
            if (this.IsConfigured)
                return;
            try {
                if (repository != null) {
                    this.repository = repository;
                    this.Name = repository.Name;
                    this.ID = ID;
                    this.Description = "Repository responsible for all adhoc data objects.";
                }

                if (this.repository == null && repository != null)
                    this.repository = repository;

                this.AddObjectRepository<ObjectRepo>(new ObjectRepo(this));
            }
            catch (Exception) {

                throw;
            }
            finally {
                IsConfigured = true;
            }
        }

        public override void Create() {
            base.Create();
        }
    }
}