using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using ModelValidation.Models;

namespace ModelValidation.CustomModelBinders
{
    public class CustomBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        { 
            if(context.Metadata.ModelType == typeof(Person))
            {
                // If the model type is Person, return the custom binder
                //return new CustomModelBinder();
                //or

                return new BinderTypeModelBinder(typeof(CustomModelBinder));
            }
            else
            {
                // Otherwise, return null to use the default binder
                return null;
            }
        }
    }
}
