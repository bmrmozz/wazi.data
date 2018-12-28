using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using wazidataservicetest.Services;
using wazi.data.core.master;
using wazi.data.models;
using Microsoft.Extensions.Options;

namespace wazidataservicetest.Controllers
{
    /// <summary>
    /// Repository controller makes use of the repository service to access all repositories available.
    /// </summary>
    [Route("api/repository")]
    [EnableCors("CorsPolicy")]
    public class RepositoryController : Controller {
        #region Class Members
        ObjectRepositoryService repositoryService = null;
        MasterRepository masterrepository = null;
        #endregion

        /// <summary>
        /// Creates the repository controller with the master repository as an injectable.
        /// </summary>
        /// <param name="masterrepository">Master repository used to access all other default repositories.</param>
        public RepositoryController(IOptions<MasterRepository> masterrepository) {
            this.masterrepository = masterrepository.Value;
            this.repositoryService = new ObjectRepositoryService(masterrepository.Value);
        }

        /// <summary>
        /// Provides a listing of all available repositories.
        /// </summary>
        /// <returns>A collection of 'RepositoryConfig' objects.</returns>
        /// GET api/repository/GetRepositoryList
        [HttpGet]
        [Route("GetRepositoryList")]
        public JsonResult GetRepositoryList() {
            return Json(this.repositoryService.GetRepositoryList());
        }

        /// <summary>
        /// Returns a specified repository which is determined based on the id={id} value.
        /// </summary>
        /// <param name="id">This parameter is a GUID of the required repository ID which is stored within the master repository.</param>
        /// <returns>null or typeof(RepositoryConfig)</returns>
        /// GET api/repository/GetRepository/id={id}
        [HttpGet]
        [Route("GetRepository/id={id}")]
        public JsonResult GetRepository(string id) {
            return Json(this.repositoryService.GetRepositoryById(id));
        }
        /// <summary>
        /// Adds a new repository to the master making it available to all consuming entities.
        /// </summary>
        /// <param name="newrepository">The repository to be added if valid.</param>
        /// POST api/AddRepository
        [HttpPost]
        [Route("AddRepository")]
        public void AddRepository([FromBody]RepositoryConfig newrepository) {
            if (null == newrepository)
                Response.StatusCode = 204;

            this.repositoryService.AddRepository(newrepository);
        }

        /// <summary>
        /// Updates the specified repository with the new value, this will always manage versioning.
        /// </summary>
        /// <param name="id">The GUID representation of the repository which is to be updated.</param>
        /// <param name="repository">The repository which is to be updated.</param>
        /// PUT api/repository/UpdateRepository/id={id}
        [HttpPut("{id}")]
        [Route("UpdateRepository/id={id}")]
        public void UpdateRepository(string id, [FromBody]RepositoryConfig repository) {
            this.repositoryService.UpdateRepository(id, repository);
        }

        /// <summary>
        /// Deletes the specified repository if there are no conflicts.
        /// </summary>
        /// <param name="id">The GUID representation of the repository which is to be deleted.</param>
        /// DELETE api/repository/DeleteRepository/id={id}
        [HttpDelete("{id}")]
        [Route("DeleteRepository/id={id}")]
        public void DeleteRepository(string id) {
            this.repositoryService.DeleteRepository(id);
        }
    }
}
