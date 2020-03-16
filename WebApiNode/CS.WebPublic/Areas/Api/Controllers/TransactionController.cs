using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS.Service.RestApiNode;
using CS.Service.RestApiNode.Models;
using CS.WebPublic.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CS.WebPublic.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : AbstractApiController
    {
        public TransactionController(IServiceProvider provider) : base(provider) { }



        [HttpPost("Pack")]
        public IActionResult Pack(RequestApiModel model)
        {
             ResponseApiModel res = new ResponseApiModel();


            res.DataResponse.TransactionPackagedStr = ServiceProvider
                .GetService<TransactionService>()
                .PackTransactionByApiModel(model);
            res.Success = true;

            return new JsonResult(res);//"value";
        }

        [HttpPost("Execute")]
        public IActionResult Execute(RequestApiModel model)
        {
            var res = ServiceProvider
                .GetService<TransactionService>()
                .ExecuteTransaction(model);

            res.FlowResult = null;
            return new JsonResult(res);//"value";
        }

    }
}