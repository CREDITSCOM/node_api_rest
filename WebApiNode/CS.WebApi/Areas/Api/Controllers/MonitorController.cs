using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

namespace CS.WebApi.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitorController : ApplicationController
    {

        public MonitorController(IServiceProvider provider) : base(provider) { }

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
        [HttpPost("GetBalance")]
        public ActionResult<ResponseApiModel> GetBalance(RequestApiModel model)
        {
            InitAuthKey(model);
            ResponseApiModel res;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetBalance(model);

                res.Success = true;
            }
            catch (Exception ex)
            {
                res = new ResponseApiModel();
                res.Success = false;
                res.MessageError = ex.Message;
            }

            return Ok(res);
        }

        [AuthKeyFilter]
        [HttpPost("GetWalletData")]
        public ActionResult<ResponseApiModel> GetWalletData(RequestApiModel model)
        {
            InitAuthKey(model);
            WalletDataResponseApiModel res;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetWalletData(model);

                res.Success = true;
            }
            catch (Exception ex)
            {
                res = new WalletDataResponseApiModel();
                res.Success = false;
                res.MessageError = ex.Message;
            }

            return Ok(res);
        }

        [AuthKeyFilter]
        [HttpPost("GetTransaction")]
        public ActionResult<ResponseApiModel> GetTransaction(RequestGetterApiModel model)
        {
            InitAuthKey(model);
            ResponseApiModel res;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetTransaction(model);

                res.Success = true;
            }
            catch (Exception ex)
            {
                res = new ResponseApiModel();
                res.Success = false;
                res.MessageError = ex.Message;
            }

            return Ok(res);
        }

        [AuthKeyFilter]
        [HttpPost("GetListTransactionByBlock")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult<ResponseApiModel> GetListTransactionByBlockId(RequestGetterApiModel model)
        {
            InitAuthKey(model);
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
                res.MessageError = ex.Message;
            }

            return Ok(res);
        }


        [AuthKeyFilter]
        [HttpPost("GetListTransactionByLastBlock")]
        public ActionResult<ResponseApiModel> GetListTransactionByLastBlock(RequestGetterApiModel model)
        {
            InitAuthKey(model);
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
                res.MessageError = ex.Message;
            }

            return Ok(res);
        }

        [AuthKeyFilter]
        [HttpPost("GetLastBlockId")]
        public ActionResult<ResponseApiModel> GetLastBlockId(RequestGetterApiModel model)
        {
            InitAuthKey(model);
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
                res.MessageError = ex.Message;
            }

            return Ok(res);
        }


        [AuthKeyFilter]
        [HttpPost("GetTransactionByInnerId")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">Required params: model.InnerId, model.PublickKey</param>
        /// <returns></returns>
        public ActionResult<ResponseApiModel> GetTransactionByInnerId(RequestGetterApiModel model)
        {
            InitAuthKey(model);
            ResponseApiModel res;
            try
            {
                res = null;

                res.Success = true;
            }
            catch (Exception ex)
            {
                res = new ResponseApiModel();
                res.Success = false;
                res.MessageError = ex.Message;
            }

            return Ok(res);
        }

        [AuthKeyFilter]
        [HttpPost("GetBlocks")]
        public ActionResult<ResponseBlocksModel> GetBlocks(RequestBlocksModel request)
        {
            InitAuthKey(request);
            string json;
            try
            {
                json = ServiceProvider.GetService<BlocksService>().GetBlocksRange(request);
            }
            catch (Exception ex)
            {
                var res = new ResponseBlocksModel();
                res.Success = false;
                res.Message = ex.Message;
                return Ok(res);
            }

            return new JsonResult(json); //(res.Blocks); ;
        }

        [AuthKeyFilter]
        [HttpPost("GetNodeInfo")]
        public ActionResult<ResponseNodeInfoModel> GetNodeInfo(RequestNodeInfoModel request)
        {
            InitAuthKey(request);
            if (!string.IsNullOrWhiteSpace(request.Ip))
            {
                ushort port = 0;
                if(!ushort.TryParse(request.Port, out port))
                {
                    port = RequestNodeInfoModel.DefaultPort;
                }
                try
                {
                    // fixed timeout, ignore config settings for specific network
                    var result = ServiceProvider.GetService<NodeAPIClient.Services.NodeInfoService>().GetNodeInfo(request.Ip, port, 60000);
                    if(result != null)
                    {
                        return Ok(result);
                    }
                    return Ok(new ResponseNodeInfoModel()
                    {
                        Success = false,
                        Message = "failed to get requested info, check the remote node is alive"
                    });
                }
                catch(Exception x)
                {
                    return Ok(new ResponseNodeInfoModel()
                    {
                        Success = false,
                        Message = x.Message
                    });
                }
            }
            return Ok(new ResponseNodeInfoModel()
            {
                Success = false,
                Message = "Ip parameter not set correctly in request"
            });
        }
    }
}
