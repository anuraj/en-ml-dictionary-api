using System;
using System.ComponentModel.DataAnnotations;

namespace Dictionary.Models
{
    public class Definition
    {
        [Key]
        public Int64 Id { get; set; }
        [Required]
        public string EnglishWord { get; set; }
        public string PartOfSpeech { get; set; }
        [Required]
        public string MalayalamDefinition { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}