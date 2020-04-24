using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using CS.Db.Context;
using CS.Service.RestApiNode;
using CS.Service.RestApiNode.Models;
using CS.WebApi.Controllers;
using CS.WebApi.Infrasructure;
using CS.WebApi.Infrasructure.AttributeFilter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NodeAPIClient.Services;

namespace CS.WebApi.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ApplicationController
    {

        public ContractController(IServiceProvider provider) : base(provider) { }

        //public string Index()
        //{
        //    using (var context = ServiceProvider.GetService<AppDbContext>())
        //    {

        //        context.Database.Migrate();
        //        var t = context.RestUser.FirstOrDefault(p => p.ID == 12);
        //        return t.Name;
        //    }

        //    return String.Empty;
        //}

        /// <summary>
        /// Получаем 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[AuthKeyFilter]
        //[HttpPost]
        //public IActionResult GetData(RequestApiModel model)
        //{
        //    // Нужна проверка валидности AuthKey - guid 

        //    InitAuthKey(model);
        //    ResponseApiModel res = ServiceProvider
        //        .GetService<MonitorService>().GetBalance(model);

        //    res.Success = true;

        //    //TODO: запись в базу, для логов

        //    return Ok(res);//"value";
        //}



        [AuthKeyFilter]
        [HttpPost("GetContract")]
        public ActionResult<ResponseApiModel> GetContract(RequestKeyApiModel model)
        {
            InitAuthKey(model);
            ResponseApiModel res;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetContract(model);

                res.Success = true;
            }
            catch (Exception ex)
            {
                res = new ResponseApiModel();
                res.Success = false;
                res.Message = ex.Message;
            }
            
            return Ok(res);
        }


        //[AuthKeyFilter]
        //[HttpPost("GetWalletTransactions")]
        //public ActionResult<ResponseApiModel> GetWalletTransactions(RequestKeyApiModel model)
        //{
        //    InitAuthKey(model);
        //    ResponseApiModel res;
        //    try
        //    {
        //        res = ServiceProvider.GetService<MonitorService>().GetWalletTransactions(model);

        //        res.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        res = new ResponseApiModel();
        //        res.Success = false;
        //        res.Message = ex.Message;
        //    }

        //    return Ok(res);
        //}

        [AuthKeyFilter]
        [HttpPost("GetFromTransaction")]
        public ActionResult<TransactionInfo> GetContractFromTransaction(RequestGetterApiModel model)
        {
            InitAuthKey(model);
            SmartSourceCode ret;
            TransactionInfo res;
            try
            {
                ret = new SmartSourceCode();
                res = ServiceProvider.GetService<MonitorService>().GetTransaction(model);
                if (res.Bundle!=null && res.Bundle.Contract.Deploy != null)
                {
                    ret.SourceString = res.Bundle.Contract.Deploy.SourceCode;
                    if (model.Compressed)
                    {
                        ret.gZipped = Utils.Compress(res.Bundle.Contract.Deploy.SourceCode);
                    }
                }

                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret = new SmartSourceCode();
                ret.Success = false;
                ret.Message = ex.Message;
            }

            return Ok(ret);
        }


        [AuthKeyFilter]
        [HttpPost("GetByAddress")]
        public ActionResult<ResponseApiModel> GetContractByAddress(RequestKeyApiModel model)
        {
            InitAuthKey(model);

            SmartSourceCode res;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetContractByAddress(model);
                if(res.SourceString != null/* && model.CompressString*/)
                {
                    if (model.Compressed)
                    {
                        res.gZipped = Utils.Compress(res.SourceString);
                    }
                    res.SourceString = res.SourceString;
                }
                res.Success = true;
            }
            catch (Exception ex)
            {
                res = new SmartSourceCode();
                res.Success = false;
                res.Message = ex.Message;
            }

            return Ok(res);
        }


        [AuthKeyFilter]
        [HttpPost("GetMethods")]
        public ActionResult<SmartContractMethodsModel> GetContractMethods(RequestKeyApiModel model)
        {
            InitAuthKey(model);

            SmartContractMethodsModel res;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetContractMethods(model);

                res.Success = true;
            }
            catch (Exception ex)
            {
                res = new SmartContractMethodsModel();
                res.Success = false;
                res.Message = ex.Message;
            }

            return Ok(res);
        }

        [AuthKeyFilter]
        [HttpPost("Validate")]
        public ActionResult<ContractValidationResponse> ContractValidation(RequestContractValidationModel model)
        {
            InitAuthKey(model);

            ContractValidationResponse res;
            try
            {

                res = ServiceProvider.GetService<MonitorService>().ValidateContract(model);
                res.Success = true;
            }
            catch (Exception ex)
            {
                res = new ContractValidationResponse();
                res.Success = false;
                res.Message = ex.Message;
            }

            return Ok(res);
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
