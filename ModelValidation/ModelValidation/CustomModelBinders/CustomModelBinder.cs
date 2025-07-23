using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using ModelValidation.Models;

namespace ModelValidation.CustomModelBinders
{
    public class CustomModelBinder : IModelBinder
    {
        private readonly ILogger<CustomModelBinder> _logger;

        public CustomModelBinder(ILogger<CustomModelBinder> logger)
        {
            _logger = logger;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // First Name and Last Name = Person Name
            Person person = new Person();

            if (bindingContext.ValueProvider.GetValue("FirstName").Length > 0)
            {
               person.PersonName = bindingContext.ValueProvider.GetValue("FirstName").FirstValue;
            }

            if (bindingContext.ValueProvider.GetValue("LastName").Length > 0)
            {
                person.PersonName += " " + bindingContext.ValueProvider.GetValue("LastName").FirstValue;
            }

            //Email
            if (bindingContext.ValueProvider.GetValue("Email").Length > 0)
            {
                person.Email = bindingContext.ValueProvider.GetValue("Email").FirstValue;
            }

            //Password
            if (bindingContext.ValueProvider.GetValue("Password").Length > 0)
            {
                person.Password = bindingContext.ValueProvider.GetValue("Password").FirstValue;
            }

            //Confirm Password
            if (bindingContext.ValueProvider.GetValue("ConfirmPassword").Length > 0)
            {
                person.ConfirmPassword = bindingContext.ValueProvider.GetValue("ConfirmPassword").FirstValue;
            }

            //Phone
            if (bindingContext.ValueProvider.GetValue("Phone").Length > 0)
            {
                person.Phone = bindingContext.ValueProvider.GetValue("Phone").FirstValue;
            }

            //Date of Birth
            if (bindingContext.ValueProvider.GetValue("DateOfBirth").Length > 0)
            {
                if (DateTime.TryParse(bindingContext.ValueProvider.GetValue("DateOfBirth").FirstValue, out DateTime dateOfBirth))
                {
                    person.DateOfBirth = dateOfBirth;
                }
            }

            //Price
            if (bindingContext.ValueProvider.GetValue("price").Length > 0)
            {
                if (double.TryParse(bindingContext.ValueProvider.GetValue("price").FirstValue, out double price))
                {
                    person.price = price;
                }
            }

            //Present Age
            if (bindingContext.ValueProvider.GetValue("PresentAge").Length > 0)
            {
                if (int.TryParse(bindingContext.ValueProvider.GetValue("PresentAge").FirstValue, out int presentAge))
                {
                    person.PresentAge = presentAge;
                }
            }

            //FromDate
            if (bindingContext.ValueProvider.GetValue("FromDate").Length > 0)
            {
                if (DateTime.TryParse(bindingContext.ValueProvider.GetValue("FromDate").FirstValue, out DateTime fromDate))
                {
                    person.FromDate = fromDate;
                }
            }

            //ToDate
            if (bindingContext.ValueProvider.GetValue("ToDate").Length > 0)
            {
                if (DateTime.TryParse(bindingContext.ValueProvider.GetValue("ToDate").FirstValue, out DateTime toDate))
                {
                    person.ToDate = toDate;
                }
            }

            //_logger.LogInformation("Tags count: {Count}", bindingContext.ValueProvider.GetValue("Tags[0]").Length);

            for(int i=0; ;i++)
            {
                string tg = "Tags[" + i.ToString() + "]";
                if(bindingContext.ValueProvider.GetValue(tg).Length>0)
                {
                    person.Tags.Add(bindingContext.ValueProvider.GetValue(tg).FirstValue);
                }
                else
                {
                    break; // Exit the loop if no more tags are found
                }
            }



            bindingContext.Result = ModelBindingResult.Success(person);
            
            return Task.CompletedTask;
        }
    }
}
