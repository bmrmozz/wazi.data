using wazi.data.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wazi.data.core {
    public interface IMasterRepository {
        void Setup(RepositoryConfig repository = null);
    }
}
