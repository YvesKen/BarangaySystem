using System;
using System.ComponentModel.DataAnnotations;

namespace BarangaySystem.Models
{
    public class Clearance
    {
        [Key]
        public int ClearanceID { get; set; }

        public string ControlNumber { get; set; } = string.Empty;

        [Required]
        public int ResidentID { get; set; }

        [Required]
        public string DocumentType { get; set; } = "Barangay Clearance";

        [Required]
        public string Purpose { get; set; } = "For Employment";

        public string ORNumber { get; set; } = string.Empty;

        public decimal AmountPaid { get; set; } = 50.00m;

        public string IssuedBy { get; set; } = "Admin";

        public DateTime DateIssued { get; set; } = DateTime.Now;
    }
}