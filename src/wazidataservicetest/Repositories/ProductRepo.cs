using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wazi.data.core;
using wazi.data.core.drivers;

namespace wazidataservicetest.Repositories
{
    public class ProductRepo : ObjectRepositoryBase<ProductItem>
    {
        public ProductRepo(IRepository defaultrepo = null) :
            base(defaultrepo) {
            this.Name = "waziproducts";
            this.Prefix = "dat";
        }

        public override void SetDefaults(IObjectClient client) {
            var defaultproduct = new ProductItem {
                name =  "default product",
                productId = Guid.NewGuid().ToString(),
                productCode = "450125825"
            };

            defaultproduct.SetDefaults();
            base.SetDefaults(client);
        }

        public override void SaveItem(ProductItem item) {
            item.SetDefaults();
            base.SaveItem(item);
        }

        public override void Save(IEnumerable<ProductItem> items) {
            base.Save(items);
        }
    }

    public class ProductItem : StorageObjectBase {
        public ProductItem() {
            
        }

        public string productCode { get; set; }
        public string name { get; set; }
        public string productId { get; set; }
    }
}
