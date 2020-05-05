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
        public ActionResult<BalanceResponseApiModel> GetBalance(RequestKeyApiModel model)
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
        [HttpPost("GetWalletInfo")]
        public ActionResult<ResponseApiModel> GetWalletInfo(RequestKeyApiModel model)
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

        [AuthKeyFilter]
        [HttpPost("GetTransactionInfo")]
        public ActionResult<ResponseApiModel> GetTransactionInfo(RequestGetterApiModel model)
        {
            InitAuthKey(model);
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


        [AuthKeyFilter]
        [HttpPost("GetFilteredTransactionsList")]
        public ActionResult<FilteredTransactionsResponseModel> GetFilteredTransactionsList(RequestFilteredListModel model)
        {
            var response = new FilteredTransactionsResponseModel();
            try
            {
                response = ServiceProvider.GetService<MonitorService>().GetFilteredTransactions(model);

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
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
        [HttpPost("GetTransactionsByWallet")]
        public ActionResult<ResponseApiModel> GetTransactionsByWallet(RequestTransactionsApiModel model)
        {
            InitAuthKey(model);

            WalletTransactionsResponseApiModel res;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetWalletTransactions(model);

                res.Success = true;
            }
            catch (Exception ex)
            {
                res = new WalletTransactionsResponseApiModel();
                res.Success = false;
                res.Message = ex.Message;
            }

            return Ok(res);
        }

        [AuthKeyFilter]
        [HttpPost("GetTokenBalance")]
        public ActionResult<TokensResponseApiModel> GetTokenBalance(RequestTokensApiModel model)
        {
            InitAuthKey(model);
            BalanceResponseApiModel res;
            TokensResponseApiModel ret;
            try
            {
                res = ServiceProvider.GetService<MonitorService>().GetBalance(model);
                ret = new TokensResponseApiModel();
                string[] tkns = model.Tokens.Split(", ");
                foreach (var tok in res.Tokens) {
                    foreach (var tt in tkns)
                    {
                        if (tok.Alias==tt || tok.Name ==tt)
                        {
                            ret.Tokens.Add(tok);
                        }
                    }
                }
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret = new TokensResponseApiModel();
                ret.Success = false;
                ret.Message = ex.Message;
            }

            return Ok(ret);
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
                return StatusCode(InternalServerError, new ResponseBlocksModel()
                {
                    Success = false,
                    Message = ex.Message
                });
            }

            return new ContentResult()
            {
                Content = json,
                ContentType = "application/json",
                StatusCode = 200
            };
        }

        const int InternalServerError = 500;

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
                        return BadRequest(new ResponseNodeInfoModel()
                        {
                            Success = false,
                            Message = "failed to get requested info, check the node is alive"
                        });
                    }
                    catch (Exception x)
                    {
                        return StatusCode(InternalServerError, new ResponseNodeInfoModel()
                        {
                            Success = false,
                            Message = x.Message
                        });
                    }
                }
            }
            return StatusCode(InternalServerError, new ResponseNodeInfoModel()
            {
                Success = false,
                Message = "Server is not properly configured"
            });
        }
    }
}
