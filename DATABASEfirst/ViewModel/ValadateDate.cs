using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DATABASEfirst.ViewModel
{
    public class ValadateDate: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime CurrentDate = DateTime.Now;
            string message = "";
            if (Convert.ToDateTime(value) < CurrentDate.AddYears(-21))
            {
                return ValidationResult.Success;
            }
            else
            {
                message = "Age must be atlest 21 years";
                return new ValidationResult(message);
            }
        }
    }
}