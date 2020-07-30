using CS.Service.RestApiNode.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

namespace CS.WebApi.Areas.Api.Controllers
{
    public class ApiBaseController : ControllerBase
    {
        protected ActionResult ReturnErrorResult(string message)
        {
            var apiBehaviorOptions = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

            ModelState.AddModelError("api", message);
            return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext) as ActionResult;
        }

        protected ActionResult ReturnErrorResult(Exception ex)
        {
            return ReturnErrorResult(ex.Message);
        }

        [NonAction]
        public void InitAuthKey(AbstractRequestApiModel model)
        {
            var request = HttpContext.Request;
            StringValues authKey;
            request.Headers.TryGetValue("AuthKey", out authKey);
            if (authKey.Count > 0)
            {
                var key = authKey.FirstOrDefault();
                model.AuthKey = key;
            }
        }
    }
}
