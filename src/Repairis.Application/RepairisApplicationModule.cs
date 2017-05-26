using System;
using System.Linq;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Repairis.Authorization;
using Repairis.Authorization.Users;
using Repairis.DeviceModels;
using Repairis.DeviceModels.Dto;
using Repairis.Devices;
using Repairis.Devices.Dto;
using Repairis.Helpers;
using Repairis.Orders;
using Repairis.Orders.Dto;
using Repairis.SpareParts;
using Repairis.SpareParts.Dto;
using Repairis.Users.Dto;

namespace Repairis
{
    [DependsOn(
        typeof(RepairisCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class RepairisApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<RepairisAuthorizationProvider>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                //DeviceModel -> DeviceModelBasicEntityDto
                cfg.CreateMap<DeviceModel, DeviceModelBasicEntityDto>()
                .ForMember(dest => dest.DeviceCategoryName, opt => opt.MapFrom(src => src.DeviceCategory.DeviceCategoryName))
                    .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
                    .ForMember(dest => dest.DeviceModelName, opt => opt.MapFrom(src => src.DeviceModelName))
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

                cfg.CreateMap<DeviceModel, DeviceModelFullEntityDto>()
                    .ForMember(dest => dest.DeviceCategoryName, opt => opt.MapFrom(src => src.DeviceCategory.DeviceCategoryName))
                    .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
                    .ForMember(dest => dest.DeviceModelName, opt => opt.MapFrom(src => src.DeviceModelName))
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                    .ForMember(dest => dest.Devices, opt => opt.MapFrom(src => src.Devices))
                    .ForMember(dest => dest.CompatibleSpareParts, opt => opt.MapFrom(src => src.CompatibleSpareParts));


                cfg.CreateMap<DeviceModel, DeviceModelAutocompleteDto>()
                    .ForMember(dest => dest.DeviceModelName, opt => opt.MapFrom(src => src.Brand.BrandName + " " + src.DeviceModelName))
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                    .ForMember(dest => dest.DeviceCategoryName, opt => opt.MapFrom(src => src.DeviceCategory.DeviceCategoryName));


                //Device -> DeviceBasicEntityDto
                cfg.CreateMap<Device, DeviceBasicEntityDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.DeviceCategoryName, opt => opt.MapFrom(src => src.DeviceModel.DeviceCategory.DeviceCategoryName))
                    .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.DeviceModel.Brand.BrandName))
                    .ForMember(dest => dest.DeviceModelName, opt => opt.MapFrom(src => src.DeviceModel.DeviceModelName))
                    .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(src => src.SerialNumber));

                //Order -> OrderBasicEntityDto
                cfg.CreateMap<Order, OrderBasicEntityDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.AssignedMasterFullName, opt => opt.MapFrom(src => src.AssignedEmployee.EmployeeUser.Surname + " " + src.AssignedEmployee.EmployeeUser.Name))
                    .ForMember(dest => dest.CustomerFullName, opt => opt.MapFrom(src => src.Customer.CustomerUser.Surname + " " + src.Customer.CustomerUser.Name + " " + src.Customer.CustomerUser.FatherName))
                    .ForMember(dest => dest.CustomerPhoneNumber, opt => opt.MapFrom(src => src.Customer.CustomerUser.PhoneNumber))
                    .ForMember(dest => dest.CustomerEmailAddress, opt => opt.MapFrom(src => src.Customer.CustomerUser.EmailAddress))
                    .ForMember(dest => dest.OrderCreationDate, opt => opt.MapFrom(src => src.CreationTime))
                    .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus))
                    .ForMember(dest => dest.IsUrgent, opt => opt.MapFrom(src => src.IsUrgent))
                    .ForMember(dest => dest.IsWarrantyComplaint, opt => opt.MapFrom(src => src.IsWarrantyComplaint))
                    .ForMember(dest => dest.IssueDescription, opt => opt.MapFrom(src => src.IssueDescription))
                    .ForMember(dest => dest.DeviceModel, opt => opt.MapFrom(src =>
                        src.Device.DeviceModel.DeviceCategory.DeviceCategoryName + " " +
                        src.Device.DeviceModel.Brand.BrandName + " " +
                        src.Device.DeviceModel.DeviceModelName));

                cfg.CreateMap<Order, OrderReportItemDto>()
                    .ForMember(dest => dest.AssignedMasterFullName, opt => opt.MapFrom(src => src.AssignedEmployee.EmployeeUser.Surname + " " + src.AssignedEmployee.EmployeeUser.Name)).ForMember(dest => dest.AssignedMasterFullName, opt => opt.MapFrom(src => src.AssignedEmployee.EmployeeUser.Surname + " " + src.AssignedEmployee.EmployeeUser.Name))
                    .ForMember(dest => dest.DevicePickupDate, opt => opt.MapFrom(src => src.DevicePickupDate))
                    .ForMember(dest => dest.RepairPrice, opt => opt.MapFrom(src => src.RepairPrice ?? 0))
                    .ForMember(dest => dest.SparePartsTotalCost, opt => opt.MapFrom(src => src.SparePartsUsed.Sum(x => x.PricePerItem * x.Quantity)))
                    .ForMember(dest => dest.SparePartsTotalSupplierCost, opt => opt.MapFrom(src => src.SparePartsUsed.Sum(x => x.SparePart.SupplierPrice * x.Quantity)));


                //SparePart -> SparePartBasicEntityDto
                cfg.CreateMap<SparePart, SparePartBasicEntityDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
                    .ForMember(dest => dest.SparePartName, opt => opt.MapFrom(src => src.SparePartName))
                    .ForMember(dest => dest.SparePartCode, opt => opt.MapFrom(src => src.SparePartCode))
                    .ForMember(dest => dest.SupplierPrice, opt => opt.MapFrom(src => src.SupplierPrice))
                    .ForMember(dest => dest.RecommendedPrice, opt => opt.MapFrom(src => src.RecommendedPrice))
                    .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                    .ForMember(dest => dest.StockStatus, opt => opt.MapFrom(src => src.StockStatus))
                    .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity));


                //SparePart -> SparePartFullEntityDto
                cfg.CreateMap<SparePart, SparePartFullEntityDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
                    .ForMember(dest => dest.SparePartName, opt => opt.MapFrom(src => src.SparePartName))
                    .ForMember(dest => dest.SparePartCode, opt => opt.MapFrom(src => src.SparePartCode))
                    .ForMember(dest => dest.SupplierPrice, opt => opt.MapFrom(src => src.SupplierPrice))
                    .ForMember(dest => dest.RecommendedPrice, opt => opt.MapFrom(src => src.RecommendedPrice))
                    .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                    .ForMember(dest => dest.StockStatus, opt => opt.MapFrom(src => src.StockStatus))
                    .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
                    .ForMember(dest => dest.CompatibleDeviceModelIds, opt => opt.MapFrom(src => src.CompatibleDeviceModels.Select(x => x.DeviceModelId)));



                cfg.CreateMap<List<Order>, ListResultDto<OrderBasicEntityDto>>()
                    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src));



                //User -> CustomerBasicEntityDto
                cfg.CreateMap<User, CustomerBasicEntityDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                    .ForMember(dest => dest.FatherName, opt => opt.MapFrom(src => src.FatherName))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                    .ForMember(dest => dest.SecondaryPhoneNumber, opt => opt.MapFrom(src => src.SecondaryPhoneNumber))
                    .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
                    .ForMember(dest => dest.AdditionalInfo, opt => opt.MapFrom(src => src.CustomerInfo.AdditionalInfo))
                    .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => src.CustomerInfo.CustomerType));


                cfg.CreateMap<CustomerInfo, CustomerBasicEntityDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CustomerUser.Name))
                    .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.CustomerUser.Surname))
                    .ForMember(dest => dest.FatherName, opt => opt.MapFrom(src => src.CustomerUser.FatherName))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.CustomerUser.Address))
                    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.CustomerUser.PhoneNumber))
                    .ForMember(dest => dest.SecondaryPhoneNumber, opt => opt.MapFrom(src => src.CustomerUser.SecondaryPhoneNumber))
                    .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.CustomerUser.EmailAddress))
                    .ForMember(dest => dest.AdditionalInfo, opt => opt.MapFrom(src => src.AdditionalInfo))
                    .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => src.CustomerType));


                cfg.CreateMap<EmployeeInfo, EmployeeBasicEntityDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.EmployeeUser.Name))
                    .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.EmployeeUser.Surname))
                    .ForMember(dest => dest.FatherName, opt => opt.MapFrom(src => src.EmployeeUser.FatherName))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.EmployeeUser.Address))
                    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.EmployeeUser.PhoneNumber))
                    .ForMember(dest => dest.SecondaryPhoneNumber, opt => opt.MapFrom(src => src.EmployeeUser.SecondaryPhoneNumber))
                    .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmployeeUser.EmailAddress))
                    .ForMember(dest => dest.SalaryValue, opt => opt.MapFrom(src => src.SalaryValue))
                    .ForMember(dest => dest.SalaryIsFlat, opt => opt.MapFrom(src => src.SalaryIsFlat));

                cfg.CreateMap<User, CustomerFullEntityDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                    .ForMember(dest => dest.FatherName, opt => opt.MapFrom(src => src.FatherName))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                    .ForMember(dest => dest.SecondaryPhoneNumber, opt => opt.MapFrom(src => src.SecondaryPhoneNumber))
                    .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
                    .ForMember(dest => dest.AdditionalInfo, opt => opt.MapFrom(src => src.CustomerInfo.AdditionalInfo))
                    .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => src.CustomerInfo.CustomerType))
                    .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.CustomerInfo.CustomerOrders));

                cfg.CreateMap<User, EmployeeBasicEntityDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                    .ForMember(dest => dest.FatherName, opt => opt.MapFrom(src => src.FatherName))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                    .ForMember(dest => dest.SecondaryPhoneNumber, opt => opt.MapFrom(src => src.SecondaryPhoneNumber))
                    .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
                    .ForMember(dest => dest.SalaryValue, opt => opt.MapFrom(src => src.EmployeeInfo.SalaryValue))
                    .ForMember(dest => dest.SalaryIsFlat, opt => opt.MapFrom(src => src.EmployeeInfo.SalaryIsFlat));
                    

                cfg.CreateMap<User, EmployeeFullEntityDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                    .ForMember(dest => dest.FatherName, opt => opt.MapFrom(src => src.FatherName))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                    .ForMember(dest => dest.SecondaryPhoneNumber, opt => opt.MapFrom(src => src.SecondaryPhoneNumber))
                    .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
                    .ForMember(dest => dest.SalaryValue, opt => opt.MapFrom(src => src.EmployeeInfo.SalaryValue))
                    .ForMember(dest => dest.SalaryIsFlat, opt => opt.MapFrom(src => src.EmployeeInfo.SalaryIsFlat))
                    .ForMember(dest => dest.AssignedOrders, opt => opt.MapFrom(src => src.EmployeeInfo.AssignedOrders));

                cfg.CreateMap<EmployeeInfo, EmployeeDropDownListDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.EmployeeUser.Name))
                    .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.EmployeeUser.Surname))
                    .ForMember(dest => dest.FatherName, opt => opt.MapFrom(src => src.EmployeeUser.FatherName));

                //Order -> OrderFullEntityDto
                cfg.CreateMap<Order, OrderFullEntityDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus))
                    .ForMember(dest => dest.IsUrgent, opt => opt.MapFrom(src => src.IsUrgent))
                    .ForMember(dest => dest.IsWarrantyComplaint, opt => opt.MapFrom(src => src.IsWarrantyComplaint))
                    //.ForMember(dest => dest.AssignedEmployee, opt => opt.MapFrom(src => src.AssignedEmployee.EmployeeUser))
                    .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer.CustomerUser))
                    .ForMember(dest => dest.Device, opt => opt.MapFrom(src => src.Device))
                    .ForMember(dest => dest.IssueDescription, opt => opt.MapFrom(src => src.IssueDescription))
                    .ForMember(dest => dest.AdditionalEquipment, opt => opt.MapFrom(src => src.AdditionalEquipment))
                    .ForMember(dest => dest.AdditionalNotes, opt => opt.MapFrom(src => src.AdditionalNotes))
                    .ForMember(dest => dest.DevicePickupDate, opt => opt.MapFrom(src => src.DevicePickupDate))
                    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                    .ForMember(dest => dest.IsRepaired, opt => opt.MapFrom(src => src.IsRepaired))
                    .ForMember(dest => dest.OrderRepairedDate, opt => opt.MapFrom(src => src.OrderRepairedDate))
                    .ForMember(dest => dest.SparePartsUsed, opt => opt.MapFrom(src => src.SparePartsUsed))
                    .ForMember(dest => dest.RepairPrice, opt => opt.MapFrom(src => src.RepairPrice))
                    .ForMember(dest => dest.WorkDoneDescripton, opt => opt.MapFrom(src => src.WorkDoneDescripton))
                    .ForMember(dest => dest.AssignedEmployeeId, opt => opt.MapFrom(src => src.AssignedEmployeeId ?? 0));


                cfg.CreateMap<Order, OrderCompletionDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                    .ForMember(dest => dest.IsUrgent, opt => opt.MapFrom(src => src.IsUrgent))
                    .ForMember(dest => dest.IsWarrantyComplaint, opt => opt.MapFrom(src => src.IsWarrantyComplaint))
                    .ForMember(dest => dest.CustomerFullName, opt => opt.MapFrom(src => src.Customer.CustomerUser.FullName))
                    .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => src.Customer.CustomerType))
                    .ForMember(dest => dest.Device, opt => opt.MapFrom(src => src.Device))
                    .ForMember(dest => dest.AdditionalEquipment, opt => opt.MapFrom(src => src.AdditionalEquipment))
                    .ForMember(dest => dest.AdditionalNotes, opt => opt.MapFrom(src => src.AdditionalNotes))
                    .ForMember(dest => dest.OrderRepairedDate, opt => opt.MapFrom(src => src.OrderRepairedDate ?? DateTime.Now))
                    .ForMember(dest => dest.DevicePickupDate, opt => opt.MapFrom(src => src.DevicePickupDate ?? DateTime.Now))
                    .ForMember(dest => dest.SparePartsUsed, opt => opt.MapFrom(src => src.SparePartsUsed))
                    .ForMember(dest => dest.RepairPrice, opt => opt.MapFrom(src => src.RepairPrice ?? 0))
                    .ForMember(dest => dest.WorkDoneDescripton, opt => opt.MapFrom(src => src.WorkDoneDescripton))
                    .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus));

                //OrderFullEntityDto -> Order
                //cfg.CreateMap<OrderFullEntityDto, Order>()
                //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                //    .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus))
                //    .ForMember(dest => dest.IsUrgent, opt => opt.MapFrom(src => src.IsUrgent))
                //    .ForMember(dest => dest.IsWarrantyComplaint, opt => opt.MapFrom(src => src.IsWarrantyComplaint))
                //    .ForMember(dest => dest.AssignedUserId, opt => opt.MapFrom(src => src.AssignedMasterId))
                //    .ForMember(dest => dest.CustomerUserId, opt => opt.MapFrom(src => src.Customer.Id))
                //    .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.Device.Id))
                //    .ForMember(dest => dest.IssueDescription, opt => opt.MapFrom(src => src.IssueDescription))
                //    .ForMember(dest => dest.AdditionalEquipment, opt => opt.MapFrom(src => src.AdditionalEquipment))
                //    .ForMember(dest => dest.AdditionalNotes, opt => opt.MapFrom(src => src.AdditionalNotes))
                //    .ForMember(dest => dest.DevicePickupDate, opt => opt.MapFrom(src => src.DevicePickupDate))
                //    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                //    .ForMember(dest => dest.IsRepaired, opt => opt.MapFrom(src => src.IsRepaired))
                //    .ForMember(dest => dest.OrderRepairedDate, opt => opt.MapFrom(src => src.OrderRepairedDate))
                //    .ForMember(dest => dest.RepairPrice, opt => opt.MapFrom(src => src.RepairPrice))
                //    .ForMember(dest => dest.WorkDoneDescripton, opt => opt.MapFrom(src => src.WorkDoneDescripton))
                //    .ForMember(dest => dest.SparePartsUsed, opt => opt.MapFrom(src => new List<SparePart>()))
                //    ;
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RepairisApplicationModule).GetAssembly());
        }
    }
}