using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;

namespace ModelValidation.CustomValidators
{
    public class DateRangeValidatorAttribute : ValidationAttribute
    {
       public string FromDatePropertyName { get; set; }
        public DateRangeValidatorAttribute(string fromDatePropertyName)
        {
            FromDatePropertyName = fromDatePropertyName;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if(value != null)
            {
                DateTime? toDate = Convert.ToDateTime(value);
                PropertyInfo? fromDateProperty = validationContext.ObjectType.GetProperty(FromDatePropertyName);
               
                if(fromDateProperty == null)
                {
                    return new ValidationResult($"Property '{FromDatePropertyName}' not found on {validationContext.ObjectType.Name}");
                }
                DateTime? fromDate = Convert.ToDateTime(fromDateProperty.GetValue(validationContext.ObjectInstance));
                


                if(fromDate > toDate)
                {
                    //return new ValidationResult("FromDate should be older or equal to ToDate");
                    // or
                    return new ValidationResult(ErrorMessage ?? $"{FromDatePropertyName} should be older or equal to {validationContext.MemberName}", new string[] {FromDatePropertyName,validationContext.MemberName}); // ErrorMessage is optional, if not provided it will use the default message. Will be shown in both FromDate and ToDate properties
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            return null; // If the value is null, no validation error
        }
    }
}
