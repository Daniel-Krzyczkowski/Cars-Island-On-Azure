using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace CarsIsland.API.DtoValidation.Filters
{
    public class DtoValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var modelValidationErrors = context.ModelState
                        .Values
                        .SelectMany(v => v.Errors.Select(b => b.ErrorMessage))
                        .ToList();
                var dtoFailedValidationResult = new DtoFailedValidationResult(modelValidationErrors);
                context.Result = new JsonResult(dtoFailedValidationResult) { StatusCode = 400 };
            }
        }
    }
}
