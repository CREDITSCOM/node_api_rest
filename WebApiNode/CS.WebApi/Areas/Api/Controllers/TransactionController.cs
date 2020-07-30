using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS.Service.RestApiNode;
using CS.Service.RestApiNode.Models;
using CS.WebApi.Controllers;
using CS.WebApi.Infrasructure;
using CS.WebApi.Infrasructure.AttributeFilter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CS.WebApi.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ApplicationController
    {

        public TransactionController(IServiceProvider provider) : base(provider) { }
        [AuthKeyFilter]
        [HttpPost("Pack")]
        // [AuthKey]
        /// <summary>
        /// Упаковка транзакции 
        ///  Base58Check
        /// .Base58CheckEncoding
        /// .EncodePlain(BCTransactionTools.CreateByteHashByTransaction(transac))
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public IActionResult Pack(RequestApiModel model)
        {
            // Нужна проверка валидности AuthKey - guid 

            InitAuthKey(model);
            ResponseApiModel res = new ResponseApiModel();


            if (model.DelegateDisable || model.DelegateEnable || model.DateExpired!=null)
            {
                res.DataResponse.TransactionPackagedStr = ServiceProvider
                .GetService<TransactionService>().PackTransactionByApiModelDelegation(model);
            }
            else
            {
                res.DataResponse = ServiceProvider
                .GetService<TransactionService>().PackTransactionByApiModel(model);
            }

            res.Success = true;

            //TODO: запись в базу, для логов. Обработка ошибок. Согласовать стандарт.

            return new JsonResult(res);//"value";
        }

        [AuthKeyFilter]
        [HttpPost("Execute")]
       // [AuthKey]
        /// <summary>
        /// Выполнение транзакции. Которая предварительно уже была упакована отправлена
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult Execute(RequestApiModel model)
        {
            // Нужна проверка валидности AuthKey - guid 

            InitAuthKey(model);


            var res = ServiceProvider.GetService<TransactionService>()
                .ExecuteTransaction(model);



            //TODO: запись в базу, для логов. Обработка ошибок. Согласовать стандарт.

            
            res.FlowResult = null;
            return new JsonResult(res);//"value";
        }

    }
}
