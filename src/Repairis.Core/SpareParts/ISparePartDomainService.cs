using Abp.Domain.Services;

namespace Repairis.SpareParts
{
    public interface ISparePartDomainService : IDomainService
    {
        void AddMapping(int sparePartId, long orderId, int quantity, decimal pricePerItem);
        void UpdateMapping(int sparePartId, long orderId, int quantity, decimal pricePerItem);
        void RemovePartsFromOrder(int sparePartId, long orderId, int? quantity);
    }
}
