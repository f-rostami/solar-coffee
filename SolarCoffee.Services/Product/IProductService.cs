using System.Collections.Generic;

namespace SolarCoffee.Services.Product
{
    public interface IProductService
    {
        List<SolarCoffe.Data.Models.Product> GetAllProducts();
        SolarCoffe.Data.Models.Product GetProductById(int id);
        ServiceResponse<SolarCoffe.Data.Models.Product> CreateProduct(SolarCoffe.Data.Models.Product product);
        ServiceResponse<SolarCoffe.Data.Models.Product> ArchiveProduct(int id);
    }

}