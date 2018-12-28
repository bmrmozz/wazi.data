using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wazi.data.core.common
{
    public class GenericRepo<T> : ObjectRepositoryBase<T> 
        where T : StorageObjectBase
    {
        public GenericRepo(IRepository repository) : base (repository) {
            var repoobject = Activator.CreateInstance<T>();
            this.Name = this.DisplayName = repoobject.ToString();
            this.Prefix = "dat";
        }

        public override void SaveItem(T item) {
            item.SetDefaults();
            base.SaveItem(item);
        }
    }
}
