using System;
using System.Collections.Generic;
using System.Linq;
using SolarCoffe.Data;
using SolarCoffe.Data.Models;

namespace SolarCoffee.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly SolarDbContext _db;

        public ProductService(SolarDbContext dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// Archives a Product by setting boolean IsArchived to true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResponse<SolarCoffe.Data.Models.Product> ArchiveProduct(int id)
        {
            try
            {
                var product = _db.Products.FirstOrDefault(p => p.Id == id);
                if (product != null)
                {
                    product.IsArchived = true;
                    _db.SaveChanges();
                };
                return new ServiceResponse<SolarCoffe.Data.Models.Product>
                {
                    Data = product,
                    IsSuccess = true,
                    Time = DateTime.UtcNow,
                    Message = "Product archived successfully"

                };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<SolarCoffe.Data.Models.Product>
                {
                    Data = null,
                    IsSuccess = false,
                    Time = DateTime.UtcNow,
                    Message = ex.StackTrace

                };
            }

        }

        /// <summary>
        /// Add new product to database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ServiceResponse<SolarCoffe.Data.Models.Product> CreateProduct(SolarCoffe.Data.Models.Product product)
        {
            try
            {
                _db.Products.Add(product);

                var newInventoryProduct = new ProductInventory
                {
                    Product = product,
                    QuantityOnHand = 0,
                    IdealQuantity = 10
                };

                _db.ProductInventories.Add(newInventoryProduct);

                _db.SaveChanges();

                return new ServiceResponse<SolarCoffe.Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = "Saved new product",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<SolarCoffe.Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = ex.StackTrace,
                    IsSuccess = false
                };
            }

        }

        /// <summary>
        /// Retrieves all products from database
        /// </summary>
        /// <returns>Product</returns>
        public List<SolarCoffe.Data.Models.Product> GetAllProducts()
        {
            return _db.Products.ToList();
        }


        /// <summary>
        /// Retrieve product from database by primary key id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SolarCoffe.Data.Models.Product GetProductById(int id)
        {
            // return _db.Products.SingleOrDefault(p=>p.Id == id);
            return _db.Products.Find(id);
        }
    }
}