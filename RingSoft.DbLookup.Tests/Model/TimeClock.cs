using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.Tests.Model
{
    public class TimeClock
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TimeClockId { get; set; }

        [Required]
        public DateTime PunchInDate { get; set; }

        public DateTime? PunchOutDate { get; set; }

        public int? CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public int? ErrorId { get; set; }

        public virtual Error Error { get; set; }

        public override string ToString()
        {
            return TimeClockId;
        }
    }

    public class TimeClockLookup
    {
        public string TimeClockId { get; set; }

        public DateTime PunchInDate { get; set; }

        public DateTime PunchOutDate { get; set; }

        public string CustomerName { get; set; }

        public string ErrorId { get; set; }
    }
}
