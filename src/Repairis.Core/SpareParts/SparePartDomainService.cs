using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

namespace Repairis.SpareParts
{
    public class SparePartDomainService : DomainService, ISparePartDomainService
    {
        private readonly IRepository<SparePart> _sparePartRepository;
        private readonly IRepository<SparePartOrderMapping, string> _orderMappingRepository;

        public SparePartDomainService(IRepository<SparePart> sparePartRepository, IRepository<SparePartOrderMapping, string> orderMappingRepository)
        {
            _sparePartRepository = sparePartRepository;
            _orderMappingRepository = orderMappingRepository;
            LocalizationSourceName = RepairisConsts.LocalizationSourceName;
        }


        public void AddMapping(int sparePartId, long orderId, int quantity, decimal pricePerItem)
        {
            var sparePart = _sparePartRepository.GetAllIncluding(x => x.SparePartOrders).FirstOrDefault(x => x.Id == sparePartId);

            var existingMapping = sparePart.SparePartOrders.FirstOrDefault(x => x.OrderId == orderId && x.SparePartId == sparePartId);
            if (existingMapping != null)
            {
                sparePart.StockQuantity -= -quantity;
                existingMapping.PricePerItem =
                    (existingMapping.PricePerItem * existingMapping.Quantity + pricePerItem * quantity) /
                    (existingMapping.Quantity + quantity);
                existingMapping.Quantity += quantity;
            }
            else
            {
                sparePart.SparePartOrders.Add(
                    new SparePartOrderMapping
                    {
                        SparePartId = sparePartId,
                        SparePart = sparePart,
                        OrderId = orderId,
                        Quantity = quantity,
                        PricePerItem = pricePerItem
                    });
                sparePart.StockQuantity -= quantity;
            }

            if (sparePart.StockQuantity <= 0)
            {
                sparePart.StockStatus = StockStatusEnum.OutOfStock;
            }

            _sparePartRepository.Update(sparePart);
        }

        public void UpdateMapping(int sparePartId, long orderId, int quantity, decimal pricePerItem)
        {
            var sparePart = _sparePartRepository.GetAllIncluding(x => x.SparePartOrders).FirstOrDefault(x => x.Id == sparePartId);
            if (sparePart == null)
            {
                return;
            }

            var existingMapping = sparePart.SparePartOrders.FirstOrDefault(x => x.OrderId == orderId && x.SparePartId == sparePartId);
            if (existingMapping != null)
            {
                sparePart.StockQuantity += existingMapping.Quantity - quantity;
                existingMapping.PricePerItem = pricePerItem;
                existingMapping.Quantity = quantity;
            }
            else
            {
                sparePart.SparePartOrders.Add(
                    new SparePartOrderMapping
                    {
                        SparePartId = sparePartId,
                        SparePart = sparePart,
                        OrderId = orderId,
                        Quantity = quantity,
                        PricePerItem = pricePerItem
                    });
                sparePart.StockQuantity -= quantity;
            }

            if (sparePart.StockQuantity <= 0)
            {
                sparePart.StockStatus = StockStatusEnum.OutOfStock;
            }

            _sparePartRepository.Update(sparePart);
        }


        public void RemovePartsFromOrder(int sparePartId, long orderId, int? quantity)
        {
            var sparePart = _sparePartRepository.GetAllIncluding(x => x.SparePartOrders).FirstOrDefault(x => x.Id == sparePartId);
            if (sparePart == null)
            {
                return;
            }

            var mapping = sparePart.SparePartOrders.FirstOrDefault(x => x.OrderId == orderId && x.SparePartId == sparePartId);
            if (mapping == null)
            {
                return;
            }

            if (quantity == null)
            {
                //Remove all and return them to stock
                sparePart.StockQuantity += mapping.Quantity;
                sparePart.SparePartOrders.RemoveAll(x => x.SparePartId == sparePartId);
                _orderMappingRepository.Delete(mapping);
            }
            else
            {
                sparePart.StockQuantity += quantity.Value;
                mapping.Quantity -= quantity.Value;
            }

            if (sparePart.StockStatus == StockStatusEnum.OutOfStock && sparePart.StockQuantity > 0)
            {
                sparePart.StockStatus = StockStatusEnum.InStock;
            }


            if (sparePart.StockStatus == StockStatusEnum.InStock && sparePart.StockQuantity <= 0)
            {
                sparePart.StockStatus = StockStatusEnum.OutOfStock;
            }

            _sparePartRepository.Update(sparePart);
        }
    }
}
