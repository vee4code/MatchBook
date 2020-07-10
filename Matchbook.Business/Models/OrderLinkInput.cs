using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Matchbook.Business.Models
{
    

    public class OrderLinkInput
    {
        public List<int> OrderIds { get; set; }

        [CustomUniqueValidator]
        public string Name { get; set; }
    }  

    public class CustomUniqueValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                bool isUnique = new LinkOrder().IsUnique(value as string);
                if (isUnique == false)
                    return new ValidationResult("Link name must be unique.");
                else
                    return ValidationResult.Success;

            }
            return new ValidationResult("Link name is required");
        }
    }
}
