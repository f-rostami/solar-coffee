using System.Collections.Generic;
using SolarCoffe.Data;


namespace SolarCoffee.Services.Customer
{
    public interface ICustomerService
    {
          SolarCoffe.Data.Models.Customer GetById(int id);
          List<SolarCoffe.Data.Models.Customer> GetAll();
          ServiceResponse<SolarCoffe.Data.Models.Customer> Create(SolarCoffe.Data.Models.Customer customer);
          ServiceResponse<bool> Delete(int id);

    }
}