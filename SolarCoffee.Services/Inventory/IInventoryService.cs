using System.Collections.Generic;
using SolarCoffe.Data.Models;

namespace SolarCoffee.Services.Inventory
{
    public interface IInventoryService
    {
        ProductInventory GetByProductId(int productId);
        List<ProductInventory> GetCurrentInventory();
        ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment);
        void CreateSnapshot(ProductInventory inventory);
        List<ProductInventorySnapshot> GetSnapshotHistory();


    }
}