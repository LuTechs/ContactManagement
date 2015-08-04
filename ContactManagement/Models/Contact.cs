using System;
using System.ComponentModel.DataAnnotations;

namespace ContactManagement.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [StringLength(160)]
        public string FirstName { get; set; }
        [StringLength(160)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(250)]
        public string PhotoPath { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }


    }
}