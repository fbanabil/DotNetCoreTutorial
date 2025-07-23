using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ModelValidation.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace ModelValidation.Models
{
    public class Person : IValidatableObject // Implementing IValidatableObject for custom validation logic
    {
        [Required(ErrorMessage ="{0} is Needed")] // With argument {0}=PersonName
        [Display(Name = "Person Name")]            // Display attribute to customize the label
        [StringLength(20,MinimumLength =5,ErrorMessage = "{0} must be between {2} and {1} characters long.")]
        [RegularExpression("^[A-Za-z .]*$",ErrorMessage ="{0} should contain alphabte space and dot")]
        public string? PersonName { get; set; }


        [Required(ErrorMessage ="{0} is Needed")]
        [EmailAddress(ErrorMessage ="{0} is not a valid email address")]
        public string? Email { get; set; }
        
        
        [Phone(ErrorMessage ="{0} is not a valid phone number")]
        // [ValidateNever] // This attribute is used to skip validation for this property
        public string? Phone { get; set; }

        
        
        [Required(ErrorMessage ="{0} can't be blank")]
        public string? Password { get; set; }
        
        
        [Required(ErrorMessage = "{0} can't be blank")]
        [Compare("Password",ErrorMessage ="{0} should match with Password")]
        [Display(Name ="Confirm Password")]
        public string? ConfirmPassword { get; set; }


        [Range(0,999.99,ErrorMessage ="{0} should be between {1} and {2}")]
        [Display(Name = "Price")]
        public double? price { get; set; }

        //[MinimumYearValidator(ErrorMessage ="Checking Error")] // Custom Validator without argument
        [MinimumYearValidator(2000, ErrorMessage = "Date of Birth should be before or equal {0}")]

        //[BindNever] // This attribute is used to skip Model Binding for this property
        public DateTime? DateOfBirth { get; set; }



        public DateTime? FromDate { get; set; }


        [DateRangeValidator("FromDate",ErrorMessage="Fromdate should be older or equal to ToDate")]
        public DateTime? ToDate { get; set; }




        public int? PresentAge { get; set; }



        public List<string?> Tags { get; set; }  = new List<string?>(); 


        // Override ToString() method to display the object in a readable format
        public override string ToString()
        {
            string model= $"Name: {PersonName}, Email: {Email}, Phone: {Phone}, Password: {Password}, Confirm_Password: {ConfirmPassword}, Price: {price}, DateOfBirth: {DateOfBirth}, FromDate: {FromDate}, ToDate: {ToDate}";
            //add Tags to the model string
            if (Tags != null && Tags.Count > 0)
            {
                model += $", Tags: {string.Join(", ", Tags)}";
            }
            else
            {
                model += ", Tags: None";
            }
            return model;
        }



        // Implementing IValidatableObject for custom validation logic
        // Only age or date of birth is required, not both
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) // It executes after all the other validations are done
        {
            if (PresentAge.HasValue == false && DateOfBirth.HasValue == false)
            {
                // Yield allows returning multiple ValidationResult instances
                yield return new ValidationResult("Either of date  of birth ore age should be supplied", new[] {nameof(PresentAge) });
            }
            // yield combines return and make it ienumerable

            //if(...){
            // yield return new ValidationResult("Error message", new[] { "PropertyName" });
            //}
        }
    }
}
