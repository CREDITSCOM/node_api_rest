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
        [HttpPost("GetContract")]
        public ActionResult<ResponseApiModel> GetContract(RequestGetterApiModel model)
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

        [AuthKeyFilter]
        [HttpPost("GetTransactionInfo")]
        public ActionResult<ResponseApiModel> GetTransactionInfo(RequestGetterApiModel model)
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
                res.Message = ex.Message;
            }

            return Ok(res);
        }

        [AuthKeyFilter]
        [HttpPost("GetWalletTransactions")]
        public ActionResult<ResponseApiModel> GetWalletTransactions(AbstractRequestApiModel model)
        {
            InitAuthKey(model);
            ResponseApiModel res;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetWalletTransactions(model);

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

        [AuthKeyFilter]
        [HttpPost("GetContractFromTransaction")]
        public ActionResult<ResponseApiModel> GetContractFromTransaction(RequestGetterApiModel model)
        {
            InitAuthKey(model);
            SmartSourceCode ret;
            ResponseApiModel res;
            try
            {
                ret = new SmartSourceCode();
                res = ServiceProvider.GetService<MonitorService>().GetTransaction(model);
                if (res.TransactionInfo.Bundle!=null && res.TransactionInfo.Bundle.Contract.Deploy != null)
                {

                }
                ret.sourceString = res.TransactionInfo.Bundle.Contract.Deploy.SourceCode;
                ret.gZipped = Utils.Compress(res.TransactionInfo.Bundle.Contract.Deploy.SourceCode);
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
        [HttpPost("GetContractByAddress")]
        public ActionResult<ResponseApiModel> GetContractByAddress(RequestApiModel model)
        {
            InitAuthKey(model);

            SmartSourceCode res;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetContractByAddress(model);
                if(res.sourceString != null/* && model.CompressString*/)
                {
                    res.gZipped = Utils.Compress(res.sourceString);
                    res.sourceString = "";
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
                res.Message = ex.Message;
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
                res.Message = ex.Message;
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
                res.Message = ex.Message;
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
                res.Message = ex.Message;
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
            var config = ServiceProvider.GetService<ParseRequestService>();
            if(config != null)
            {
                var svc = ServiceProvider.GetService<NodeAPIClient.Services.NodeInfoService>();
                if (svc != null)
                {
                    try
                    {
                        var result = svc.GetNodeInfo(config.GetNetworkIp(request), config.GetDiagnosticPort(request), config.GetRequestTimeout(request));
                        if (result != null)
                        {
                            return Ok(result);
                        }
                        return Ok(new ResponseNodeInfoModel()
                        {
                            Success = false,
                            Message = "failed to get requested info, check the remote node is alive"
                        });
                    }
                    catch (Exception x)
                    {
                        return Ok(new ResponseNodeInfoModel()
                        {
                            Success = false,
                            Message = x.Message
                        });
                    }
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
