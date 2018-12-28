using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wazi.data.models
{
    public abstract class ConfigurationItem {
        public ConfigurationItem() {

        }

        public virtual string CategoryGroupID {
            get;
            set;
        }

        public string CreatedBy {
            get;
            set;
        }

        public DateTime DateCreated {
            get;
            set;
        }

        public virtual string Description {
            get;
            set;
        }

        public virtual string DisplayName {
            get;
            set;
        }

        public virtual string Icon {
            get;
            set;
        }

        public virtual string ID {
            get;
            set;
        }

        public virtual string Name {
            get;
            set;
        }

        public virtual int OrdinalPosition {
            get;
            set;
        }

        public virtual ConfigState State {
            get;
            set;
        }

        public ConfigurationItem(bool setdefaults = false) {
            if (setdefaults) {
                this.SetDefaults(ConfigState.active, null);
            }
        }

        public virtual void SetDefaults(ConfigState defaultstate = 0, string userid = null) {
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
