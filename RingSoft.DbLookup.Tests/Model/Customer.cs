using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.Tests.Model
{
    public class Customer
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        public virtual ICollection<TimeClock> TimeClocks { get; set; }

        public Customer()
        {
            TimeClocks = new HashSet<TimeClock>();
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class CustomerLookup
    {
        public string Name { get; set; }

        public string ContactName { get; set; }
    }
}
