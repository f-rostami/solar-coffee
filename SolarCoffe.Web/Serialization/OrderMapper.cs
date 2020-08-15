using System;
using System.Collections.Generic;
using System.Linq;
using SolarCoffe.Data.Models;
using SolarCoffe.Web.ViewModels;

namespace SolarCoffe.Web.Serialization
{
    public static class OrderMapper
    {
        public static SalesOrder SerilizationInvoiceToOrder(InvoiceModel invoice)
        {
            var salesOrderItem = invoice.LineItems
                .Select(item => new SalesOrderItem
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    Product = ProductMapper.SerializationProductModel(item.Product)
                }).ToList();

            return new SalesOrder
            {
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                SalesOrderItems = salesOrderItem
            };
        }

        public static List<OrderModel> SerializationOrdersToViewModels(IEnumerable<SalesOrder> orders)
        {
            return orders.Select(order => new OrderModel
            {
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                IsPaid = order.IsPaid,
                Id = order.Id,
                SalesOrderItems = SerializeSalesOrderItems(order.SalesOrderItems),
                Customer = CustomerMapper.SerializationCustomer(order.Customer)
            }).ToList();
        }

        private static List<SalesOrderItemModel> SerializeSalesOrderItems(IEnumerable<SalesOrderItem> orderItems)
        {
            return orderItems.Select(orderItem => new SalesOrderItemModel
            {
                Id = orderItem.Id,
                Quantity = orderItem.Quantity,
                Product = ProductMapper.SerializationProductModel(orderItem.Product)
            }).ToList();
        }
    }
}