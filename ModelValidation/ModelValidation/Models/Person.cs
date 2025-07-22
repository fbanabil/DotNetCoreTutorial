using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ModelValidation.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace ModelValidation.Models
{
    public class Person
    {
        [Required(ErrorMessage ="{0} is Needed")] // With argument {0}=PersonName
        [Display(Name = "Person Name")]            // Display attribute to customize the label
        [StringLength(20,MinimumLength =5,ErrorMessage = "{0} must be between {2} and {1} characters long.")]
        [RegularExpression("^[A-Za-z .]+$",ErrorMessage ="{0} should contain alphabte space and dot")]
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
        public DateTime? DateOfBirth { get; set; }


        public override string ToString()
        {
            return $"Name: {PersonName}, Email: {Email}, Phone: {Phone}, Password: {Password}, Confirm_Password: {ConfirmPassword}, Price: {price}";
        }
    }
}
