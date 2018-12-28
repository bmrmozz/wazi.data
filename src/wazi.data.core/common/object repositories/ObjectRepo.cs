using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wazi.data.core.drivers;
using wazi.data.models;

namespace wazi.data.core
{
    public class ObjectRepo : ObjectRepositoryBase<RepositoryConfig> {
        public ObjectRepo(IRepository defaultrepo = null) : base(defaultrepo) {
            base.Name = "rcos_data";
            base.Prefix = "mst";
        }

        public override void SetDefaults(IObjectClient client) {
            RepositoryConfig config;
            string orid = Guid.NewGuid().ToString();
            Guid oid = Guid.NewGuid();
            RepositoryConfig masterConfig = base.Repository.GetConfig();
            RepositoryConfig repositoryConfig = new RepositoryConfig() {
                Type = RepositoryType.data,
                CategoryGroupID = Guid.NewGuid().ToString(),
                Connector = ConnectorType.Mongo,
                Description = "Default data repository for custom data object definitions",
                DisplayName = "Default Data Repository",
                ID = orid,
                Name = "Default Data Repository",
                Servers = masterConfig.Servers,
                Username = "dat_user"
            };
            List<RepositoryDataObject> list = new List<RepositoryDataObject>();
            RepositoryDataObject repositoryDataObject = new RepositoryDataObject() {
                CategoryGroupID = Guid.NewGuid().ToString(),
                Description = "Example data object",
                DisplayName = "Example",
                ID = oid.ToString(),
                Name = "Example",
                ORID = orid
            };
            List<DataObjectField> list1 = new List<DataObjectField>();
            list1.Add(new DataObjectField() {
                ID = Guid.NewGuid().ToString(),
                Behavior = DOFieldBehavior.requiredAlways,
                CategoryGroupID = Guid.NewGuid().ToString(),
                DataType = DODataType.text,
                DefaultValue = "default value",
                Description = "required always field",
                DisplayName = "field 1",
                Name = "field1",
                OID = oid.ToString()
            });
            list1.Add(new DataObjectField() {
                ID = Guid.NewGuid().ToString(),
                Behavior = DOFieldBehavior.hidden,
                CategoryGroupID = Guid.NewGuid().ToString(),
                DataType = DODataType.text,
                DefaultValue = "default value",
                Description = "hidden field",
                DisplayName = "field 2",
                Name = "field2",
                OID = oid.ToString()
            });
            list1.Add(new DataObjectField() {
                ID = Guid.NewGuid().ToString(),
                Behavior = DOFieldBehavior.readOnly,
                CategoryGroupID = Guid.NewGuid().ToString(),
                DataType = DODataType.text,
                DefaultValue = "default value",
                Description = "read only field",
                DisplayName = "field 3",
                Name = "field3",
                OID = oid.ToString()
            });
            repositoryDataObject.Fields = list1;
            list.Add(repositoryDataObject);
            repositoryConfig.DataObjects = list;
            RepositoryConfig defaultDataRepository = repositoryConfig;
            IRepository repository = base.Repository;
            if (repository != null) {
                config = repository.GetConfig();
            }
            else {
                config = null;
            }
            this.SaveItem(config);
            this.SaveItem(defaultDataRepository);
        }
    }
}