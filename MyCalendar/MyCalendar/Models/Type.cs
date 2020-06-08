using System.ComponentModel.DataAnnotations;

namespace MyCalendar.Models
{
    public class Type
    {
        public byte Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
