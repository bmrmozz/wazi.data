using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wazi.data.core;
using wazi.data.core.common.data;
using wazi.data.core.master;
using wazi.data.models;

namespace wazidataservicetest.Services
{
    public sealed class ObjectRepositoryService {
        MasterRepository parentrepository = null;
        /// <summary>
        /// Constructor that requires access to the default master repository.
        /// </summary>
        /// <param name="parentrepository"></param>
        public ObjectRepositoryService(MasterRepository parentrepository) {
            this.parentrepository = parentrepository;
        }

        /// <summary>
        /// Provides a listing of all available active repositories.
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<object> GetRepositoryList() {
            var repository = getrepository();
            if (null != repository) {
                return repository.GetAll();
            }
            else {
                return new List<object>();
            }
        }

        /// <summary>
        /// Returns a specified repository which is determined based on the id={id} value.
        /// </summary>
        /// <param name="repositoryId">This parameter is a GUID of the required repository ID which is stored within the master repository.</param>
        /// <returns>null or typeof(RepositoryConfig)</returns>
        internal object GetRepositoryById(string repositoryId) {
            var repository = getrepository();
            if (null == repository)
                return null;
            else
                return repository.Get(new List<FilterItem>() {
                     new FilterItem("ID", repositoryId, FilterOperator.equals)
                }).FirstOrDefault();
        }

        /// <summary>
        /// Adds a new repository to the master making it available to all consuming entities.
        /// </summary>
        /// <param name="newrepository">The repository to be added if valid.</param>
        internal void AddRepository(RepositoryConfig newrepository) {
            var repository = getrepository();
            if (null == repository) {
                //ToDo: Rectify exception thrown below.
                throw new Exception();
            }
            else {
                repository.SaveItem(newrepository);
            }
        }

        internal void DeleteRepository(string id) {
            //delete will only remove the first match.
            transact(repo => {
                repo.RegisterClassMapping();
                repo.DeleteOne(new List<FilterItem>() {
                    new FilterItem("ID", id)
                });
            });
        }

        /// <summary>
        /// Updates the specified repository with the new value, this will always manage versioning.
        /// </summary>
        /// <param name="repositoryId">The GUID representation of the repository which is to be updated.</param>
        /// <param name="repository">The repository which is to be updated.</param>
        internal void UpdateRepository(string repositoryId, RepositoryConfig repository) {
            //update the current repository to a state of 'archive', then insert the new one.
            transact(repo => {
                repo.RegisterClassMapping();
                repo.Update(new List<FilterItem>() { new FilterItem("ID", repositoryId) }, repository);
            });
        }

        void transact(Action<ObjectRepo> action) {
            if (this.parentrepository == null)
                throw new ArgumentNullException("Parent repository is required.");
            var repo = this.parentrepository.GetObjectRepository<RepositoryConfig>() as ObjectRepo;
            action(repo);
        }

        ObjectRepo getrepository() {
            if (this.parentrepository == null)
                return null;

            return this.parentrepository.GetObjectRepository<RepositoryConfig>() as ObjectRepo;
        }
    }
}
