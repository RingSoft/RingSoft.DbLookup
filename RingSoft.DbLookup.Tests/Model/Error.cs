using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.Tests.Model
{
    public class Error
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ErrorId { get; set; }

        public virtual ICollection<TimeClock> TimeClocks { get; set; }

        public Error()
        {
            TimeClocks = new HashSet<TimeClock>();
        }

        public override string ToString()
        {
            return ErrorId;
        }
    }

    public class ErrorLookup
    {
        public string ErrorId { get; set; }
    }

}
