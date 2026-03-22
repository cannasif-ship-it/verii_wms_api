using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_PLATFORM_PAGE_GROUP")]
    public class PlatformPageGroup : BaseEntity
    {

        [Required, MaxLength(100)]
        public string GroupName { get; set; } = string.Empty;
        
        [Required, MaxLength(100)]
        public string GroupCode { get; set; } = string.Empty;

    }
}
