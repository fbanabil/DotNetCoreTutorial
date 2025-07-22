using System.ComponentModel.DataAnnotations;

namespace ModelValidation.CustomValidators
{
    public class MinimumYearValidatorAttribute : ValidationAttribute
    {
        public int MinumiumYear { get; set; } = 2000; // Default minimum year

        public MinimumYearValidatorAttribute()
        {
            
        }

        public MinimumYearValidatorAttribute(int minimumYear)
        {
            MinumiumYear= minimumYear;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value!=null)
            {
                DateTime date=(DateTime)value;

                if(date.Year > MinumiumYear) // For no argument case MinimumYear must be 2000 or any other int
                {
                    //return new ValidationResult("Year before or equal to 2000 is a must");
                    // or
                    //return new ValidationResult(ErrorMessage);

                    // or for {} to work
                    return new ValidationResult(string.Format(ErrorMessage, MinumiumYear));
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
