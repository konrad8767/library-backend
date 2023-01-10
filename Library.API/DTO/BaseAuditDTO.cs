using System;

namespace Library.API.DTO
{
    public class BaseAuditDTO
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int CreatedById { get; set; }
        public int UpdatedById { get; set; }
    }
}
