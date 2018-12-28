using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wazi.data.core
{
    public abstract class StorageObjectBase
    {
        public StorageObjectBase(bool setDefaults = false) {
            if (setDefaults)
                this.SetDefaults();
        }

        public virtual string ID { get; set; }
        public virtual int OrdinalPosition { get; set; }
        public virtual string Name { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string Description { get; set; }
        public virtual string CategoryGroupID { get; set; }
        public virtual string Icon { get; set; }
        public virtual ConfigState State { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }

        public virtual void SetDefaults(ConfigState defaultstate = ConfigState.active, string userid = null) {
            this.ID = Guid.NewGuid().ToString();
            this.OrdinalPosition = 0;
            this.CategoryGroupID = null;
            this.State = defaultstate;
            this.DateCreated = DateTime.Now;
            this.CreatedBy = userid;
        }
    }

    public enum ConfigState {
        active,
        archived,
        deleted,
        pendingdelete,
        pendingarchive,
        disabled
    }
}
