using System;
using CS.Service.RestApiNode;
using CS.Service.RestApiNode.Models;
using CS.WebPublic.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CS.WebPublic.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitorController : AbstractApiController
    {
        public MonitorController(IServiceProvider provider) : base(provider) { }

        [HttpPost("GetBalance")]
        public ActionResult<ResponseApiModel> GetBalance(RequestApiModel model)
        {
            BalanceResponseApiModel res;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetBalance(model);
                res.Success = true;
            }
            catch (Exception ex)
            {
                res = new BalanceResponseApiModel();
                res.Success = false;
                res.Message = ex.Message;
            }

            return Ok(res);
        }

        [HttpPost("GetContract")]
        public IActionResult GetContract(RequestGetterApiModel model)
        {
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

        [HttpPost("GetTransactionInfo")]
        public ActionResult<TransactionInfo> GetTransaction(RequestGetterApiModel model)
        {
            TransactionInfo res;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetTransaction(model);
                res.Success = true;
            }
            catch (Exception ex)
            {
                res = new TransactionInfo();
                res.Success = false;
                res.Message = ex.Message;
            }

            return Ok(res);
        }

        [HttpPost("GetListTransactionByBlock")]
        public ActionResult<ResponseApiModel> GetListTransactionByBlockId(RequestGetterApiModel model)
        {
            ResponseApiModel res;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetListTransactionByBlockId(model);

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

        [HttpPost("GetListTransactionByLastBlock")]
        public ActionResult<ResponseApiModel> GetListTransactionByLastBlock(RequestGetterApiModel model)
        {
            ResponseApiModel res;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetLastBlockId(model);

                model.BlockId = res.BlockId;
                res = ServiceProvider.GetService<MonitorService>().GetListTransactionByBlockId(model);


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

        [HttpPost("GetLastBlockId")]
        public ActionResult<ResponseApiModel> GetLastBlockId(RequestGetterApiModel model)
        {
            ResponseApiModel res;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetLastBlockId(model);

                model.BlockId = res.BlockId;
                res = ServiceProvider.GetService<MonitorService>().GetListTransactionByBlockId(model);


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

        //[HttpPost("GetTransactionByInnerId")]
        //public IActionResult GetTransactionByInnerId(RequestGetterApiModel model)
        //{
        //    ResponseApiModel res;
        //    try
        //    {
        //        res = null;
        //        res.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        res = new ResponseApiModel();
        //        res.Success = false;
        //        res.MessageError = ex.Message;
        //    }

        //    return Ok(res);
        //}
    }
}