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
        
        public string? ContactName { get; set; }
    }

    public class CustomerLookup
    {
        public string Name { get; set; }

        public string ContactName { get; set; }
    }
}
