using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NHSDigitalExclusionWebsite.Models
{
    [Table("patients")]
    public class Patient
    {
        [Key]
        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("nhs_number")]
        public string NhsNumber { get; set; } = "";

        [Column("first_name")]
        public string FirstName { get; set; } = "";

        [Column("last_name")]
        public string LastName { get; set; } = "";

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Column("gender")]
        public string Gender { get; set; } = "";

        [Column("phone")]
        public string Phone { get; set; } = "";

        [Column("email")]
        public string Email { get; set; } = "";

        [Column("postcode")]
        public string Postcode { get; set; } = "";

        [Column("city")]
        public string City { get; set; } = "";
    }
}