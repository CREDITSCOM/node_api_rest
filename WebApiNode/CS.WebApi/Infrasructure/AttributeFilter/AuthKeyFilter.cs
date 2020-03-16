using CS.Service.RestApiNode;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.WebApi.Infrasructure.AttributeFilter
{
    public class AuthKeyFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            StringValues authKey = StringValues.Empty;
             var request = context.HttpContext.Request;
            request.Headers.TryGetValue("AuthKey", out authKey);
            if (authKey.Count > 0)
            {
                var key = authKey.FirstOrDefault();

                var data = AuthDataService.ListAuthKey.FirstOrDefault(p => p.AuthKey == key && p.IsActive);


                if (data == null)
                    throw new Exception("AuthKey was not found!");       
            }


        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }


    }
}
