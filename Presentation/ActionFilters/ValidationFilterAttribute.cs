using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        // method çalışmadan hemen önce
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"]; // hangi controllerdayım
            var action = context.RouteData.Values["action"]; // hangi method çalışacak

            // dto param yakala
            var param = context.ActionArguments.SingleOrDefault(p=>p.Value.ToString().Contains("Dto")).Value; // Dto lu parametrenin değerini ver

            if (param is null) { 
                context.Result = new BadRequestObjectResult( //400
                    $"object is null.=>" + 
                    $"Controller:{controller}=>" + 
                    $"Action:{action}"); return;
            }

            if (!context.ModelState.IsValid)
                context.Result = new UnprocessableEntityObjectResult(context.ModelState); //422
            
        }
    }
}
