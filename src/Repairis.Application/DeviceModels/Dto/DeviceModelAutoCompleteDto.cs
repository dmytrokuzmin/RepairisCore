using System;
using Abp.Application.Services.Dto;
using Abp.Localization;

namespace Repairis.DeviceModels.Dto
{
    public class DeviceModelAutocompleteDto : EntityDto<string>
    {
        public string DeviceModelName { get; set; }
        public string DeviceCategoryName { get; set; }
    }
}
