using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refresher.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }



        [Required]
        public string? Name { get; set; }



        [Required]
        [Display(Name = "Street Address")]
        public string? StreetAddress { get; set; }



        [Required]
        public string? City { get; set; }



        [Required]
        public string? State { get; set; }


        [Required]
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }


        [Required]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
    }
}
