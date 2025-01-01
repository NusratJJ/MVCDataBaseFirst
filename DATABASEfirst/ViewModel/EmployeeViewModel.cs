using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DATABASEfirst.ViewModel
{
    public class EmployeeViewModel
    {
        public EmployeeViewModel()
        {
            this.SkillList = new List<int>();
        }
        public int EmployeeID { get; set; }
        [Required(ErrorMessage = "Please enter employee Last name")]
        [Display(Name = "First Name"), StringLength(30)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter employee Last name")]
        [Display(Name = "Last Name"), StringLength(30)]
        public string lastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Date Of Birth")]
        [ValadateDate]
        public System.DateTime DateOfBirth { get; set; }
        [Range(15000, 50000)]
        public decimal Salary { get; set; }
        [Display(Name = "Active ?")]
        public bool IsActive { get; set; }
        [Display(Name = "Profile Picure")]
        public string Picture { get; set; }
        [Display(Name = "Mobile Number")]
        [StringLength(11, MinimumLength = 11)]
        public string MobileNo { get; set; }

        public String FullName
        {
            get { return FirstName + " " + lastName; }
        }

        public HttpPostedFileBase PicturePath { get; set; }
        public List<int> SkillList { get; set; }
    }
}