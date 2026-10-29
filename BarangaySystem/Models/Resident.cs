using System;
using System.ComponentModel.DataAnnotations;

namespace BarangaySystem.Models
{
    public class Resident
    {
        [Key]
        public int ResidentID { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; } = string.Empty;

        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; } = string.Empty;

        public string? Suffix { get; set; }
        public string Gender { get; set; } = "Male";

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; } = DateTime.Now.AddYears(-18);

        public string CivilStatus { get; set; } = "Single";
        public string AddressLine { get; set; } = string.Empty;

        [Required(ErrorMessage = "Purok selection is required")]
        public string Purok { get; set; } = string.Empty;

        public string? ContactNumber { get; set; }
        public string VoterStatus { get; set; } = "Registered Voter";

        public bool IsSenior { get; set; }
        public bool IsPWD { get; set; }
        public bool IsSoloParent { get; set; }
        public bool Is4Ps { get; set; }

        public DateTime DateRegistered { get; set; } = DateTime.Now;
        public string FullName => $"{FirstName} {MiddleName} {LastName} {Suffix}".Trim();
    }
}