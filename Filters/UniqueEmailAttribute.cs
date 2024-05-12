using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagmentRoleBased.Models;

namespace TaskManagmentRoleBased.Filters
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string emailAddress = value.ToString();
                var dbContext = (office_sidpd_newEntities)validationContext.GetService(typeof(office_sidpd_newEntities));

                // Check if the email address exists in the database
                bool isUnique = !dbContext.Employees.Any(u => u.Email == emailAddress);

                if (!isUnique)
                {
                    return new ValidationResult("Email address already in use.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
