using System;
using CS.Service.RestApiNode;
using CS.Service.RestApiNode.Models;
using CS.WebApi.Controllers;
using CS.WebApi.Infrasructure.AttributeFilter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CS.WebApi.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitorController : ApiBaseController
    {
        private readonly IServiceProvider serviceProvider;

        public MonitorController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [AuthKeyFilter]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("GetBalance")]
        public ActionResult<BalanceResponseApiModel> GetBalance(RequestKeyApiModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(new BalanceResponseApiModel
                    {
                        Success = false,
                        Message = "Model is not valid"
                    });
                }

                InitAuthKey(model);

                var res = serviceProvider.GetService<MonitorService>().GetBalance(model);
                res.Success = true;

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new BalanceResponseApiModel
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [AuthKeyFilter]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("GetWalletInfo")]
        public ActionResult<ResponseApiModel> GetWalletInfo(RequestKeyApiModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(new WalletDataResponseApiModel()
                    {
                        Success = false,
                        MessageError = "Model is not valid"
                    });
                }

                InitAuthKey(model);

                var res = serviceProvider.GetService<MonitorService>().GetWalletData(model);
                res.Success = true;

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new WalletDataResponseApiModel()
                {
                    Success = false,
                    MessageError = ex.Message
                });
            }
        }


        [AuthKeyFilter]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("GetContract")]
        public ActionResult<ResponseApiModel> GetContract(RequestKeyApiModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(new ResponseApiModel()
                    {
                        Success = false,
                        Message = "Model is not valid"
                    });
                }

                InitAuthKey(model);

                var res = serviceProvider.GetService<MonitorService>().GetContract(model);
                res.Success = true;

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiModel()
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [AuthKeyFilter]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("GetTransactionInfo")]
        public ActionResult<ResponseApiModel> GetTransactionInfo(RequestGetterApiModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(new TransactionInfo()
                    {
                        Success = false,
                        Message = "Model is not valid"
                    });
                }

                InitAuthKey(model);

                var res = serviceProvider.GetService<MonitorService>().GetTransaction(model);
                res.Success = true;

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new TransactionInfo()
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [AuthKeyFilter]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("GetFilteredTransactionsList")]
        public ActionResult<FilteredTransactionsResponseModel> GetFilteredTransactionsList(RequestFilteredListModel model)
        {
            var response = new FilteredTransactionsResponseModel();
            try
            {
                response = serviceProvider.GetService<MonitorService>().GetFilteredTransactions(model);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }

        [AuthKeyFilter]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("GetTransactionsByWallet")]
        public ActionResult<ResponseApiModel> GetTransactionsByWallet(RequestTransactionsApiModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(new WalletTransactionsResponseApiModel()
                    {
                        Success = false,
                        Message = "Model is not valid"
                    });
                }

                InitAuthKey(model);

                var res = serviceProvider.GetService<MonitorService>().GetWalletTransactions(model);
                res.Success = true;

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new WalletTransactionsResponseApiModel()
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [AuthKeyFilter]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("GetTokenBalance")]
        public ActionResult<TokensResponseApiModel> GetTokenBalance(RequestTokensApiModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(new TokensResponseApiModel()
                    {
                        Success = false,
                        Message = "Model is not valid"
                    });
                }

                InitAuthKey(model);

                var res = serviceProvider.GetService<MonitorService>().GetBalance(model);
                var ret = new TokensResponseApiModel();

                string[] tkns = model.Tokens.Split(", ");
                foreach (var tok in res.Tokens)
                {
                    foreach (var tt in tkns)
                    {
                        if (tok.Alias == tt || tok.Name == tt)
                        {
                            ret.Tokens.Add(tok);
                        }
                    }
                }

                ret.Success = true;

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest(new TokensResponseApiModel()
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [AuthKeyFilter]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("GetListTransactionByBlock")]
        public ActionResult<ResponseApiModel> GetListTransactionByBlockId(RequestGetterApiModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(new ResponseApiModel()
                    {
                        Success = false,
                        Message = "Model is not valid"
                    });
                }

                InitAuthKey(model);

                var res = serviceProvider.GetService<MonitorService>().GetListTransactionByBlockId(model);
                res.Success = true;

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiModel()
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }


        [AuthKeyFilter]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("GetListTransactionByLastBlock")]
        public ActionResult<ResponseApiModel> GetListTransactionByLastBlock(RequestGetterApiModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(new ResponseApiModel()
                    {
                        Success = false,
                        Message = "Model is not valid"
                    });
                }

                InitAuthKey(model);

                var res = serviceProvider.GetService<MonitorService>().GetLastBlockId(model);
                model.BlockId = res.BlockId;
                res = serviceProvider.GetService<MonitorService>().GetListTransactionByBlockId(model);
                res.Success = true;

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiModel()
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [AuthKeyFilter]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("GetLastBlockId")]
        public ActionResult<ResponseApiModel> GetLastBlockId(RequestGetterApiModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(new ResponseApiModel()
                    {
                        Success = false,
                        Message = "Model is not valid"
                    });
                }

                InitAuthKey(model);

                var res = serviceProvider.GetService<MonitorService>().GetLastBlockId(model);
                model.BlockId = res.BlockId;
                res = serviceProvider.GetService<MonitorService>().GetListTransactionByBlockId(model);
                res.Success = true;

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiModel()
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [AuthKeyFilter]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("GetTransactionByInnerId")]
        public ActionResult<ResponseApiModel> GetTransactionByInnerId(RequestGetterApiModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(new ResponseApiModel()
                    {
                        Success = false,
                        Message = "Model is not valid"
                    });
                }

                return Ok(new ResponseApiModel()
                {
                    Success = false,
                    Message = "Method is not implemented yet"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiModel()
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [AuthKeyFilter]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("GetBlocks")]
        public ActionResult<ResponseBlocksModel> GetBlocks(RequestBlocksModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(new ResponseBlocksModel()
                    {
                        Success = false,
                        Message = "Model is not valid"
                    });
                }

                InitAuthKey(model);

                var json = serviceProvider.GetService<BlocksService>().GetBlocksRange(model);
                return new ContentResult()
                {
                    Content = json,
                    ContentType = "application/json",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBlocksModel()
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [AuthKeyFilter]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPost("GetNodeInfo")]
        public ActionResult<ResponseNodeInfoModel> GetNodeInfo(RequestNodeInfoModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    return BadRequest(new ResponseNodeInfoModel()
                    {
                        Success = false,
                        Message = "Model is not valid"
                    });
                }

                InitAuthKey(model);

                var config = serviceProvider.GetService<ParseRequestService>();
                if (config == null)
                {
                    return BadRequest(new ResponseNodeInfoModel()
                    {
                        Success = false,
                        Message = "Server is not properly configured"
                    });
                }

                var svc = serviceProvider.GetService<NodeAPIClient.Services.NodeInfoService>();
                if (svc == null)
                {
                    return BadRequest(new ResponseNodeInfoModel()
                    {
                        Success = false,
                        Message = "Requested service is null"
                    });
                }

                var result = svc.GetNodeInfo(config.GetNetworkIp(model), config.GetDiagnosticPort(model), config.GetRequestTimeout(model));
                if (result != null)
                {
                    return Ok(result);
                }

                return BadRequest(new ResponseNodeInfoModel()
                {
                    Success = false,
                    Message = "Failed to get requested info, check the node is alive"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseNodeInfoModel()
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
