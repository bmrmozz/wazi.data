using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wazi.data.models
{
    public class RepositoryDataObject : ConfigurationItem {

        public RepositoryDataObject() : base(false) {
        }

        public IEnumerable<DataObjectField> Fields {
            get;
            set;
        }

        public string ORID {
            get;
            set;
        }
    }


    public class DataObjectField : ConfigurationItem {
        public DOFieldBehavior Behavior {
            get;
            set;
        }

        public DODataType DataType {
            get;
            set;
        }

        public object DefaultValue {
            get;
            set;
        }

        public string OID {
            get;
            set;
        }

        public DataObjectField() : base(false) {
        }
    }

    public enum DOFieldBehavior {
        notRequired,
        requiredAlways,
        requiredOnDislay,
        requiredOnImport,
        readOnly,
        hidden
    }

    public enum DODataType {
        text,
        number,
        currency,
        date,
        datetime,
        email,
        contactNumber,
        avatar,
        list,
        color
    }
}
