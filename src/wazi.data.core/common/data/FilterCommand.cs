using wazi.data.core.common.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wazi.data.core.common {
    public class FilterCommand {
        private IEnumerable<FilterItem> filters = null;

        public FilterCommand() {
        }

        public IEnumerable<FilterItem> InFilters {
            get {
                if (filters == null)
                    return null;

                return filters.Where(f => f.Operator == FilterOperator.equals);
            }
        }

        public void AddFilter(string field, object value, FilterOperator filteroperator) {
            addFilter(field, value, filteroperator);
        }

        private void addFilter(string field, object value, FilterOperator filteroperator) {
            if (null == filters)
                filters = new List<FilterItem>();

            //now lets add them accordingly.
            (this.filters as List<FilterItem>).Add(new FilterItem(field, value, filteroperator));
        }
    }
}
