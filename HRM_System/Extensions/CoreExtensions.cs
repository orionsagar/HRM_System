using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UKHRM.Extensions
{
    public static class CoreExtensions
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controller, string action)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeAction = routeData.Values["action"].ToString().ToLower();
            var routeController = routeData.Values["controller"].ToString().ToLower();


            var returnActive = (controller.ToLower() == routeController.ToLower() && (action.ToLower() == routeAction.ToLower() || routeAction == "Details"));

            return returnActive ? "active" : "";
        }

        public static string ActiveClass(this IHtmlHelper htmlHelper, string controllers = null, string actions = null, string cssClass = "active pcoded-trigger")
        {
            var currentController = htmlHelper?.ViewContext.RouteData.Values["controller"] as string;
            var currentAction = htmlHelper?.ViewContext.RouteData.Values["action"] as string;

            var acceptedControllers = (controllers.ToLower() ?? currentController.ToLower() ?? "").Split(',');
            var acceptedActions = (actions.ToLower() ?? currentAction.ToLower() ?? "").Split(',');

            return acceptedControllers.Contains(currentController.ToLower()) && acceptedActions.Contains(currentAction.ToLower())
                ? cssClass
                : "";
        }
    }
}
