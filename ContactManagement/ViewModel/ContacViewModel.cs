using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactManagement.ViewModel
{
    public class ContactAddEditViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(160)]
        public string FirstName { get; set; }
        [StringLength(160)]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public string Mobile { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [Display(Name = "Upload")]
        public HttpPostedFileBase PhotoUpload { get; set; }
        [Display(Name = "Photo")]
        public string PhotoPath { get; set; }

     
    }

    public class ContactListViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [Display(Name = "Photo")]
        public string PhotoPath { get; set; }
    }

  }