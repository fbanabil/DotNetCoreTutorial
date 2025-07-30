using Microsoft.AspNetCore.Mvc;
using ViewComponentExample.Models;

namespace ViewComponentExample.ViewComponents
{
    [ViewComponent]
    public class GridViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PersonGridModel grid)
        {
            PersonGridModel model = new PersonGridModel
            {
                GridTitle = "People List",
                People = new List<Person>
                {
                    new Person { PersonName = "Alice", JobTitle = "Software Engineer" },
                    new Person { PersonName = "Bob", JobTitle = "Project Manager" },
                    new Person { PersonName = "Charlie", JobTitle = "UX Designer" }
                }
            };
            PersonGridModel model1 = grid;
            ViewData["Grid"] = model; 
            return View(model1);
        }
    }
}
