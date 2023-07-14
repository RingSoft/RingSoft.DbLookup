using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.RecordLocking
{
    public class RecordLock
    {
        [Required]
        [MaxLength(50)]
        public string Table { get; set; }

        [Required]
        [MaxLength(50)]
        public string PrimaryKey { get; set; }

        [Required]
        public DateTime LockDateTime { get; set; }

        [MaxLength(50)]
        public string? User { get; set; }
    }
}
