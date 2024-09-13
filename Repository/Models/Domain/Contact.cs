using System;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models.Domain
{
    public class Contact
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public bool Married { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        public decimal Salary { get; set; }
    }
}
