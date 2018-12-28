using wazi.data.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wazi.data.core {
    public class ConfigRepo : ObjectRepositoryBase<ConfigurationItem> {
        public ConfigRepo(IRepository defaultrepo = null) :
            base(defaultrepo) {
            this.Name = "waziconfig";
            this.Prefix = "sys";
        }
    }
}
