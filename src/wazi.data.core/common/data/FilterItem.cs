using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wazi.data.core.common.data {
    public class FilterItem {
        public FilterItem() {

        }

        public FilterItem(string key, object value, FilterOperator filteroperator = FilterOperator.equals) {
            this.Key = key;
            this.Value = value;
            this.Operator = filteroperator;
        }

        public string Key { get; set; }

        public object Value { get; set; }

        public FilterOperator Operator { get; set; }
    }

    public enum FilterOperator {
        equals,
        lessthan,
        greatherthan,
        contains,
        notequals
    }
}
