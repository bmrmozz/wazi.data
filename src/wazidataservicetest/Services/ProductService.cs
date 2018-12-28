using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wazi.data.core;
using wazi.data.core.common.data;
using wazidataservicetest.Repositories;

namespace wazidataservicetest.Services
{
    public class ProductService<T>
    {
        private IRepository repository = null;

        public ProductService(IRepository repository) {
            this.repository = repository;
        }

        public T GetProduct(string productId) {
            var repo = this.repository.GetObjectRepository<ProductItem>() as ProductRepo;
            return (T)repo.Get(new List<FilterItem> { new FilterItem("ID", productId) }).FirstOrDefault();
        }

        public IEnumerable<object> GetProductListing() {
            var thisrepo = (this.repository.GetObjectRepository<ProductItem>() as ProductRepo);

            if (null == thisrepo) {
                thisrepo = new ProductRepo(repository);
                repository.AddObjectRepository<ProductItem>(thisrepo);
            }

            return (this.repository.GetObjectRepository<ProductItem>() as ProductRepo).GetAll();
        }

        public void AddProduct(ProductItem product) {
            transact(repo => {
                repo.SaveItem(product);
            });
        }

        public void RemoveProduct(string productid) {
            transact(repo => {
                repo.DeleteOne(new List<FilterItem> {
                    new FilterItem("ID", productid)
                });
            });
        }

        public void UpdateProduct(string productid, ProductItem product) {
            transact(repo => {
                repo.Update(new List<FilterItem> {
                    new FilterItem ("ID", product)
                }, product);
            });
        }


        void transact(Action<ProductRepo> action) {
            if (this.repository == null)
                throw new ArgumentNullException("Parent repository is required.");
            var repo = this.repository.GetObjectRepository<ProductItem>() as ProductRepo;
            if (null == repo) {
                repo = new Repositories.ProductRepo(this.repository);
                this.repository.AddObjectRepository<ProductItem>(repo);
            }
            action(repo);
        }
    }
}
