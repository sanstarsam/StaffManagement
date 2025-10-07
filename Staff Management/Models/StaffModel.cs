using System.ComponentModel.DataAnnotations;

namespace Staff_Management.Models
{
    public class StaffModel
    {
        [Required]
        [StringLength(8)]
        public string StaffId { get; set; } = "";
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = "";
        [Required]
        public DateOnly Birthday { get; set; }
        [Required]
        public int Gender { get; set; } // 1 = Male, 2 = Female
    }

    public class StaffFilter
    {
        public string? StaffId { get; set; }
        public int? Gender { get; set; } // 1 = Male, 2 = Female
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Export { get; set; } = "";
    }
}
