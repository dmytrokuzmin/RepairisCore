using System;
using Abp.Application.Services.Dto;

namespace Repairis.DeviceModels.Dto
{
    public class DeviceModelAutocompleteDto : EntityDto<string>
    {
        public string DeviceModelName { get; set; }
        public string DeviceCategoryName { get; set; }
    }
}
