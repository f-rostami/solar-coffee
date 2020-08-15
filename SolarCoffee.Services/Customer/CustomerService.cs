using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SolarCoffe.Data;

namespace SolarCoffee.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly SolarDbContext _db;

        public CustomerService(SolarDbContext db)
        {
            _db = db;
        }

        public ServiceResponse<SolarCoffe.Data.Models.Customer> Create(SolarCoffe.Data.Models.Customer customer)
        {
            try
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return new ServiceResponse<SolarCoffe.Data.Models.Customer>
                {
                    Data = customer,
                    IsSuccess = true,
                    Message = "New customer add to database",
                    Time = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<SolarCoffe.Data.Models.Customer>
                {
                    Data = customer,
                    IsSuccess = false,
                    Message = ex.StackTrace,
                    Time = DateTime.Now
                };
            }

        }

        public ServiceResponse<bool> Delete(int id)
        {
            var customer = _db.Customers.Find(id);
            if (customer == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    Message = "Can't find customer for delete",
                    Time = DateTime.UtcNow
                };
            }

            try
            {

                _db.Customers.Remove(customer);
                _db.SaveChanges();
                return new ServiceResponse<bool>
                {
                    Data = true,
                    IsSuccess = true,
                    Message = "Customer delete successfully.",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    Message = ex.StackTrace,
                    Time = DateTime.UtcNow
                };
            }
        }

        public List<SolarCoffe.Data.Models.Customer> GetAll()
        {
            return _db.Customers
            .Include(customer => customer.PrimaryAddress)
            .OrderBy(cusomter => cusomter.LastName)
            .ToList();
        }

        public SolarCoffe.Data.Models.Customer GetById(int id)
        {
            return _db.Customers.Find(id);
        }
    }
}