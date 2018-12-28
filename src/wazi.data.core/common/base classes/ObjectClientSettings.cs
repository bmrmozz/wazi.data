using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wazi.data.core.drivers {
    public abstract class ObjectClientSettings {
        public virtual string GetConnectionString() {
            throw new NotImplementedException();
        }
    }
}
