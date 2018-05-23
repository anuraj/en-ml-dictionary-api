using System;
using System.ComponentModel.DataAnnotations;

namespace Dictionary.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required, DataType(DataType.EmailAddress), EmailAddress]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}