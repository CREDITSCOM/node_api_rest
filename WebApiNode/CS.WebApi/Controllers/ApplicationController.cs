using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS.Service.RestApiNode.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace CS.WebApi.Controllers
{
  
    public abstract class ApplicationController : ControllerBase
    {
        public IServiceProvider ServiceProvider { get; }
        public ApplicationController(IServiceProvider provider) : base()
        {
            ServiceProvider = provider;
        }

        //[NonAction]
        //public void InitAuthKey(RequestApiModel model)
        //{
        //    StringValues authKey = StringValues.Empty;
        //    var request = HttpContext.Request;
        //    request.Headers.TryGetValue("AuthKey", out authKey);
        //    if (authKey.Count > 0)
        //    {
        //        var key = authKey.FirstOrDefault();
        //        model.AuthKey = key;
        //    }

        //}

        //[NonAction]
        //public void InitAuthKey(RequestGetterApiModel model)
        //{
        //    StringValues authKey = StringValues.Empty;
        //    var request = HttpContext.Request;
        //    request.Headers.TryGetValue("AuthKey", out authKey);
        //    if (authKey.Count > 0)
        //    {
        //        var key = authKey.FirstOrDefault();
        //        model.AuthKey = key;
        //    }

        //}

        [NonAction]
        public void InitAuthKey(AbstractRequestApiModel model)
        {
            StringValues authKey = StringValues.Empty;
            var request = HttpContext.Request;
            request.Headers.TryGetValue("AuthKey", out authKey);
            if (authKey.Count > 0)
            {
                var key = authKey.FirstOrDefault();
                model.AuthKey = key;
            }

        }

    }
}