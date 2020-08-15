using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffe.Data;
using SolarCoffe.Data.Models;

namespace SolarCoffee.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly SolarDbContext _db;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(SolarDbContext db, ILogger<InventoryService> logger)
        {
            _db = db;
            _logger = logger;
        }


        public ProductInventory GetByProductId(int productId)
        {
            return _db.ProductInventories
                    .Include(inv => inv.Product)
                    .FirstOrDefault(inv => inv.Product.Id == productId);
        }

        public List<ProductInventory> GetCurrentInventory()
        {
            return _db.ProductInventories
                .Include(pInv => pInv.Product)
                .Where(pInv => !pInv.Product.IsArchived)
                .ToList();
        }

        //return snapshot history 6 hours ago
        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            var earlier = DateTime.Now - TimeSpan.FromHours(6);
            return _db.ProductInventorySnapshots
                        .Include(snap => snap.Product)
                        .Where(snap => snap.SnapshotTime > earlier && !snap.Product.IsArchived)
                        .ToList();
        }

        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment)
        {
            try
            {
                var inventory = _db.ProductInventories
                            .Include(pInv => pInv.Product)
                            .First(pInv => pInv.Product.Id == id);
                inventory.QuantityOnHand += adjustment;

                try
                {
                    CreateSnapshot(inventory);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error on created snapshot.");
                    _logger.LogError(ex.StackTrace);
                }

                _db.SaveChanges();

                return new ServiceResponse<ProductInventory>
                {
                    Data = inventory,
                    IsSuccess = true,
                    Message = "Inventory Updated.",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ProductInventory>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = ex.StackTrace,
                    Time = DateTime.UtcNow
                };
            }
        }


        public void CreateSnapshot(ProductInventory inventory)
        {
            var now = DateTime.UtcNow;
            var snapshot = new ProductInventorySnapshot
            {
                Product = inventory.Product,
                QuantityOnHand = inventory.QuantityOnHand,
                SnapshotTime = now
            };

            _db.ProductInventorySnapshots.Add(snapshot);
        }

        
    }
}